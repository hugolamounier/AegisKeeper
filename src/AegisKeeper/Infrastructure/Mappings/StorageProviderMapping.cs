using AegisKeeper.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AegisKeeper.Infrastructure.Mappings;

public class StorageProviderMapping: BaseMapping<StorageProvider>
{
    protected override void Map(EntityTypeBuilder<StorageProvider> builder)
    {
        builder.Property(x => x.Provider).IsRequired();
        builder.Property(x => x.StorageName).IsRequired();
        builder.Property(x => x.ContainerName).IsRequired();
        builder.Property(x => x.Credential).IsRequired(false);
        builder.Property(x => x.ConnectionString).IsRequired(false);
    }
}