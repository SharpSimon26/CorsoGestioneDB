using CorsoGestioneDB.Application.Configuration;
using CorsoGestioneDB.Application.Engine;
using CorsoGestioneDB.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                     .AddUserSecrets<Program>();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

using var host = builder.Build();

await host.Services
    .GetRequiredService<ImportEngine>()
    .RunAsync();
