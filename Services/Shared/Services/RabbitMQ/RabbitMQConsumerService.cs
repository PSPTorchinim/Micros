using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Shared.Services.RabbitMQ
{
    public class RabbitMQConsumerService
    {
        private readonly IChannel Model;

        public RabbitMQConsumerService()
        {
            var connectionString = Environment.GetEnvironmentVariable("RabbitConnection");
            var connection = new ConnectionFactory { Uri = new Uri(connectionString) }.CreateConnectionAsync().Result;
            Model = connection.CreateChannelAsync().Result;
        }

        public async Task ReceiveMessageAsync<T>(Func<T, Task<bool>> action, string queueName)
        {
            await Model.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new AsyncEventingBasicConsumer(Model);
            consumer.ReceivedAsync += async (model, received) =>
            {
                var body = received.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var converted = JsonConvert.DeserializeObject<T>(message);
                await action(converted);
            };
            await Model.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
