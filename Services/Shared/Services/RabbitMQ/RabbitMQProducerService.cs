using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace Shared.Services.RabbitMQ
{
    public class RabbitMQProducerService{

        private readonly IChannel Model;

        public RabbitMQProducerService(){
            var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            var user = Environment.GetEnvironmentVariable("RABBITMQ_USER");
            var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
            var connection = new ConnectionFactory { Uri = new Uri($"amqp://{user}:{password}@{host}") }.CreateConnectionAsync().Result;
            Model = connection.CreateChannelAsync().Result;
        }

        public async Task SendMessage<T>(T message, string queueName){
            await Model.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            var body = JsonConvert.SerializeObject(message);
            var bytes = Encoding.UTF8.GetBytes(body);
            await Model.BasicPublishAsync(exchange: "", routingKey: queueName, mandatory: true, body: bytes);
            await Task.CompletedTask;
        }
    }
}