using AegisKeeper.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AegisKeeper.Infrastructure.Mappings;

public class DatabaseServerMapping: BaseMapping<DatabaseServer>
{
    protected override void Map(EntityTypeBuilder<DatabaseServer> builder)
    {
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Provider).IsRequired();
        builder.Property(x => x.ConnectionString).IsRequired();
    }
}