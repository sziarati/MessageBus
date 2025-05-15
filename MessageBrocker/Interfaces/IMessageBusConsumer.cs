
namespace MessageBrocker.Interfaces;

public interface IMessageBusConsumer<TMessage> where TMessage : class
{
    public Task ConsumeAsync(TMessage message);
}



