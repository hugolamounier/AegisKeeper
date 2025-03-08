using AegisKeeper.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AegisKeeper.Infrastructure.Mappings;

public abstract class BaseMapping<TEntity>: IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.MapBase();
        Map(builder);
    }

    protected abstract void Map(EntityTypeBuilder<TEntity> builder);
}

public static class MappingExtension
{
    public static void MapBase<T>(this EntityTypeBuilder<T> modelBuilder) where T : BaseEntity
    {
        modelBuilder.HasKey(nameof(BaseEntity.Id));

        modelBuilder.HasQueryFilter(e => e.DeletedAt == null);
        
        modelBuilder.Property(nameof(BaseEntity.Id)).IsRequired().HasDefaultValueSql("NEWID()");
        modelBuilder.Property(nameof(BaseEntity.CreatedAt)).IsRequired().HasDefaultValueSql("getutcdate()");
        modelBuilder.Property(nameof(BaseEntity.UpdatedAt)).IsRequired(false);
        modelBuilder.Property(nameof(BaseEntity.DeletedAt)).IsRequired(false);

        modelBuilder.ToTable(typeof(T).Name + "s");
    }
}