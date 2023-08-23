namespace Webion.Async.Events.Tests.Mocks;

public sealed class MockEvent : IAsyncEvent
{
    public required int Value { get; set; }
}