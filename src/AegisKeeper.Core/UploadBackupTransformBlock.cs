using System.Threading.Tasks.Dataflow;
using AegisKeeper.Core.Entities;
using AegisKeeper.Core.Models;
using Microsoft.Extensions.Logging;

namespace AegisKeeper.Core;

// UploadBackupTransformBlock
public partial class BackupPipeline
{
    private readonly TransformBlock<Backup, Backup> _uploadBackupBlock = null!;
    private static Predicate<Backup> UploadBackupBlockCondition => backup =>
        backup 
            is { CurrentStep: PipelineSteps.Backup, Failed: false, Content: not null } 
            or { CurrentStep: PipelineSteps.SaveOnStorage, Failed: true };
    
    private TransformBlock<Backup, Backup> UploadBackupBlock() => new(async backup =>
    {
        try
        {
            ArgumentNullException.ThrowIfNull(backup.Content);

            backup.CurrentStep = PipelineSteps.SaveOnStorage;
            using var scope = _serviceScopeFactory.CreateScope();
            var storage = GetStorage(scope);
            
            var backupFileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".bacpac";
            var folderName = backup.Database;
            
            await storage.UploadAsync(backupFileName, folderName, backup.Content);

            await backup.Content.DisposeAsync();

            return backup;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while sending the backup content to the storage");

            backup.Failed = true;

            return backup;
        }
    }, new ExecutionDataflowBlockOptions
    {
        CancellationToken = _cancellationToken,
        MaxDegreeOfParallelism = Environment.ProcessorCount * 4,
    });
}