using MessageBrocker.Infrestructure;
using MessageBrocker.Infrestructure.Masstransit;
using MessageBrocker.Infrestructure.RabbitMQ;
using MessageBrocker.Interfaces;
using MessageBrocker.Messages;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var config = builder.Configuration.GetSection("MessageBusConfig").Get<MessageBusConfig>();
if (config != null)
    builder.Services.AddMessageBus<MassTransitStrategy>(config);

//builder.Services.AddScoped<PublishMessage>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.MapGet("/1", async (IMessageBusPublisher<Message1> publish) =>
{
    await publish.PublishAsync(new Message1
    {
        Prop11 = 11,
        Prop12 = 12
    });
});

app.MapGet("/2", async (IMessageBusPublisher<Message2> publish) =>
{
    await publish.PublishAsync(new Message2
    {
        Prop21 = 21,
        Prop22 = 22
    });
});

app.Run();
