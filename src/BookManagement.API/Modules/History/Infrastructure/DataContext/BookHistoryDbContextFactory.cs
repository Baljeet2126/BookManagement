using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookManagement.API.Modules.History.Infrastructure.DataContext
{
    public class BookHistoryDbContextFactory
     : IDesignTimeDbContextFactory<BookHistoryDbContext>
    {
        public BookHistoryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookHistoryDbContext>();
            optionsBuilder.UseSqlite("Data Source=Database/history.db");
            return new BookHistoryDbContext(optionsBuilder.Options);
        }
    }
}
