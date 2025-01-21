using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.RabbitMQ
{
    public class RabbitMQBackgroundService : BackgroundService
    {

        private readonly RabbitMQConsumerService _consumerService;

        public RabbitMQBackgroundService(RabbitMQConsumerService consumerService = null)
        {
            _consumerService = consumerService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
