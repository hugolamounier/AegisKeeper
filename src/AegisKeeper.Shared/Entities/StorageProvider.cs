using AegisKeeper.Core.Models;

namespace AegisKeeper.Shared.Entities;

public class StorageProvider: BaseEntity
{
    public StorageProviderType Provider { get; set; }
    
    /// <summary>
    /// The name of your storage account on the selected storage provider
    /// </summary>
    /// <remarks>
    /// For Google Cloud Storage, this should be filled with the Bucket name
    /// For Azure Storage, this should be filled with the storage service name
    /// </remarks>
    public string StorageName { get; set; }
    
    /// <summary>
    /// The name of folder/container where the backups are going to be stored
    /// </summary>
    public string ContainerName { get; set; }
    
    public string? Credential { get; set; }
    
    public string? ConnectionString { get; set; }
}