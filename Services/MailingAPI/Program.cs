using Shared.Configurations;
using Shared.Services.App;
using Shared.Data;
using Shared.Services.Database;
using Shared.Services.Run;
using MailingAPI.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

var systemConfig = new SystemConfiguration();
ConfigurationBinder.Bind(builder.Configuration, systemConfig);

builder.Services.BuildBasicServices(systemConfig, "MailingMicroservice", "v0.0.1");
///builder.Services.AddHostedService<MailingBackgroundService>();
///builder.Services.BuildScope<Program, SeedData, IdentityScope>(UseDatabase.ConfigureSqlServer<IdentityContext>);

var app = builder.Build();

app.BuildBasicApp();
///await app.BuildServicesAppAsync<SeedData>(UseDatabase.UseSQLServerAsync<IdentityContext, Program>);

app.Run();
