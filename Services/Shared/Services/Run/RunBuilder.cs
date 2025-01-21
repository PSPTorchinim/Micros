using Shared.Services.Database;
using Shared.Services.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Shared.Services.App
{
    public static class RunBuilder
    {
        public static WebApplication BuildBasicApp(this WebApplication app, Action<SwaggerOptions> options = null, Action < SwaggerUIOptions> uiOptions = null)
        {
            if (!app.Environment.IsProduction())
            {
                app.UseSwagger(options);
                app.UseSwaggerUI(uiOptions);
            }
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors("cors");
            app.UseAuthorization();
            app.MapHealthChecks("/healthz/live", new HealthCheckOptions
            {
                Predicate = _ => true // Perform all health checks
            });
            app.MapControllers();
            return app;
        }

        public static async Task<WebApplication> BuildServicesAppAsync<S>(this WebApplication app, Func<WebApplication, Task<WebApplication>> func) where S : IDatabaseInitializer
        {

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseMiddleware<SecureMiddleware>();
            });

            await func(app);

            await app.Services.CreateScope().ServiceProvider.GetRequiredService<S>().InitializeAsync();

            return app;
        }

        public static async Task<WebApplication> BuildServicesAppAsync<S>(this WebApplication app) where S : IDatabaseInitializer
        {

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseMiddleware<SecureMiddleware>();
            });

            await app.Services.CreateScope().ServiceProvider.GetRequiredService<S>().InitializeAsync();

            return app;
        }
    }
}
