using Microsoft.Extensions.Logging;

namespace Webion.Async.Events.Playground;

public sealed class MyEventHandler : AsyncEventHandler<MyEvent>
{
    public MyEventHandler(ILogger<MyEventHandler> logger) : base(logger)
    {
    }

    protected override async Task HandleAsync(MyEvent e, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        Logger.LogInformation("Handled event {Id} at {Instant}", e.Id, DateTime.UtcNow);
    }
}