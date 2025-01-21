using Shared.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Yarp.ReverseProxy.Swagger;
using Shared.Services.RabbitMQ;
using Shared.Services.Security;
using Shared.Services.Database;
using Shared.Services.App;
using System.Reflection;

namespace Shared.Services.Run
{
    public static class ServicesBuilder
    {
        public static IServiceCollection BuildBasicServices(this IServiceCollection services, SystemConfiguration systemConfiguration, string name, string version, bool isApiGW = false)
        {
            services.AddControllers();
            services.AddHealthChecks().AddCheck<SampleHealthCheck>("Sample");
            services.BuildAuthentication(systemConfiguration);
            services.BuildSwagger(name, version, isApiGW);
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddEndpointsApiExplorer();
            services.AddLogging();
            services.AddCors(options =>
            {
                options.AddPolicy("cors",
                builder =>
                {
                    builder.WithOrigins(["http://localhost:8080"])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            services.AddScoped<RabbitMQProducerService>();
            services.AddScoped<RabbitMQConsumerService>();
            return services;
        }

        private static IServiceCollection BuildAuthentication(this IServiceCollection services, SystemConfiguration systemConfiguration)
        {
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = systemConfiguration.TokenConfiguration.ValidateIssuer,
                    ValidIssuer = systemConfiguration.TokenConfiguration.Issuer,
                    ValidateAudience = systemConfiguration.TokenConfiguration.ValidateAudience,
                    ValidAudience = systemConfiguration.TokenConfiguration.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")))
                };
            });
            return services;
        }

        private static IServiceCollection BuildSwagger(this IServiceCollection services, string name, string version, bool isApiGW = false)
        {
            services.AddSwaggerGen(c =>
            {
                if (isApiGW)
                    c.DocumentFilter<ReverseProxyDocumentFilter>();
                c.OperationFilter<AddHeaderParameter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"{name} Microservice",
                    Version = version
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        }, new string[]{}
                    }
                });
            });
            return services;
        }

        public static IServiceCollection BuildScope<P, S, Sc>(this IServiceCollection services, Action<IServiceCollection> configureDbContext) where S : IDatabaseInitializer where Sc : Scope, new()
        {
            services.AddHttpContextAccessor();
            new Sc().CreateScope(services);
            configureDbContext(services);
            services.AddAutoMapper(typeof(P));
            services.AddScoped(typeof(S));
            return services;
        }
    }
}
