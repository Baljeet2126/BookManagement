namespace BookManagement.API.Modules.Books.Api.Dtos
{
    public record BookResponseModel
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? ShortDescription { get; init; }
        public DateTime PublishDate { get; init; }
        public List<string> Authors { get; init; } = [];
    }
}
