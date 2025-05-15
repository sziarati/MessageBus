using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace MessageBrocker.Infrestructure.Masstransit;

public class AddConsumers(IBusRegistrationConfigurator services, Assembly assembly) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var consumers = MessageBusHelper.FindConsumers(assembly);

        foreach (var consumer in consumers)
        {
            services.AddConsumer(typeof(MassTransitSubscriber<>).MakeGenericType(consumer.MessageType));
        }

        await Task.CompletedTask;
    }
}
