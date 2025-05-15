
using System.Reflection;

namespace MessageBrocker.Interfaces;

public interface IMessageBusStrategy
{
    void Register(MessageBusConfig config, Assembly? assembly = null);
}