using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Shared.Services.App;
using Shared.Services.RabbitMQ;

namespace CompanyAPI.Services
{
    public interface IClientsService : IService{

    }

    public class ClientsService : BaseService<IClientsService>, IClientsService
    {
        public ClientsService(ILogger<IClientsService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, RabbitMQProducerService rabbitMQProducerService, IServiceProvider serviceProvider) : base(logger, mapper, httpContextAccessor, rabbitMQProducerService, serviceProvider)
        {
        }
    }
}