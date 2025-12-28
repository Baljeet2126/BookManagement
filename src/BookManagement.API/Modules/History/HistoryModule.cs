using BookManagement.API.Modules.Books.Domain.Events;
using BookManagement.API.Modules.History.Application.Events;
using BookManagement.API.Modules.History.Application.Interfaces;
using BookManagement.API.Modules.History.Application.Services;
using BookManagement.API.Modules.History.Infrastructure.DataContext;
using BookManagement.API.Modules.History.Infrastructure.Repositories;
using BookManagement.API.Shared.Events;
using Microsoft.EntityFrameworkCore;


namespace BookManagement.API.Modules.History
{
    public static class HistoryModule
    {
        public static IServiceCollection AddHistoryModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<BookHistoryDbContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("HistoryDb"),
                      b => b.MigrationsAssembly(typeof(BookHistoryDbContext).Assembly.FullName)
                    ));


            // Repos
            services.AddScoped<IBookHistoryRepository, BookHistoryRepository>();

            // Application services
            services.AddScoped<HistoryService>();

            services.AddScoped<IEventHandler<BookCreatedEvent>, BookCreatedEventHandler>();
            services.AddScoped<IEventHandler<BookUpdatedEvent>, BookUpdatedEventHandler>();
            services.AddScoped<IEventHandler<BookDeletedEvent>, BookDeletedEventHandler>();


            return services;
        }
    }
}

