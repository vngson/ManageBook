using ManageBook.Domain;
using Microsoft.EntityFrameworkCore;

namespace ManageBook.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        { }
        public DbSet<Book> books { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasKey(b => b.ISBN);
        }
    }
}
