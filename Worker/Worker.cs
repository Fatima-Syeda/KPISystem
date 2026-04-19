using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class Worker : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "events",
            durable: false,
            exclusive: false,
            autoDelete: false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[Worker] Received: {message}");
        };

        channel.BasicConsume(
            queue: "events",
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }
}