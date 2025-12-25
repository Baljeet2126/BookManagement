namespace BookManagement.API.Modules.History.Domain.Entities
{

    public sealed class BookHistory
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public Guid BookId { get; init; }

        public required string Action { get; init; }

        public required string BookTitle { get; init; }

        public required string Authors { get; init; }

        public required string Description { get; init; }

        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    }

}
