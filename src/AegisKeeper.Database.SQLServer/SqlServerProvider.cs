using AegisKeeper.Shared.Entities;
using AegisKeeper.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Dac;

namespace AegisKeeper.Database.SQLServer;

public class SqlServerProvider: IDatabase
{
    private readonly string _backupOutputPath = Path.GetTempPath() + Guid.NewGuid();
    private readonly ILogger<SqlServerProvider> _logger;
    private readonly string _connectionString;

    public SqlServerProvider(ILogger<SqlServerProvider> logger, DatabaseServer databaseServer)
    {
        _logger = logger;
        _connectionString = databaseServer.ConnectionString;
        
        ConnectionStringValidator.Validate(_connectionString);
    }

    public Task<Stream> BackupAsync(Backup backup, CancellationToken cancellationToken = default)
    {
        try
        {
            var dacServices = new DacServices(_connectionString);
            
            dacServices.ExportBacpac(
                _backupOutputPath, 
                backup.Database,
                DacSchemaModelStorageType.Memory,
                cancellationToken: cancellationToken);
            
            if(!File.Exists(_backupOutputPath))
                throw new ApplicationException("No backup file was generated");
            
            return Task.FromResult<Stream>(File.OpenRead(_backupOutputPath));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while backing up '{DatabaseName}' database from Server: '{DatabaseServerId}'",
                backup.Database, backup.DatabaseServerId);

            throw;
        }
    }

    public void Dispose()
    {
        // ignored
    }
}