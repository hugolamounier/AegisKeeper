using AegisKeeper.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace AegisKeeper.Infrastructure.Context;

public class ApplicationContext(DbContextOptions<ApplicationContext> options): DbContext(options)
{
    public DbSet<Backup> Backups { get; set; }
    public DbSet<DatabaseServer> DatabaseServers { get; set; }
    public DbSet<StorageProvider> StorageProviders { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}