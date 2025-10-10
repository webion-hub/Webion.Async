using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Webion.Async.Events.Abstractions;

namespace Webion.Async.Events;

public abstract class AsyncEventHandler<T> : IAsyncEventHandler<T>
    where T : IAsyncEvent
{
    protected readonly ILogger<AsyncEventHandler<T>> Logger;
    
    private readonly Channel<T> _events;

    protected AsyncEventHandler(ILogger<AsyncEventHandler<T>> logger)
    {
        Logger = logger;
        _events = Channel.CreateUnbounded<T>();
    }
    
    
    public void Push(object value)
    {
        if (value is not T t)
        {
            Logger.LogWarning("Invalid event type {Type}", value.GetType());
            return;
        }

        if (!_events.Writer.TryWrite(t))
        {
            Logger.LogWarning("Error while enqueueing event");
        }
    }
    
    
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            var events = _events.Reader.ReadAllAsync(cancellationToken);
            await foreach (var e in events)
            {
                await HandleAsync(e, cancellationToken);
            }
        }
        catch (TaskCanceledException) {}
        catch (OperationCanceledException) {}
        catch (Exception e)
        {
            Logger.LogError(e, "An error has occurred while dequeuing");
            throw;
        }
    }
    
    
    protected abstract Task HandleAsync(T value, CancellationToken cancellationToken);
}