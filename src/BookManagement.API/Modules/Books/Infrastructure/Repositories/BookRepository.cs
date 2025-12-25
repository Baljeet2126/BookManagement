using BookManagement.API.Modules.Books.Application.Interfaces;
using BookManagement.API.Modules.Books.Application.Models;
using BookManagement.API.Modules.Books.Domain.Entities;
using BookManagement.API.Modules.Books.Infrastructure.DataContext;
using BookManagement.API.Modules.Books.Infrastructure.Extensions;
using BookManagement.API.Shared.Extensions;
using BookManagement.API.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.API.Modules.Books.Infrastructure.Repositories
{
    public sealed class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;
        private readonly ILogger<BookRepository> _logger;
        public BookRepository(
            BookDbContext context,
            ILogger<BookRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResult<Book>> GetPagedAsync(
            BookQuery query,
            CancellationToken ct = default)
        {
            var baseQuery = _context.Books
                .AsNoTracking()
                .ApplyFiltering(query);

            // Total count (before paging)
            var totalCount = await baseQuery.CountAsync(ct);

            var books = await baseQuery
                .ApplySorting(query)
                .ApplyPaging(query.PageNumber, query.PageSize)
                //.Select(b => new Book
                //{
                //    Id = b.Id,
                //    Title = b.Title,
                //    ShortDescription = b.ShortDescription,
                //    PublishDate = b.PublishDate,
                //    Authors = b.Authors,
                //    RowVersion = b.RowVersion
                //})
                .ToListAsync(ct);

            return new PagedResult<Book>(books, totalCount, query.PageNumber, query.PageSize);
        }
        public async Task<Book?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        public async Task AddAsync(Book book, CancellationToken ct = default)
        {
            _context.Books.Add(book);
        }

        public async Task UpdateAsync(Book book, CancellationToken ct = default)
        {
            _context.Books.Update(book);
        }

        public async Task DeleteAsync(Book book, CancellationToken ct = default)
        {
            _context.Books.Remove(book);
        }
        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}

