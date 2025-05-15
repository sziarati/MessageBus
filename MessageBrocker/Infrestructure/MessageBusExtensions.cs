using MessageBrocker.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MessageBrocker.Infrestructure;

public static class MessageBusExtensions
{
    public static void AddMessageBus<TStrategy>(this IServiceCollection services,
        MessageBusConfig config) where TStrategy : IMessageBusStrategy
    {
        RegisterCommonServices(services, Assembly.GetCallingAssembly());

        var ctor = typeof(TStrategy).GetConstructor([typeof(IServiceCollection)]) 
                                    ?? throw new InvalidOperationException($"{typeof(TStrategy).Name} must have a constructor accepting IServiceCollection.");

        ((IMessageBusStrategy)ctor.Invoke([services]))
                                         .Register(config, Assembly.GetCallingAssembly());

    }

    private static void RegisterCommonServices(IServiceCollection services, Assembly assembly)
    {
        var consumers = MessageBusHelper.FindConsumers(assembly);

        foreach (var consumer in consumers)
        {
            services.AddScoped(typeof(IMessageBusConsumer<>).MakeGenericType(consumer.MessageType), consumer.Type);
        }
    }
}