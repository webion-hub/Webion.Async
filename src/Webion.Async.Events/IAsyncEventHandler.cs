namespace Webion.Async.Events;

public interface IAsyncEventHandler<out T> where T : IAsyncEvent
{
    void Push(object value);
    Task RunAsync(CancellationToken cancellationToken);
}