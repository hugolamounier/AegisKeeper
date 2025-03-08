using AegisKeeper.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AegisKeeper.Storage.Google;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogleStorage(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IStorage, GoogleStorageClient>();

        return serviceCollection;
    }
}