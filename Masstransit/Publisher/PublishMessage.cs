using MessageBrocker.Interfaces;
using MessageBrocker.Messages;

namespace Publisher;

public class PublishMessage(IMessageBusPublisher<Message1> publisher)
{
    public async Task Publish(Message1 message)
    {
        await publisher.PublishAsync(message);
    }
}