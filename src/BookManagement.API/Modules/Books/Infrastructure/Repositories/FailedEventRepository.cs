using BookManagement.API.Modules.Books.Infrastructure.DataContext;
using BookManagement.API.Shared.Events;

namespace BookManagement.API.Modules.Books.Infrastructure.Repositories
{
    public class FailedEventRepository : IFailedEventStore
    {
        private readonly BookDbContext _context;
        private readonly ILogger<FailedEventRepository> _logger;
        public FailedEventRepository(
            BookDbContext context,
            ILogger<FailedEventRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }
        public async Task StoreAsync(
            string eventType,
            string payload,
            string error,
            CancellationToken ct = default)
        {
            var failedEvent = new FailedEvent
            {
                EventType = eventType,
                Payload = payload,
                Error = error
            };

            _context.FailedEvents.Add(failedEvent);
            await _context.SaveChangesAsync(ct);
        }
    }
}
