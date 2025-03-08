using AegisKeeper.Core.Entities;
using AegisKeeper.Core.Models;

namespace AegisKeeper.Core.Interfaces;

public interface IDatabase
{
    string Servername { get; }
    Task<Stream> BackupAsync(Backup backup, CancellationToken cancellationToken = default);
}