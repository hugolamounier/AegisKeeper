using AegisKeeper.Core;
using AegisKeeper.Core.Entities;
using AegisKeeper.Core.Models;

namespace AegisKeeper;

public class Worker(BackupPipeline backupPipeline, ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Initializing AegisKeeper...");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var backup = new Backup
            {
                DatabaseProvider = DatabaseProviders.SQLServer,
                StorageProvider = StorageProviders.GoogleCloud,
                Database = "Geonew"
            };
            
            await backupPipeline.EnqueueAsync(backup);

            await Task.Delay(100000, stoppingToken);
        }
    }
}