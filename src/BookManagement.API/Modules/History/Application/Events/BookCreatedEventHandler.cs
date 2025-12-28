using BookManagement.API.Modules.Books.Domain.Events;
using BookManagement.API.Modules.History.Application.Interfaces;
using BookManagement.API.Modules.History.Domain.Entities;
using BookManagement.API.Shared.Events;

namespace BookManagement.API.Modules.History.Application.Events
{
    public sealed class BookCreatedEventHandler : IEventHandler<BookCreatedEvent>
    {
        private readonly IBookHistoryRepository _repository;

        public BookCreatedEventHandler(IBookHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(BookCreatedEvent evt, CancellationToken ct)
        {
            var entry = new BookHistory
            {
                BookId = evt.BookSnapshot.BookId,
                Action = "Created",
                BookTitle = evt.BookSnapshot.Title,
                Authors = string.Join(", ", evt.BookSnapshot.Authors),
                Description = $"Book '{evt.BookSnapshot.Title}' was created",
                OccurredOn = evt.BookSnapshot.OccuredAtUtc
            };

            await _repository.AddAsync(entry, ct);
            await _repository.SaveChangesAsync();
        }
    }

}
