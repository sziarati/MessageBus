namespace MessageBrocker.Interfaces;

public interface IMessageBusSubscriber
{
    public Task SubscribeAsync();
}


