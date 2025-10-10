namespace Webion.Async.Events.Abstractions;

public interface IAsyncEventHandler<out T> where T : IAsyncEvent
{
    void Push(object value);
    Task RunAsync(CancellationToken cancellationToken);
}