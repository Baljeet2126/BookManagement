using BookManagement.API.Modules.History.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.API.Modules.History.Infrastructure.DataContext
{
    
    public class BookHistoryDbContext : DbContext
    {
        public BookHistoryDbContext(DbContextOptions<BookHistoryDbContext> options) : base(options) { }

        public DbSet<BookHistory> BookHistories => Set<BookHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BookHistoryConfiguration());
        }
    }

}
