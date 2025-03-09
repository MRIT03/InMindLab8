using System;
using System.Text;
using System.Threading.Tasks;
using InMindLab8.Application.Services;
using InMindLab8.Application.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

public class PersistentRabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _exchangeName;

    // Private constructor: instances are created via the async factory method.
    private PersistentRabbitMqPublisher(IConnection connection, IChannel channel, string exchangeName)
    {
        _connection = connection;
        _channel = channel;
        _exchangeName = exchangeName;
    }

    // Async factory method to create and initialize the publisher.
    public static async Task<PersistentRabbitMqPublisher> CreateAsync(string hostname, string exchangeName)
    {
        var factory = new ConnectionFactory { HostName = hostname };

        // Create a persistent connection and channel asynchronously.
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        // Declare the exchange once at startup.
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Fanout,
            durable: true,        // mark durable so it survives broker restarts
            autoDelete: false,
            arguments: null);

        return new PersistentRabbitMqPublisher(connection, channel, exchangeName);
    }

    public Task PublishCourseCreated(CourseCreatedEvent courseCreatedEvent)
    {
        // Serialize the event to JSON.
        var messageBody = JsonConvert.SerializeObject(courseCreatedEvent);
        var body = Encoding.UTF8.GetBytes(messageBody);

        // Publish using the pre-established channel.
        _channel.BasicPublishAsync(
            exchange: _exchangeName,
            routingKey: "",
            body: body);

        Console.WriteLine(" [x] Sent {0}", messageBody);
        return Task.CompletedTask;
    }

    // Dispose async resources.
    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
        _channel.Dispose();
        _connection.Dispose();
    }
}
