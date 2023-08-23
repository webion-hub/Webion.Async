using Microsoft.Extensions.Logging;
using NSubstitute;
using Webion.Async.Events.Tests.Mocks;

namespace Webion.Async.Events.Tests;

public sealed class DispatchTest
{
    [Fact]
    public async Task DispatchAsync_ShouldDispatchTheEventsCorrectly_Async()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        
        var dispatcher = new AsyncEventDispatcher(
            logger: Substitute.For<ILogger<AsyncEventDispatcher>>()
        );
        
        var handler = new MockEventHandler(
            logger: Substitute.For<ILogger<MockEventHandler>>()
        );
        
        dispatcher.Register<MockEvent>(handler);
        
        var events = new List<MockEvent>
        {
            new() { Value = 1 },
            new() { Value = 2 },
            new() { Value = 3 }
        };

        // Act
        await Task.WhenAny(
            dispatcher.RunAsync(cts.Token),
            EnqueueAndWaitAsync(cts.Token)
        );
        
        
        // Assert
        Assert.Equal(events.Sum(e => e.Value), handler.Sum);
        return;

        
        async Task EnqueueAndWaitAsync(CancellationToken cancellationToken)
        {
            foreach (var e in events)
                dispatcher.Push(e);

            await Task.Delay(1000, cancellationToken);
        }
    }
}