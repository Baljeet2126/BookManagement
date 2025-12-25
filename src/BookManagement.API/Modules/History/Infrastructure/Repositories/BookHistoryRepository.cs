using BookManagement.API.Modules.Books.Infrastructure.Extensions;
using BookManagement.API.Modules.History.Application.Interfaces;
using BookManagement.API.Modules.History.Application.Models;
using BookManagement.API.Modules.History.Domain.Entities;
using BookManagement.API.Modules.History.Infrastructure.DataContext;
using BookManagement.API.Shared.Extensions;
using BookManagement.API.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.API.Modules.History.Infrastructure.Repositories
{
    public sealed class BookHistoryRepository : IBookHistoryRepository
    {
        private readonly BookHistoryDbContext _context;
        private readonly ILogger<BookHistoryRepository> _logger;
        public BookHistoryRepository(
            BookHistoryDbContext context,
            ILogger<BookHistoryRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResult<BookHistory>> GetPagedAsync(
            HistoryQuery query,
            CancellationToken ct = default)
        {
            var baseQuery = _context.BookHistories
                            .AsNoTracking()
                            .ApplyFiltering(query)
                            .OrderByDescending(x => x.OccurredOn);
                            

            // Total count (before paging)
            var totalCount = await baseQuery.CountAsync(ct);

            var booksHistory = await baseQuery
                .ApplyPaging(query.PageNumber, query.PageSize)
                .ToListAsync(ct);

            return new PagedResult<BookHistory>(booksHistory, totalCount, query.PageNumber, query.PageSize);
        }
        public async Task<BookHistory?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.BookHistories
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        
        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
       
        public async Task  AddAsync(BookHistory bookHistory, CancellationToken ct)
        {
            _context.BookHistories.Add(bookHistory);
        }
    }
}

