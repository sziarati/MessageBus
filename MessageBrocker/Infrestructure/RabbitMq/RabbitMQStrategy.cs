using MessageBrocker.Infrestructure.RabbitMq;
using MessageBrocker.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MessageBrocker.Infrestructure.RabbitMQ;

public class RabbitMQStrategy(IServiceCollection services) : IMessageBusStrategy
{
    public void Register(MessageBusConfig config, Assembly? assembly = null)
    {
        services.AddScoped(typeof(IMessageBusPublisher<>), typeof(RabbitMQPublisher<>));

        services.AddSingleton<IRabbitMQConnection>(sp =>
        {
            return new RabbitMQConnection(config);
        });

        if (assembly is not null)
        {
            services.AddHostedService(sp =>
            {
                var connection = sp.GetRequiredService<IRabbitMQConnection>();
                return new AddConsumers(sp, sp.GetRequiredService<IRabbitMQConnection>(), assembly);
            });
        }
    }
}
