using MassTransit;
using MessageBrocker.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MessageBrocker.Infrestructure.Masstransit;

public class MassTransitStrategy(IServiceCollection services) : IMessageBusStrategy
{
    private readonly string RabbitMQHostFormat = "RabbitMQ://{0}:{1}/";
    public void Register(MessageBusConfig config, Assembly? assembly = null)
    {
        var host = string.Format(RabbitMQHostFormat, config.Host, config.Port);

        services.AddScoped(typeof(IMessageBusPublisher<>), typeof(MassTransitPubliser<>));

        services.AddMassTransit(x =>
        {
            if (assembly is not null)
            {
                x.AddConsumers(assembly);
                x.SetEndpointNameFormatter(new CustomEndpointNameFormatter(assembly.GetName().Name));
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, host =>
                {
                    host.Username(config.UserName);
                    host.Password(config.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

    }
}

public static class MassTransitConfigurator
{
    public static void AddConsumers(this IBusRegistrationConfigurator x, Assembly? assembly)
    {
        if (assembly is null)
            return;

        var consumers = MessageBusHelper.FindConsumers(assembly);

        foreach (var consumer in consumers)
        {
            x.AddConsumer(typeof(MassTransitSubscriber<>).MakeGenericType(consumer.MessageType));
        }
    }
}

public class CustomEndpointNameFormatter(string prefix) : IEndpointNameFormatter
{
    public string Separator => throw new NotImplementedException();

    public string SanitizeName(string name)
    {
        return Regex.Replace(name, @"[^a-zA-Z0-9\.\-]", "-").ToLowerInvariant();
    }
    public string Consumer<T>()
        where T : class, IConsumer
    {
        return SanitizeName(MessageBusHelper.GenerateQueueName<T>());
    }

    public string TemporaryEndpoint(string tag)
    {
        return SanitizeName($"{prefix}-temp-{tag}-{Guid.NewGuid().ToString("N").Substring(0, 6)}");
    }

    public string Saga<T>()
        where T : class, ISaga
    {
        return SanitizeName($"{prefix}.saga.{typeof(T).Name}");
    }

    public string ExecuteActivity<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
        where TArguments : class
    {
        return SanitizeName($"{prefix}.execute.{typeof(T).Name}");
    }

    public string CompensateActivity<T, TLog>()
        where T : class, ICompensateActivity<TLog>
        where TLog : class
    {
        return SanitizeName($"{prefix}.compensate.{typeof(T).Name}");
    }

    public string Message<T>()
        where T : class
    {
        return SanitizeName($"{prefix}.message.{typeof(T).Name}");
    }
}
