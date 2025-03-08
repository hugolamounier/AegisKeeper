using AegisKeeper.Core.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AegisKeeper.Storage.Google;

public class GoogleStorageClient: IStorage
{
    private readonly GoogleStorageConfiguration _storageConfiguration;
    private readonly ILogger<GoogleStorageClient> _logger;
    private readonly StorageClient _storageClient;
    
    public GoogleStorageClient(IOptions<GoogleStorageConfiguration> storageConfiguration, ILogger<GoogleStorageClient> logger)
    {
        _storageConfiguration = storageConfiguration.Value;
        _logger = logger;
        
        var googleCredential = _storageConfiguration.GoogleCredentialType switch
        {
            GoogleCredentialType.Json => GoogleCredential.FromJson(_storageConfiguration.Credential),
            GoogleCredentialType.File => GoogleCredential.FromFile(_storageConfiguration.Credential),
            _ => throw new InvalidOperationException("Invalid Google Storage credential configuration.")
        };
        
        _storageClient = StorageClient.Create(googleCredential);
    }
    
    public Task UploadAsync(string fileName, string folder, Stream content)
    {
        try
        {
            var bucketName = _storageConfiguration.StorageName;
            var objectName = _storageConfiguration.ContainerName + "/" + folder + "/" + fileName;
            
           return _storageClient
                .UploadObjectAsync(bucketName, objectName, "application/octet-stream", content);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while uploading the object '{FileName}' to Google storage", fileName);
            throw;
        }
    }
}