using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InMindLab8.Data;
using InMindLab8.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;

public class CourseCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _hostname;
    private readonly string _exchangeName;
    private IConnection _connection;
    private IChannel _channel;
    private readonly ConnectionFactory _connectionFactory;


    public CourseCreatedConsumer( IServiceScopeFactory serviceScopeFactory)
    {
        _hostname = "localhost";
        _exchangeName = "CourseExchange";
        
        _serviceScopeFactory = serviceScopeFactory;
        _connectionFactory = new ConnectionFactory { HostName = _hostname };

       
    }

    private async Task InitializeRabbitMqListener()
    {
        while (true)
        {
            try
            {
                _connection = await _connectionFactory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                // Declare the exchange
                await _channel.ExchangeDeclareAsync(exchange: _exchangeName, type: ExchangeType.Fanout, durable: false, autoDelete: false, arguments: null);

                // Declare the queue
                await _channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                // Bind the queue to the exchange
                await _channel.QueueBindAsync(queue: "hello", exchange: _exchangeName, routingKey: "");

                return;
            }
            catch (RabbitMQ.Client.Exceptions.OperationInterruptedException ex)
            {
                Console.WriteLine($"[!] Exchange not found. Retrying in 5 seconds...)");
                await Task.Delay(5000);
            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
            {
                Console.WriteLine($"[!] Exchange unreachable. Retrying in 5 seconds...");
                await Task.Delay(5000);
            }
        }
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitializeRabbitMqListener();

        // Create a consumer
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            // Deserialize the message
            var courseCreatedEvent = JsonConvert.DeserializeObject<CourseCreatedEvent>(message);

            // Handle the event (e.g., insert into microservice DB)
            await HandleCourseCreated(courseCreatedEvent);
        };

        // Start consuming
        await _channel.BasicConsumeAsync(
            queue: "hello",
            autoAck: true,
            consumer: consumer
        );
    }

    private async Task HandleCourseCreated(CourseCreatedEvent courseCreatedEvent)
    {
        using (var scope = _serviceScopeFactory.CreateScope()) // Create a new scope
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

    public override void Dispose()
    {
        _channel.CloseAsync();
        _connection.CloseAsync();
        base.Dispose();
    }
}
