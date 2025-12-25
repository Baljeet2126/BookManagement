using BookManagement.API.Modules.Books.Domain.Entities;
using BookManagement.API.Modules.History.Application.Models;
using BookManagement.API.Modules.History.Domain.Entities;
using BookManagement.API.Shared.Models;
namespace BookManagement.API.Modules.History.Application.Interfaces
{
    public interface IBookHistoryRepository
    {
        Task<PagedResult<BookHistory>> GetPagedAsync(
        HistoryQuery query,
        CancellationToken ct = default);

        Task<BookHistory?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task AddAsync(BookHistory bookHistory, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
