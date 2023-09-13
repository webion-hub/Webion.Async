using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Webion.Async.Events.Hosting;

public static class ServiceCollectionExtension
{
    public static void AddAsyncEventDispatcher(this IServiceCollection services)
    {
        services.AddSingleton<IAsyncEventDispatcher, AsyncEventDispatcher>(s =>
        {
            var logger = s.GetRequiredService<ILogger<AsyncEventDispatcher>>();
            var handlers = s.GetServices<AsyncEventHandlerConfig>();
            var dispatcher = new AsyncEventDispatcher(logger);

            foreach (var h in handlers)
            {
                var handler = s.GetRequiredService(h.HandlerType);
                if (handler is not IAsyncEventHandler<IAsyncEvent> ah)
                    throw new InvalidOperationException($"Type {h.HandlerType} is not an {nameof(IAsyncEventHandler<IAsyncEvent>)}");
                
                dispatcher.Register(h.Key, ah);
            }
            
            return dispatcher;
        });
        
        services.AddHostedService<BackgroundDispatcherService>();
    }
    
    public static void AddAsyncEventHandler<TEvent, THandler>(this IServiceCollection services)
        where THandler : AsyncEventHandler<TEvent>
        where TEvent : IAsyncEvent
    {
        services.AddSingleton(new AsyncEventHandlerConfig
        {
            Key = typeof(TEvent),
            HandlerType = typeof(THandler)
        });
        
        services.AddSingleton<THandler>();
    }
}