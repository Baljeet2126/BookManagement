using BookManagement.API.Modules.History.Application.Interfaces;
using BookManagement.API.Modules.History.Application.Services;
using BookManagement.API.Modules.History.Infrastructure.DataContext;
using BookManagement.API.Modules.History.Infrastructure.Repositories;
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

            return services;
        }
    }
}

