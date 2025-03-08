using Microsoft.EntityFrameworkCore;

namespace AegisKeeper.Infrastructure.Context;

public class AppContext: DbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}