Provides a simple dispatcher to handle events asynchronously.

The dispatcher dispatches the events synchronously, while having the handlers consume them asynchronously.

The events are handled in the order they are pushed.

## Usage

```csharp
// Define an event handler
public sealed class MyEventQueue : AsyncEventHandler<MyEvent>
{
    public MyEventQueue(ILogger<MyEventHandler> logger) : base(logger)
    {
    }
    
    protected override async Task HandleAsync(object value, CancellationToken cancellationToken)
    {
        // Do something with the event
    }
}

public sealed class MyEvent : IAsyncEvent
{
    // ...
}
```

```csharp
// Create the dispatcher and register the queue
var loggerFactory = NullLoggerFactory.Instance;

var dispatcher = new AsyncEventDispatcher(
    logger: loggerFactory.CreateLogger<AsyncEventDispatcher>(),
);

var handler = new MyEventHandler(/* ... */);
dispatcher.Register<MyEvent>(handler);

// Run the dispatcher
await dispatcher.RunAsync(cancellationToken);


// Dispatch an event
dispatcher.Push(new MyEvent());
```