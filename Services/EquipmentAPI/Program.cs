using Shared.Configurations;
using Shared.Services.App;
using Shared.Services.Database;
using EquipmentAPI.Data;
using Shared.Services.Run;

var builder = WebApplication.CreateBuilder(args);

var systemConfig = new SystemConfiguration();
ConfigurationBinder.Bind(builder.Configuration, systemConfig);

builder.Services.BuildBasicServices(systemConfig, "Gear", "v0.0.1");
builder.Services.BuildScope<Program, SeedData, GearScope>(UseDatabase.ConfigureSqlServer<GearContext>);

var app = builder.Build();

app.BuildBasicApp();
await app.BuildServicesAppAsync<SeedData>(UseDatabase.UseSQLServerAsync<GearContext, Program>);

app.Run();
