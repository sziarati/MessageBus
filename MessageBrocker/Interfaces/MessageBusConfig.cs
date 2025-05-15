namespace MessageBrocker.Interfaces;

public class MessageBusConfig
{
    public required string Host { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required int Port { get; set; }
}