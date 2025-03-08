using AegisKeeper.Core.Models;

namespace AegisKeeper.Shared.Entities;

public class DatabaseServer: BaseEntity
{
    public DatabaseProviderType Provider { get; set; }
    public string ConnectionString { get; set; }
}