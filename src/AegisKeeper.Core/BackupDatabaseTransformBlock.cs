using System.Threading.Tasks.Dataflow;
using AegisKeeper.Core.Entities;
using AegisKeeper.Core.Models;
using Microsoft.Extensions.Logging;

namespace AegisKeeper.Core;

// BackupDatabaseTransformBlock
public partial class BackupPipeline
{
    private readonly TransformBlock<Backup, Backup> _backupDatabaseBlock = null!;
    private static Predicate<Backup> BackupBlockCondition => backup => 
        backup 
            is { CurrentStep: PipelineSteps.Initializing } 
            or { CurrentStep: PipelineSteps.Backup, Failed: true };
    
    private TransformBlock<Backup, Backup> BackupDatabaseBlock() => new (async backup =>
    {
        try
        {
            backup.CurrentStep = PipelineSteps.Backup;
            
            using var scope = _serviceScopeFactory.CreateScope();
            var database = GetDatabase(scope);

            var databaseStream = await database.BackupAsync(backup, _cancellationToken);
            backup.Content = databaseStream;

            return backup;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while performing backup for '{DatabaseName}'",
                backup.Database);

            backup.Failed = true;

            return backup;
        }
    }, new ExecutionDataflowBlockOptions
    {
        CancellationToken = _cancellationToken,
        MaxDegreeOfParallelism = 1,
        BoundedCapacity = 1
    });
}