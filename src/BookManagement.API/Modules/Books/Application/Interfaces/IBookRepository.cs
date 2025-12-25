using BookManagement.API.Modules.Books.Application.Models;  
using BookManagement.API.Shared.Models;
using BookManagement.API.Modules.Books.Domain.Entities;
namespace BookManagement.API.Modules.Books.Application.Interfaces
{
    public interface IBookRepository
    {
        Task<PagedResult<Book>> GetPagedAsync(
        BookQuery query,
        CancellationToken ct = default);

        Task<Book?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task AddAsync(Book book, CancellationToken ct = default);

        Task UpdateAsync(Book book, CancellationToken ct = default);

        Task DeleteAsync(Book book, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
