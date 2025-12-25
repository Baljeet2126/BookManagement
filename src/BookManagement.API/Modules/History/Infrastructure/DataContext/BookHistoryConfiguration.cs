
using BookManagement.API.Modules.History.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.API.Modules.History.Infrastructure.DataContext
{
    public class BookHistoryConfiguration : IEntityTypeConfiguration<BookHistory>
    {
        public void Configure(EntityTypeBuilder<BookHistory> builder)
        {
            builder.ToTable("BookHistory");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.BookId)
                   .IsRequired();

            builder.Property(e => e.Action)
                   .IsRequired();

            builder.Property(e => e.BookTitle)
                   .IsRequired();

            builder.Property(e => e.Authors)
                   .IsRequired();

            builder.Property(e => e.Description)
                   .IsRequired();

            builder.Property(e => e.OccurredOn)
                   .IsRequired();
        }
    }

}
