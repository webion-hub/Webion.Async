namespace Webion.Async.Events;

public interface IAsyncEventHandler<out T>
{
    void Push(object value);
    Task RunAsync(CancellationToken cancellationToken);
}