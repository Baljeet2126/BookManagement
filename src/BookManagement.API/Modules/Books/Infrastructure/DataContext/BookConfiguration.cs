using BookManagement.API.Modules.Books.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.API.Modules.Books.Infrastructure.DataContext
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.ShortDescription)
                   .HasMaxLength(1000);

            builder.Property(b => b.Authors)
                   .IsRequired()
                   .HasMaxLength(400);

            builder.Property(b => b.RowVersion)
                   .IsRowVersion(); // Optimistic concurrency
        }
    }

}
