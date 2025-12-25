namespace BookManagement.API.Shared.Events
{
    public sealed class FailedEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string SourceModule { get; init; } = string.Empty; 

        public string EventType { get; init; } = string.Empty;

        public string Payload { get; init; } = string.Empty;

        public string? Error { get; init; }

        public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    }

}
