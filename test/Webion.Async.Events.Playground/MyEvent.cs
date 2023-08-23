namespace Webion.Async.Events.Playground;

public sealed class MyEvent : IAsyncEvent
{
    public required Guid Id { get; init; }
}