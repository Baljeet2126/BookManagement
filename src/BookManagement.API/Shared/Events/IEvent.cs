namespace BookManagement.API.Shared.Events
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTime OccuredAtUtc { get; }
    }
}
