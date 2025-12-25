using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookManagement.API.Modules.Books.Application.Models
{
    public record BookQuery
    {
        [DefaultValue(1)]
        public int PageNumber { get; init; } = 1;
        [Range(1, 100)]


        [DefaultValue(10)]
        public int PageSize { get; init; } = 10;
        public string? Title { get; init; }
        public string? Authors { get; init; }

        [DefaultValue("Title")]
        public string? SortBy { get; init; } = "id";

        [DefaultValue("asc")]
        public string? SortDirection { get; init; } = "asc";

        public static BookQuery Create(IConfiguration config, int? page = null, int? pageSize = null)
        {
            return new BookQuery
            {
                PageNumber = page ?? 1,
                PageSize = pageSize ?? config.GetValue<int>("Paging:DefaultPageSize", 10),
                SortBy = config.GetValue<string>("Paging:DefaultSortBy", "id")
            };
        }
    }

}
