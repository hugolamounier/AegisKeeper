namespace AegisKeeper.Core.Interfaces;

public interface IStorage
{
    Task UploadAsync(string fileName, string folder, Stream content);
}