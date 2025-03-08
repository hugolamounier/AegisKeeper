using System.ComponentModel.DataAnnotations.Schema;
using AegisKeeper.Core.Models;

namespace AegisKeeper.Shared.Entities;

public class Backup: BaseEntity
{
    public Guid DatabaseServerId { get; set; }
    public Guid StorageProviderId { get; set; }
    public PipelineSteps CurrentStep { get; set; }
    public string Database { get; set; }
    public bool Failed { get; set; }
    
    [NotMapped] public Stream? Content { get; set; }
    [NotMapped] public string Filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".bacpac";
    
    public virtual DatabaseServer DatabaseServer { get; set; }
    public virtual StorageProvider StorageProvider { get; set; }
}