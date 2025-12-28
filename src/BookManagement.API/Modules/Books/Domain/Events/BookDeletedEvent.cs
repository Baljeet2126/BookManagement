using BookManagement.API.Shared.Events;

namespace BookManagement.API.Modules.Books.Domain.Events
{
    public sealed record BookDeletedEvent(Guid BookId, string Title)
        : EventBase
    {
       
    };
}
