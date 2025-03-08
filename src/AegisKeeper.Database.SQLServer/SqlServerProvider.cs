using AegisKeeper.Core.Entities;
using AegisKeeper.Core.Interfaces;
using AegisKeeper.Core.Models;
using AegisKeeper.Core.Models.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Dac;

namespace AegisKeeper.Database.SQLServer;

public class SqlServerProvider: IDatabase
{
    private readonly string _backupOutputPath = Path.GetTempPath() + Guid.NewGuid();
    private readonly ILogger<SqlServerProvider> _logger;
    private readonly string _connectionString;

    public SqlServerProvider(IOptions<AegisKeeperSettings> options, ILogger<SqlServerProvider> logger)
    {
        _connectionString = options.Value.ConnectionString;
        ConnectionStringValidator.Validate(_connectionString, out var sqlConnectionStringBuilder);

        _logger = logger;
        Servername = sqlConnectionStringBuilder.DataSource;
    }
    
    public string Servername { get; }

    public Task<Stream> BackupAsync(Backup backup, CancellationToken cancellationToken = default)
    {
        try
        {
            if(backup.DatabaseProvider is not DatabaseProviders.SQLServer)
                throw new ApplicationException($"Unexpected database provider type: {backup.DatabaseProvider.ToString()}");

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
            _logger.LogError(e, "Error while backing up '{DatabaseName}' database from '{ServerName}'",
                backup.Database, Servername);

            throw;
        }
    }
}