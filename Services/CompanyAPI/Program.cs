using Shared.Configurations;
using Shared.Services.App;
using CompanyAPI.Data;
using Shared.Services.Database;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shared.Services.Run;

var builder = WebApplication.CreateBuilder(args);

var systemConfig = new SystemConfiguration();
ConfigurationBinder.Bind(builder.Configuration, systemConfig);

builder.Services.BuildBasicServices(systemConfig, "Brand", "v0.0.1");
builder.Services.BuildScope<Program, SeedData, BrandScope>(UseDatabase.ConfigureSqlServer<BrandContext>);

var app = builder.Build();

app.BuildBasicApp();
await app.BuildServicesAppAsync<SeedData>(UseDatabase.UseSQLServerAsync<BrandContext, Program>);

app.Run();
