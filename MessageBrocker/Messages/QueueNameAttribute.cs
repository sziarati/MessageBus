namespace MessageBrocker.Messages;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class QueueNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
