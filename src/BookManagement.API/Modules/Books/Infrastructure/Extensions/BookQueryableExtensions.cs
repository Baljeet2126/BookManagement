using BookManagement.API.Modules.Books.Application.Models;
using BookManagement.API.Modules.Books.Domain.Entities;
using BookManagement.API.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.API.Modules.Books.Infrastructure.Extensions
{
    public static class BookQueryableExtensions
    {
        public static IQueryable<Book> ApplyFiltering(this IQueryable<Book> query, BookQuery bookQuery)
        {
            if (!string.IsNullOrWhiteSpace(bookQuery.Title))
                query = query.Where(b => b.Title.Contains(bookQuery.Title));

            if (!string.IsNullOrWhiteSpace(bookQuery.Authors))
                query = query.Where(b => b.Authors.Contains(bookQuery.Authors));

            return query;
        }

        public static IQueryable<Book> ApplySorting(this IQueryable<Book> query, BookQuery bookQuery)
        {
            return bookQuery.SortBy switch
            {
                "title" => bookQuery.SortDirection == "desc"
                    ? query.OrderByDescending(b => b.Title)
                    : query.OrderBy(b => b.Title),
                "publishDate" => bookQuery.SortDirection == "desc"
                    ? query.OrderByDescending(b => b.PublishDate)
                    : query.OrderBy(b => b.PublishDate),
                _ => query.OrderBy(b => b.Id)
            };
        }

        public static async Task<PagedResult<Book>> ApplyPagingAsync(
        this IQueryable<Book> query,
        BookQuery bookQuery,
        CancellationToken ct = default)
        {
            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((bookQuery.PageNumber - 1) * bookQuery.PageSize)
                .Take(bookQuery.PageSize)
                .ToListAsync(ct);

            return new PagedResult<Book>(items, totalCount, bookQuery.PageNumber, bookQuery.PageSize);
        }
    }
}

