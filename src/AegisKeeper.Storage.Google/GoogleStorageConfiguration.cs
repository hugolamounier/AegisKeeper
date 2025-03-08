using AegisKeeper.Core.Interfaces;

namespace AegisKeeper.Storage.Google;

public class GoogleStorageConfiguration: IStorageConfiguration
{
    public string StorageName { get; set; }
    public string ContainerName { get; set; }
    public GoogleCredentialType GoogleCredentialType { get; set; }
    public string Credential { get; set; }
}

public enum GoogleCredentialType
{
    Json = 1,
    File = 2
}