using Shared.Configurations;
using Shared.Services.App;
using Music.Data;
using Shared.Services.Database;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shared.Services.Run;

var builder = WebApplication.CreateBuilder(args);

var systemConfig = new SystemConfiguration();
ConfigurationBinder.Bind(builder.Configuration, systemConfig);

builder.Services.BuildBasicServices(systemConfig, "Music", "v0.0.1");
builder.Services.BuildScope<Program, SeedData, MusicScope>(UseDatabase.ConfigureSqlServer<MusicContext>);

var app = builder.Build();

app.BuildBasicApp();
await app.BuildServicesAppAsync<SeedData>(UseDatabase.UseSQLServerAsync<MusicContext, Program>);

app.Run();
