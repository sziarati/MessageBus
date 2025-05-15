using MessageBrocker.Interfaces;
using MessageBrocker.Messages;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace MessageBrocker.Infrestructure;

public class MessageBusHelper
{
    public static List<ConsumerInfo> FindConsumers(Assembly assembly)
    {
        return assembly.GetTypes()
                        .Where(type => !type.IsAbstract && !type.IsInterface)
                        .SelectMany(type => type.GetInterfaces()
                            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageBusConsumer<>))
                            .Select(i => new { Implementation = type, MessageType = i.GetGenericArguments()[0] }))
                        .GroupBy(x => x.MessageType)
                        .Select(g => new ConsumerInfo { Type = g.First().Implementation, MessageType = g.First().MessageType })
                        .ToList();
    }

    public static string GenerateQueueName(Type messageType) =>
        $"{messageType.Namespace?.Replace('.', '-').ToLowerInvariant()}.{messageType.Name.ToLowerInvariant()}";
    public static string GenerateQueueName<TMessage>() => 
        $"{typeof(TMessage).Namespace?.Replace('.', '-').ToLowerInvariant()}.{typeof(TMessage).Name.ToLowerInvariant()}";

}

public static class QueueNameFormatter
{
    public static string GetQueueName<TMessage>() => 
        $"{typeof(TMessage).Namespace.Replace('.', '_').ToLowerInvariant()}_{typeof(TMessage).Name.ToLowerInvariant()}" ;
}

