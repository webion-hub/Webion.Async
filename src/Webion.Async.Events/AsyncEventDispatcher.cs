using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Webion.Async.Events;

public sealed class AsyncEventDispatcher : IAsyncEventDispatcher
{
    private readonly ConcurrentDictionary<object, List<IAsyncEventHandler<IAsyncEvent>>> _handlers = new();
    private readonly ILogger<AsyncEventDispatcher> _logger;

    public AsyncEventDispatcher(ILogger<AsyncEventDispatcher> logger)
    {
        _logger = logger;
    }
    
    
    public void Register<TEvent>(IAsyncEventHandler<IAsyncEvent> handler)
    {
        Register(typeof(TEvent), handler);
    }

    public void Register(object key, IAsyncEventHandler<IAsyncEvent> handler)
    {
        var handlers = _handlers.GetOrAdd(
            key: key,
            valueFactory: _ => new List<IAsyncEventHandler<IAsyncEvent>>()
        );
        
        handlers.Add(handler);
    }

    public void Push<TEvent>(TEvent value)
        where TEvent : notnull
    {
        Push(typeof(TEvent), value);
    }
    
    public void Push(object key, object value)
    {
        var found = _handlers.TryGetValue(key, out var handlers);
        if (!found)
        {
            _logger.LogWarning("No handler found for event {Key}", key);
            return;
        }
        
        foreach (var h in handlers!)
            h.Push(value);
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var tasks = _handlers.Values
            .SelectMany(h => h)
            .Select(q => q.RunAsync(cancellationToken))
            .ToList();
        
        await Task.WhenAny(tasks);
    }
}