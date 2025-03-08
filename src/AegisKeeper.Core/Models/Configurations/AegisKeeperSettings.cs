namespace AegisKeeper.Core.Models.Configurations;

public class AegisKeeperSettings
{
    /// <summary>
    /// The database provider connection string
    /// </summary>
    public string ConnectionString { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public DatabaseProviders DatabaseProvider { get; set; }
    
    public StorageProviders StorageProvider { get; set; }

    /// <summary>
    /// The backup configuration
    /// </summary>
    public ICollection<BackupConfiguration> BackupConfigurations { get; set; }
    
    /// <summary>
    /// The time zone the application should use to perform the operations
    /// </summary>
    public string TimeZone { get; set; } = "E. South America Standard Time";
}