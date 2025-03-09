using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace InMindLab8.Microservices.Students.Services;

public class CourseCreatedConsumerHostedService : IHostedService, IAsyncDisposable
{
    private readonly PersistentRabbitMqConsumer _consumer;

    public CourseCreatedConsumerHostedService(PersistentRabbitMqConsumer consumer)
    {
        _consumer = consumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.StartConsuming();
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _consumer.DisposeAsync();
    }

    public async ValueTask DisposeAsync() => await _consumer.DisposeAsync();
}