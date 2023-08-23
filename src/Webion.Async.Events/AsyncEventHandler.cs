using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Threading;

namespace Webion.Async.Events;

public abstract class AsyncEventHandler<T> : IAsyncEventHandler<T>
{
    protected readonly ILogger<AsyncEventHandler<T>> Logger;
    private readonly AsyncQueue<T> _events = new();

    protected AsyncEventHandler(ILogger<AsyncEventHandler<T>> logger)
    {
        Logger = logger;
    }
    
    
    public void Push(object value)
    {
        if (value is not T t)
            return;
        
        _events.Enqueue(t);
    }
    
    
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var value = await _events.DequeueAsync(cancellationToken);
                await HandleAsync(value, cancellationToken);
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