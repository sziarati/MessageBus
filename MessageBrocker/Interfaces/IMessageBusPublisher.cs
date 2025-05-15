namespace MessageBrocker.Interfaces;

public interface IMessageBusPublisher<in TMessage> where TMessage : class // todo cancellation Token in publish and send
{
    Task PublishAsync(TMessage message, CancellationToken cancellationToken = default);
    Task SendAsync(TMessage message, CancellationToken cancellationToken = default);
}
