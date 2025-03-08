using AegisKeeper.Shared.Entities;
using AegisKeeper.Shared.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;

namespace AegisKeeper.Storage.Google;

public sealed class GoogleStorageClient: IStorage
{
    private readonly ILogger<GoogleStorageClient> _logger;
    private readonly StorageProvider _storageProvider;
    private readonly StorageClient _storageClient;
    
    public GoogleStorageClient(ILogger<GoogleStorageClient> logger, StorageProvider storageProvider)
    {
        _logger = logger;
        _storageProvider = storageProvider;

        var googleCredential = GoogleCredential.FromJson(storageProvider.Credential);
        
        _storageClient = StorageClient.Create(googleCredential);
    }
    
    public Task UploadAsync(Backup backup, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(backup.Content);
            
            var bucketName = _storageProvider.StorageName;
            var objectName = _storageProvider.ContainerName + "/" + backup.Database + "/" + backup.Filename;
            
           return _storageClient.UploadObjectAsync(bucketName, objectName, 
               "application/octet-stream", backup.Content, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while uploading the backup '{BackupId}' to Google storage", backup.Id);
            throw;
        }
    }

    public void Dispose()
    {
        _storageClient.Dispose();
    }
}