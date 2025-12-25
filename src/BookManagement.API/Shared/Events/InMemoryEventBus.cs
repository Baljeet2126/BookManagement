
namespace BookManagement.API.Shared.Events
{
    public class InMemoryEventBus : IEventBus
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<InMemoryEventBus> _logger;
        public InMemoryEventBus(IServiceProvider serviceProvider, ILogger<InMemoryEventBus> logger) {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
       where TEvent : IEvent
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>().ToList();

            _logger.LogInformation("Publishing event {EventType} to {HandlerCount} handlers. Id={EventId}",
                typeof(TEvent).Name, handlers.Count, @event.Id);

            foreach (var handler in handlers)
            {
                try
                {
                    await handler.HandleAsync(@event, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error handling event {EventType} in {HandlerType}. Id={EventId}",
                        typeof(TEvent).Name, handler.GetType().Name, @event.Id);
                    // Here you can also write to FailedEvents table.
                }
            }
        }
    }
}

