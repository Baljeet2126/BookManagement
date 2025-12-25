using BookManagement.API.Modules.Books.Application.Interfaces;
using BookManagement.API.Modules.Books.Application.Services;
using BookManagement.API.Modules.Books.Infrastructure.DataContext;
using BookManagement.API.Modules.Books.Infrastructure.Repositories;
using BookManagement.API.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.API.Modules.Books
{
    public static class BookModule
    {
        public static IServiceCollection AddBooksModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<BookDbContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("BooksDb"),
                      b => b.MigrationsAssembly(typeof(BookDbContext).Assembly.FullName)
                    ));


            // Repos
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IFailedEventStore, FailedEventRepository>();

            // Application services
            services.AddScoped<BookService>();

            return services;
        }
    }
}
