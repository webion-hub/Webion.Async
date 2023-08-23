Registers an async event dispatcher in the DI container.

## Usage

```csharp
// Register the service
builder.Services.AddAsyncEventHandler<MyEvent, MyEventHandler>();
builder.Services.AddAsyncEventDispatcher();


// Resolve the dispatcher and push an event
var dispatcher = provider.GetRequiredService<IAsyncEventDispatcher>();

dispatcher.Push(new MyEvent());
```