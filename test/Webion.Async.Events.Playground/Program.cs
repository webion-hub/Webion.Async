using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Webion.Async.Events;
using Webion.Async.Events.Hosting;
using Webion.Async.Events.Playground;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAsyncEventHandler<MyEvent, MyEventHandler>();
builder.Services.AddAsyncEventDispatcher();

var app = builder.Build();
var dispatcher = app.Services.GetRequiredService<IAsyncEventDispatcher>();

await Task.WhenAny(
    app.RunAsync(),
    EmitAsync()
);

return;


async Task EmitAsync()
{
    using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    while (await timer.WaitForNextTickAsync())
    {
        dispatcher.Push(new MyEvent
        {
            Id = Guid.NewGuid(),
        });
    }
}