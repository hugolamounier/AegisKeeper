using AegisKeeper.Core.Models;
using AegisKeeper.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AegisKeeper.Infrastructure.Mappings;

public class BackupMapping: BaseMapping<Backup>
{
    protected override void Map(EntityTypeBuilder<Backup> builder)
    {
        builder.HasIndex(x => x.StorageProvider);
        builder.HasIndex(x => x.DatabaseServer);
        
        builder.Property(x => x.DatabaseServerId).IsRequired();
        builder.Property(x => x.StorageProviderId).IsRequired();
        builder.Property(x => x.Database).IsRequired();
        
        builder.Property(x => x.Failed)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(x => x.CurrentStep)
            .IsRequired()
            .HasDefaultValue(PipelineSteps.Initializing);

        builder.HasOne(x => x.DatabaseServer).WithMany();
        builder.HasOne(x => x.StorageProvider).WithMany();
    }
}