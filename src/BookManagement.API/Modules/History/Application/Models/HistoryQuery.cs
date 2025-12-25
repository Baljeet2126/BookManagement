using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookManagement.API.Modules.History.Application.Models
{
    public sealed record HistoryQuery
    {
        [DefaultValue(1)]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; init; } = 1;

        [DefaultValue(10)]
        [Range(1, 100)]
        public int PageSize { get; init; } = 10;

        // Filters (domain-specific)
        public string? Title { get; init; }

        public string? Action { get; init; } 
    }
}

