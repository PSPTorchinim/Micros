using Shared.Configurations;
using Shared.Services.App;
using IdentityAPI.Data;
using Shared.Services.Database;
using Shared.Services.Run;

var builder = WebApplication.CreateBuilder(args);

var systemConfig = new SystemConfiguration();
ConfigurationBinder.Bind(builder.Configuration, systemConfig);

builder.Services.BuildBasicServices(systemConfig, "Identity", "v0.0.1");
builder.Services.BuildScope<Program, SeedData, IdentityScope>(UseDatabase.ConfigureSqlServer<IdentityContext>);

var app = builder.Build();

app.BuildBasicApp();
await app.BuildServicesAppAsync<SeedData>(UseDatabase.UseSQLServerAsync<IdentityContext, Program>);

app.Run();
