using MudBlazor.Services;
using AegisKeeper.Components;
using AegisKeeper.Core;
using AegisKeeper.Core.Models;
using AegisKeeper.Core.Models.Configurations;
using AegisKeeper.Database.SQLServer;
using AegisKeeper.Shared.Models.Configurations;
using AegisKeeper.Storage.Google;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Logging.AddConsole();
builder.Services.Configure<AegisKeeperSettings>(builder.Configuration.GetSection("Settings"));

var settings = builder.Configuration.GetSection("Settings").Get<AegisKeeperSettings>()!;

builder.Services.AddSingleton<BackupPipeline>();
//builder.Services.AddHostedService<Worker>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AegisKeeper.Client._Imports).Assembly);

app.Run();
