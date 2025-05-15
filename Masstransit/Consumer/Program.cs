using MessageBrocker.Infrestructure;
using MessageBrocker.Infrestructure.Masstransit;
using MessageBrocker.Infrestructure.RabbitMQ;
using MessageBrocker.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
//RabbitMQ://localhost:5673

var config = builder.Configuration.GetSection("MessageBusConfig").Get<MessageBusConfig>();
if (config != null)
builder.Services.AddMessageBus<MassTransitStrategy>(config);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();
