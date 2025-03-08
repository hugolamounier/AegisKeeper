using AegisKeeper.Shared.Entities;

namespace AegisKeeper.Shared.Interfaces;

public interface IDatabase: IDisposable
{
    Task<Stream> BackupAsync(Backup backup, CancellationToken cancellationToken = default);
}