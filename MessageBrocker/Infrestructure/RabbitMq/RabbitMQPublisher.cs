using MessageBrocker.Interfaces;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace MessageBrocker.Infrestructure.RabbitMQ;

public class RabbitMQPublisher<TMessage>(IRabbitMQConnection messageBus) : IMessageBusPublisher<TMessage> where TMessage : class
{
    public async Task PublishAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        using var connection = await messageBus.GetConnection(queueName: MessageBusHelper.GenerateQueueName(message.GetType()));

        await connection.BasicPublishAsync(
                            exchange: string.Empty,
                            routingKey: MessageBusHelper.GenerateQueueName(message.GetType()),
                            mandatory: true,
                            basicProperties: new BasicProperties(),
                            body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)),
                            cancellationToken: cancellationToken);

    }

    public Task SendAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
