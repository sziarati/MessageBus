using MassTransit;
using MessageBrocker.Interfaces;

namespace MessageBrocker.Infrestructure.Masstransit;

public class MassTransitPubliser<TMessage>(IPublishEndpoint publisher) : IMessageBusPublisher<TMessage> where TMessage : class
{
    public async Task PublishAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            await publisher.Publish(message, cancellationToken);
        }
        catch (Exception exp)
        {

        }
    }

    public Task SendAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
