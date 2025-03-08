using System.ComponentModel.DataAnnotations.Schema;
using AegisKeeper.Core.Models;

namespace AegisKeeper.Core.Entities;

public class Backup
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DatabaseProviders DatabaseProvider { get; set; }
    public StorageProviders StorageProvider { get; set; }
    public PipelineSteps CurrentStep { get; set; }
    public string Database { get; set; }
    public bool Failed { get; set; }
    
    [NotMapped] public Stream? Content { get; set; }
}