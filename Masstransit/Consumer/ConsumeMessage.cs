using MessageBrocker.Interfaces;
using MessageBrocker.Messages;

namespace Consumer;

public class ConsumeMessage1(IConfiguration configuration) : IMessageBusConsumer<Message1>
{
    public Task ConsumeAsync(Message1 message)
    {
        var connectionString = configuration.GetConnectionString("ConnString");
        Console.WriteLine(message.Prop11 + " " + message.Prop12 + "connectionString:" + connectionString);
        return Task.CompletedTask;
    }
}

public class ConsumeMessage2 : IMessageBusConsumer<Message2>
{
    public Task ConsumeAsync(Message2 message)
    {
        Console.WriteLine(message.Prop21 + " " + message.Prop22);
        return Task.CompletedTask;
    }
}