using BookManagement.API.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.API.Shared.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(
            this IQueryable<T> query,
            int page,
            int pageSize)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
