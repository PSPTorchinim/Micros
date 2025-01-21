using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;

namespace Shared.Services.Database
{
    public static class UseDatabase
    {
        public static void ConfigureSqlServer<TContext>(IServiceCollection services) where TContext : DbContext
        {
            services.AddDbContext<TContext>(opt =>
            {
                var host = Environment.GetEnvironmentVariable("DATABASE_HOST");
                var user= Environment.GetEnvironmentVariable("DATABASE_USER");
                var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
                var catalog = Environment.GetEnvironmentVariable("DATABASE_CATALOG");
                opt.UseSqlServer($"Data Source={host};Initial Catalog={catalog};Persist Security Info=True;User ID={user};Password={password};TrustServerCertificate=True;",options => {
                    options.EnableRetryOnFailure(5);
                });
            }, ServiceLifetime.Singleton);
        }

        public static void ConfigureMongoDBServer(IServiceCollection services)
        {
            var host = Environment.GetEnvironmentVariable("DATABASE_HOST");
            var user = Environment.GetEnvironmentVariable("DATABASE_USER");
            var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
            var settings = MongoClientSettings.FromConnectionString($"mongodb://{user}:{password}@{host}");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            services.AddSingleton(client);
        }

        private static async Task<WebApplication> UseDatabaseScopeAsync<C, P>(this WebApplication app, Func<C, Task<bool>> action) where C: DbContext
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<C>();
                try
                {
                    await action(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<P>>();
                    logger.LogError(ex, "An error occurred migration the DB.");
                }
            }
            return app;
        }

        public static async Task<WebApplication> UseSQLServerAsync<C, P>(WebApplication app) where C : DbContext
        {
            var x = await app.UseDatabaseScopeAsync<C, P>(async context =>
            {
                try
                {
                    await context.Database.EnsureCreatedAsync();
                    await context.Database.MigrateAsync();
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            });
            return x;
        }
    }
}