using System.Diagnostics;
using Shared.Configurations;
using Shared.Services.App;
using Shared.Services.Run;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;

var builder = WebApplication.CreateBuilder(args);

var systemConfig = new SystemConfiguration();
ConfigurationBinder.Bind(builder.Configuration, systemConfig);

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.BuildBasicServices(systemConfig, "ApiGateway", "v0.0.1", true);
var configuration = builder.Configuration.GetSection("ReverseProxy");
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")).AddSwagger(configuration).ConfigureHttpClient((context, handler) =>
{
    handler.ActivityHeadersPropagator = DistributedContextPropagator.CreatePassThroughPropagator();
});

var app = builder.Build();

app.BuildBasicApp(null, options => {
    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "Api Gateway");
    var config = app.Services.GetRequiredService<IOptionsMonitor<ReverseProxyDocumentFilterConfig>>().CurrentValue;
    foreach (var cluster in config.Clusters)
    {
        options.SwaggerEndpoint($"/swagger/{cluster.Key}/swagger.json", cluster.Key);
    }
});
app.MapReverseProxy();

app.Run();
