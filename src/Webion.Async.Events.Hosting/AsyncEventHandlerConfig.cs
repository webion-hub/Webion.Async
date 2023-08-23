namespace Webion.Async.Events.Hosting;

internal sealed class AsyncEventHandlerConfig
{
    public required Type Key { get; init; }
    public required Type HandlerType { get; init; }
}