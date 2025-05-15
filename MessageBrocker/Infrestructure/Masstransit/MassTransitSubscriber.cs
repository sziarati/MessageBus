using MassTransit;
using MessageBrocker.Interfaces;

namespace MessageBrocker.Infrestructure.Masstransit;

public class MassTransitSubscriber<TMessage>(IMessageBusConsumer<TMessage> consumer) : IConsumer<TMessage> where TMessage : class
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await consumer.ConsumeAsync(context.Message);
    }
}