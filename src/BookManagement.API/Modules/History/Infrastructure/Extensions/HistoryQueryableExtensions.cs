using BookManagement.API.Modules.History.Application.Models;
using BookManagement.API.Modules.History.Domain.Entities;
using BookManagement.API.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.API.Modules.Books.Infrastructure.Extensions
{
    public static class HistoryQueryableExtensions
    {
        public static IQueryable<BookHistory> ApplyFiltering(this IQueryable<BookHistory> query, HistoryQuery historyQuery)
        {
            if (!string.IsNullOrWhiteSpace(historyQuery.Title))
                query = query.Where(b => b.BookTitle.Contains(historyQuery.Title));

            if (!string.IsNullOrWhiteSpace(historyQuery.Action))
                query = query.Where(b => b.Authors.Contains(historyQuery.Action));

            return query;
        }

        //public static IQueryable<BookHistory> ApplySorting(this IQueryable<BookHistory> query, HistoryQuery historyQuery)
        //{
        //    return historyQuery.SortBy switch
        //    {
        //        "title" => historyQuery.SortDirection == "desc"
        //            ? query.OrderByDescending(b => b.BookTitle)
        //            : query.OrderBy(b => b.BookTitle),
        //        _ => query.OrderBy(b => b.Id)
        //    };
        //}

        public static async Task<PagedResult<BookHistory>> ApplyPagingAsync(
        this IQueryable<BookHistory> query,
        HistoryQuery historyQuery,
        CancellationToken ct = default)
        {
            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((historyQuery.PageNumber - 1) * historyQuery.PageSize)
                .Take(historyQuery.PageSize)
                .ToListAsync(ct);

            return new PagedResult<BookHistory>(items, totalCount, historyQuery.PageNumber, historyQuery.PageSize);
        }
    }
}

