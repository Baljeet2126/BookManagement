using BookManagement.API.Modules.Books.Api.Dtos;
using BookManagement.API.Modules.Books.Application.Models;
using BookManagement.API.Shared.Models;

namespace BookManagement.API.Modules.Books.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookResponseModel?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<PagedResult<BookResponseModel>> GetPagedAsync(BookQuery query, CancellationToken ct = default);

        Task<BookResponseModel> CreateAsync( BookRequestModel bookRequestModel,CancellationToken ct = default);

        Task<BookResponseModel?> UpdateAsync(Guid id, BookRequestModel request, CancellationToken ct = default);

        Task<bool> DeleteAsync( Guid id, CancellationToken ct = default);


    }
}
