namespace MessageBrocker.Infrestructure;

public class ConsumerInfo
{
    public required Type Type { get; set; }
    public required Type MessageType { get; set; }
}