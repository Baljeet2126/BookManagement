namespace BookManagement.API.Shared.Events
{
    public interface IFailedEventStore
    {
        Task StoreAsync(
            string eventType,
            string payload,
            string error,
            CancellationToken ct = default);
    }

}
