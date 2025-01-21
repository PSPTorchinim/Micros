using Shared.Configurations;
using Shared.Services.App;
using DocumentsAPI.Data;
using Shared.Services.Database;
using Shared.Services.Run;

var builder = WebApplication.CreateBuilder(args);

var systemConfig = new SystemConfiguration();
ConfigurationBinder.Bind(builder.Configuration, systemConfig);

builder.Services.BuildBasicServices(systemConfig, "Documents", "v0.0.1");
builder.Services.BuildScope<Program, SeedData, DocumentsScope>(UseDatabase.ConfigureMongoDBServer);

var app = builder.Build();

app.BuildBasicApp();
await app.BuildServicesAppAsync<SeedData>();

app.Run();
