using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using InMindLab8.Microservices.Students.Data;
using InMindLab8.Microservices.Students.Entities;
using Microsoft.Extensions.DependencyInjection;

public class PersistentRabbitMqConsumer : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _exchangeName;
    private readonly string _queueName;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // Private constructor: use the async factory to initialize.
    private PersistentRabbitMqConsumer(
        IConnection connection, 
        IChannel channel, 
        string exchangeName, 
        string queueName, 
        IServiceScopeFactory serviceScopeFactory)
    {
        _connection = connection;
        _channel = channel;
        _exchangeName = exchangeName;
        _queueName = queueName;
        _serviceScopeFactory = serviceScopeFactory;
    }

    // Async factory method to create and initialize the consumer.
    public static async Task<PersistentRabbitMqConsumer> CreateAsync(
        string hostname, 
        string exchangeName, 
        string queueName, 
        IServiceScopeFactory serviceScopeFactory)
    {
        var factory = new ConnectionFactory { HostName = hostname };

        // Create a persistent connection and channel.
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        // Declare the exchange.
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null);

        // Declare the queue.
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        // Bind the queue to the exchange.
        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: ""); 
        
        Console.WriteLine(" Created Channel and Binds");

        return new PersistentRabbitMqConsumer(connection, channel, exchangeName, queueName, serviceScopeFactory);
    }

    public async void StartConsuming()
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);

            // Deserialize the message into a CourseCreatedEvent.
            var courseCreatedEvent = JsonConvert.DeserializeObject<CourseCreatedEvent>(message);
            if (courseCreatedEvent != null)
            {
                await HandleCourseCreated(courseCreatedEvent);
            }
            // If manual acknowledgements are used (autoAck: false), acknowledge the message here.
            await Task.Yield();
        };

        // Start consuming messages.
        await _channel.BasicConsumeAsync(
            queue: _queueName,
            autoAck: true, 
            consumer: consumer);
    }

    private async Task HandleCourseCreated(CourseCreatedEvent courseCreatedEvent)
    {
        Console.WriteLine($"Creating following course {courseCreatedEvent.Name}");
        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
                await repository.AddCourseAsync(new Course
                {
                    CourseId = courseCreatedEvent.CourseId,
                    Name = courseCreatedEvent.Name,
                    MaxNb = courseCreatedEvent.MaxNb
                });

                Console.WriteLine($"[x] Received and Processed Course: {courseCreatedEvent.Name}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Exception in HandleCourseCreated: {ex}");
        }
    }


    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
        _channel.Dispose();
        _connection.Dispose();
    }
}
