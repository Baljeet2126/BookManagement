using BookManagement.API.Modules.Books.Domain.Events;
using BookManagement.API.Modules.History.Application.Interfaces;
using BookManagement.API.Modules.History.Domain.Entities;

namespace BookManagement.API.Modules.History.Application.Events
{
    public sealed class BookUpdatedEventHandler
    {
        private readonly IBookHistoryRepository _repository;

        public BookUpdatedEventHandler(IBookHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(BookUpdatedEvent evt, CancellationToken ct)
        {
            var changes = new List<string>();

            if (evt.Old.Title != evt.New.Title)
            {
                changes.Add(
                    $"Title changed from '{evt.Old.Title}' to '{evt.New.Title}'");
            }

            if (!evt.Old.Authors.SequenceEqual(evt.New.Authors))
            {
                changes.Add(
                    $"Authors changed from [{string.Join(", ", evt.Old.Authors)}] " +
                    $"to [{string.Join(", ", evt.New.Authors)}]");
            }

            var entry = new BookHistory
            {
                BookId = evt.New.BookId,
                Action = "Updated",
                BookTitle = evt.New.Title,
                Authors = string.Join(", ", evt.New.Authors),
                Description = string.Join("; ", changes)
            };

            await _repository.AddAsync(entry, ct);
            await _repository.SaveChangesAsync();
        }
    }

}
