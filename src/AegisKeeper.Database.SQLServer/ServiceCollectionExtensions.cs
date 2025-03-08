using AegisKeeper.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AegisKeeper.Database.SQLServer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlServerProvider(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDatabase, SqlServerProvider>();
        
        return serviceCollection;
    }
}