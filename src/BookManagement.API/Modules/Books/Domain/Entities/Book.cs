using System.ComponentModel.DataAnnotations;

namespace BookManagement.API.Modules.Books.Domain.Entities
{
    public class Book
    {
        private Book() { }
        public Book(string title, string? shortDescription, DateTime publishDate, List<string> authors)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            ShortDescription = shortDescription;
            PublishDate = publishDate;
            Authors = authors ?? throw new ArgumentNullException(nameof(authors));
        }
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string? ShortDescription { get; set; }

        public DateTime PublishDate { get; set; }

        public List<string> Authors { get; set; } = [];

        public byte[] RowVersion { get; init; } = Guid.NewGuid().ToByteArray();
    }
}
