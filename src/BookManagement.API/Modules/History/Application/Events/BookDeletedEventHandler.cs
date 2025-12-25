using BookManagement.API.Modules.Books.Domain.Events;
using BookManagement.API.Modules.History.Application.Interfaces;
using BookManagement.API.Modules.History.Domain.Entities;

namespace BookManagement.API.Modules.History.Application.Events
{
    public sealed class BookDeletedEventHandler
    {
        private readonly IBookHistoryRepository _repository;

        public BookDeletedEventHandler(IBookHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(BookDeletedEvent evt, CancellationToken ct)
        {
            var entry = new BookHistory
            {
                BookId = evt.BookId,
                Action = "Deleted",
                BookTitle = evt.Title,
                Authors = string.Empty,
                Description = $"Book '{evt.Title}' was deleted",
            };

            await _repository.AddAsync(entry, ct);
            await _repository.SaveChangesAsync();
        }
    }

}
