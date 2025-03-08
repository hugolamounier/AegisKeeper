using System.Threading.Tasks.Dataflow;
using AegisKeeper.Shared.Entities;
using AegisKeeper.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AegisKeeper.Core;

public partial class BackupPipeline
{
    private static IDatabase GetDatabase(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<IDatabase>();
    private static IStorage GetStorage(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<IStorage>();

    private readonly DataflowLinkOptions _flowOptions = new () { PropagateCompletion = true };
    private readonly BufferBlock<Backup> _backupQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<BackupPipeline> _logger;
    private readonly CancellationToken _cancellationToken;

    public BackupPipeline(IServiceScopeFactory serviceScopeFactory, ILogger<BackupPipeline> logger, IHostApplicationLifetime lifetime)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _cancellationToken = lifetime.ApplicationStopping;

        _backupQueue = new BufferBlock<Backup>(new DataflowBlockOptions
        {
            CancellationToken = _cancellationToken
        });

        _backupDatabaseBlock = BackupDatabaseBlock();
        _uploadBackupBlock = UploadBackupBlock();

        DefineInputFlow();
        DefineBackupFlow();
        DefineStorageFlow();

        _backupQueue.LinkTo(DataflowBlock.NullTarget<Backup>());
        
    }

    private void DefineInputFlow()
    {
        _backupQueue.LinkTo(_backupDatabaseBlock, _flowOptions, BackupBlockCondition);
        _backupQueue.LinkTo(_uploadBackupBlock, _flowOptions, UploadBackupBlockCondition);
    }

    private void DefineBackupFlow()
    {
        _backupDatabaseBlock.LinkTo(_uploadBackupBlock, _flowOptions, UploadBackupBlockCondition);
        _backupDatabaseBlock.LinkTo(_backupQueue, _flowOptions, x => x.Failed);
    }

    private void DefineStorageFlow()
    {
        _uploadBackupBlock.LinkTo(_backupQueue, _flowOptions, x => x.Failed);
    }
    
    public Task EnqueueAsync(Backup backup) => _backupQueue.SendAsync(backup, _cancellationToken);
}