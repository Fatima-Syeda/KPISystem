using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class EventConsumer
{
    private readonly IConfiguration _config;
    private IConnection _connection;
    private IModel _channel;

    public EventConsumer(IConfiguration config)
    {
        _config = config;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQ:Host"],
            UserName = _config["RabbitMQ:Username"],
            Password = _config["RabbitMQ:Password"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: _config["RabbitMQ:Queue"],
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public void StartConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"📩 Received: {message}");

            // TODO: deserialize + process event
            // Example:
            // var evt = JsonSerializer.Deserialize<Event>(message);

            _channel.BasicAck(args.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(
            queue: _config["RabbitMQ:Queue"],
            autoAck: false,
            consumer: consumer
        );
    }
}