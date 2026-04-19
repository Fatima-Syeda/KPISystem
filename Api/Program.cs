
using Api.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.MapPost("/events", (Event evt) =>
{
    var factory = new ConnectionFactory() { HostName = "rabbitmq" };
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();

    channel.QueueDeclare(
        queue: "events",
        durable: false,
        exclusive: false,
        autoDelete: false);

    var message = JsonSerializer.Serialize(evt);
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: "",
        routingKey: "events",
        body: body);

    return Results.Ok(new { message = "Event sent to queue" });
});

app.Run();