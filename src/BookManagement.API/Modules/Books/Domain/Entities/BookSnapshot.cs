using BookManagement.API.Shared.Events;

namespace BookManagement.API.Modules.Books.Domain.Entities
{
    public sealed record BookSnapshot
    (
        Guid BookId,
        string Title,
        IReadOnlyList<string> Authors

    ) : EventBase;
}
