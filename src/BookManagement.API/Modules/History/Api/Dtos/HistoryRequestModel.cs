using System.ComponentModel.DataAnnotations;

namespace BookManagement.API.Modules.History.Api.Dtos
{
    public record HistoryRequestModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; init; } = string.Empty;

        [StringLength(1000, ErrorMessage = "ShortDescription cannot exceed 1000 characters")]
        public string? ShortDescription { get; init; }

        [Required(ErrorMessage = "Publish Date is required")]
        public DateTime PublishDate { get; init; }

        [Required(ErrorMessage = "Authors is required")]
        [MinLength(1, ErrorMessage = "At least one author is required")]
        public List<string> Authors { get; init; } = [];
    }

}
