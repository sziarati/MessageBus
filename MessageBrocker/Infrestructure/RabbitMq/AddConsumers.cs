using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using MessageBrocker.Interfaces;
using MessageBrocker.Infrestructure.RabbitMQ;
using RabbitMQ.Client.Events;
using System.Reflection;

namespace MessageBrocker.Infrestructure.RabbitMq;

public class AddConsumers(IServiceProvider serviceProvider, IRabbitMQConnection connection, Assembly assembly) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var consumers = MessageBusHelper.FindConsumers(assembly);

        foreach (var consumer in consumers)
        {
            await AddConsumerAsync(consumer, cancellationToken);
        }
    }

    private async Task AddConsumerAsync(ConsumerInfo consumer, CancellationToken cancellationToken)
    {
        var channel = await connection.GetConnection(queueName: MessageBusHelper.GenerateQueueName(consumer.MessageType));
        var baseConsumer = new AsyncEventingBasicConsumer(channel);

        baseConsumer.ReceivedAsync += async (model, eventArgs) =>
        {
            var message = JsonSerializer.Deserialize(Encoding.UTF8.GetString(eventArgs.Body.ToArray()), consumer.MessageType);

            var scope = serviceProvider.CreateScope();
            var consumerInterface = typeof(IMessageBusConsumer<>).MakeGenericType(consumer.MessageType);
            var handler = scope.ServiceProvider.GetRequiredService(consumerInterface);
            var method = consumerInterface.GetMethod("ConsumeAsync");
            if (method != null)
            {
                await (Task)method.Invoke(handler, [message]);
            }
        };

        _ = await channel.BasicConsumeAsync(queue: MessageBusHelper.GenerateQueueName(consumer.MessageType),
                                            autoAck: true,
                                            consumer: baseConsumer,
                                            cancellationToken: cancellationToken);
    }
}
