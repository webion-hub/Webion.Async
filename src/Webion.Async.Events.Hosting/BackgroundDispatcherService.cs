using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Webion.Async.Events.Hosting;

public sealed class BackgroundDispatcherService : BackgroundService
{
    private readonly ILogger<BackgroundDispatcherService> _logger;
    private readonly IAsyncEventDispatcher _dispatcher;

    public BackgroundDispatcherService(ILogger<BackgroundDispatcherService> logger, IAsyncEventDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting background dispatcher service");
        await _dispatcher.RunAsync(stoppingToken);
        _logger.LogInformation("Background dispatcher service has stopped");
    }
}