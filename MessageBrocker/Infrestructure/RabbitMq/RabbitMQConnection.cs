using MessageBrocker.Interfaces;
using RabbitMQ.Client;

namespace MessageBrocker.Infrestructure.RabbitMQ;

public interface IRabbitMQConnection
{
    Task<IChannel> GetConnection(string queueName);
}
public class RabbitMQConnection(MessageBusConfig config) : IRabbitMQConnection
{
    private readonly ConnectionFactory factory = new()
    {
        HostName = config.Host,
        Port = config.Port,
        UserName = config.UserName,
        Password = config.Password
    };

    public async Task<IChannel> GetConnection(string queueName)
    {
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null); //todo define queue template
        return channel;
    }
}
