using Microsoft.Extensions.Logging;

namespace Webion.Async.Events.Tests.Mocks;

public sealed class MockEventHandler : AsyncEventHandler<MockEvent>
{
    public int Sum { get; private set; }

    public MockEventHandler(ILogger<MockEventHandler> logger) : base(logger)
    {
    }
    
    
    protected override Task HandleAsync(MockEvent e, CancellationToken cancellationToken)
    {
        Sum += e.Value;
        return Task.CompletedTask;
    }
}