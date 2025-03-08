using AegisKeeper_Old;
using AegisKeeper.Core;
using AegisKeeper.Core.Models;
using AegisKeeper.Core.Models.Configurations;
using AegisKeeper.Database.SQLServer;
using AegisKeeper.Storage.Google;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole();
builder.Services.Configure<AegisKeeperSettings>(builder.Configuration.GetSection("Settings"));
builder.Services.Configure<GoogleStorageConfiguration>(builder.Configuration.GetSection("GoogleStorage"));

var settings = builder.Configuration.GetSection("Settings").Get<AegisKeeperSettings>()!;

switch (settings.DatabaseProvider)
{
    case DatabaseProviders.SQLServer:
    {
        builder.Services.AddSqlServerProvider();
        break;
    }
    
    default:
        throw new InvalidOperationException("Database provider is not supported");
}

switch (settings.StorageProvider)
{
    case StorageProviders.GoogleCloud:
    {
        builder.Services.AddGoogleStorage();
        break;
    }
    
    default:
        throw new InvalidOperationException("Storage provider is not supported");
}

builder.Services.AddSingleton<BackupPipeline>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();