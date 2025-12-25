namespace BookManagement.API.Shared.Events
{
    public abstract record EventBase : IEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccuredAtUtc { get; init; } = DateTime.UtcNow; 
    }
}
