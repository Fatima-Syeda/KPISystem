namespace Worker;

using global::Worker.Data;
using global::Worker.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;



public class EventWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EventWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };

        IConnection connection = null;

        while (connection == null)
        {
            try
            {
                connection = factory.CreateConnection();
            }
            catch
            {
                Console.WriteLine("RabbitMQ not ready, retrying in 5 seconds...");
                Thread.Sleep(5000);
            }
        }
        var channel = connection.CreateModel();

        channel.QueueDeclare("events", false, false, false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var evt = JsonSerializer.Deserialize<EventRecord>(json);

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Events.Add(evt);
            db.SaveChanges();

            Console.WriteLine($"Saved to DB: {evt.Type}");
        };

        channel.BasicConsume("events", true, consumer);

        return Task.CompletedTask;
    }
}