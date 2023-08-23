namespace Webion.Async.Events;

public interface IAsyncEventDispatcher
{
    void Register(object key, IAsyncEventHandler<IAsyncEvent> handler);
    void Register<TEvent>(IAsyncEventHandler<IAsyncEvent> handler);
    
    
    void Push<TEvent>(TEvent value) where TEvent : notnull;
    
    /// <summary>
    /// Adds an event to its respective queue.
    /// </summary>
    /// <param name="key">
    /// The key of the queue to which the event will be added.
    /// </param>
    /// <param name="value">
    /// The event to enqueue.
    /// </param>
    void Push(object key, object value);
    
    /// <summary>
    /// Dispatches the events to their respective queue handlers
    /// until stopped.
    /// </summary>
    Task RunAsync(CancellationToken cancellationToken);
}