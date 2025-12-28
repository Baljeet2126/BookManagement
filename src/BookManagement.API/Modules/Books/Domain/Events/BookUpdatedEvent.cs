using BookManagement.API.Modules.Books.Domain.Entities;
using BookManagement.API.Shared.Events;

namespace BookManagement.API.Modules.Books.Domain.Events
{
    public sealed record BookUpdatedEvent
    (
        BookSnapshot Old,
        BookSnapshot New
) : EventBase
    { }
}

