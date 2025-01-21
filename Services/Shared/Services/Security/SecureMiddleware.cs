using Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Security
{
    public class SecureMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SecureMiddleware> _logger;

        public SecureMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<SecureMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            const string HeaderKeyName = "secure_value";
            context.Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            var hash = headerValue.FirstOrDefault();

            if (hash == null || !CheckSecureKey(hash))
            {
                await HandleUnauthorized(context);
                return;
            }
            _logger.LogDebug("AUTHORIZED");
            await _next(context);
        }

        private bool CheckSecureKey(string? hash)
        {
            if (hash == null) return false;
            try
            {
                var bytes = Convert.FromBase64String(hash);
                var decoded = Encoding.UTF8.GetString(bytes);

                ////var secureKey = _configuration.GetValue<string>("SecureKey");

                return decoded.Contains(Environment.GetEnvironmentVariable("SECURE_KEY"));
            }
            catch
            {
                return false;
            }
        }

        private async Task HandleUnauthorized(HttpContext context)
        {
            _logger.LogError("UNAUTHORIZED");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("UNAUTHORIZED");
        }
    }
}
