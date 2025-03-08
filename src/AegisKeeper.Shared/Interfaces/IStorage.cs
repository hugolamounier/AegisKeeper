using AegisKeeper.Shared.Entities;

namespace AegisKeeper.Shared.Interfaces;

public interface IStorage: IDisposable
{
    Task UploadAsync(Backup backup, CancellationToken cancellationToken = default);
}