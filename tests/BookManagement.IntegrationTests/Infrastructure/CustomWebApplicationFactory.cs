using BookManagement.API;
using BookManagement.API.Modules.Books.Infrastructure.DataContext;
using BookManagement.API.Modules.History.Infrastructure.DataContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>  
        {
            // Remove existing registrations
            services.RemoveAll<DbContextOptions<BookDbContext>>();
            services.RemoveAll<DbContextOptions<BookHistoryDbContext>>();

            // Add InMemory databases
            services.AddDbContext<BookDbContext>(options =>
                options.UseInMemoryDatabase("BooksTestDb"));  

            services.AddDbContext<BookHistoryDbContext>(options =>
                options.UseInMemoryDatabase("HistoryTestDb"));
        });
    }
}
