
using BookManagement.API.Modules.Books.Domain.Entities;
using BookManagement.API.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.API.Modules.Books.Infrastructure.DataContext
{
    
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<FailedEvent> FailedEvents => Set<FailedEvent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.ShortDescription).HasMaxLength(1000);
                entity.Property(b => b.Authors).IsRequired().HasMaxLength(500);
                entity.Property(b => b.PublishDate).IsRequired();
                entity.Property(b => b.RowVersion).IsRowVersion();

                entity.Property(b => b.RowVersion)
               .IsConcurrencyToken()
               .IsRequired()
               .ValueGeneratedNever();
            });

            modelBuilder.Entity<FailedEvent>(entity =>
            {
                entity.ToTable("FailedEvents");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SourceModule)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(e => e.EventType)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(e => e.Payload)
                      .IsRequired();

            });
        }
    }

}
