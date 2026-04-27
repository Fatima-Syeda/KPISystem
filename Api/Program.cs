
using Api.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Api.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=postgres;Port=5432;Database=eventsdb;Username=user;Password=password"));
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

app.MapGet("/events", async (AppDbContext db) =>
{
    var events = await db.Events.ToListAsync();
    return Results.Ok(events);
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();