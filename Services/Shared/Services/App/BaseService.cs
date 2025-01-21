using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Services.RabbitMQ;
using Shared.Services;
using Shared.Data.Exceptions;

namespace Shared.Services.App
{
    public class BaseService<TService> where TService : IService
    {
        public readonly ILogger<TService> _logger;
        public readonly IMapper _mapper;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly RabbitMQProducerService _rabbitMQProducerService;
        public readonly IServiceProvider _serviceProvider;

        public BaseService(ILogger<TService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, RabbitMQProducerService rabbitMQProducerService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _rabbitMQProducerService = rabbitMQProducerService;
            _serviceProvider = serviceProvider;
        }

        public string? GetClaim(string type)
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type.Equals(type))?.Value;
        }

        public async Task<string?> GetTokenAsync(string type)
        {
            return await _httpContextAccessor?.HttpContext?.GetTokenAsync(type);
        }
    }
}
