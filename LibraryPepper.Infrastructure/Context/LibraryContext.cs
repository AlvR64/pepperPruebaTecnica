using LibraryPepper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryPepper.Infrastructure.Context
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase("LibraryDB");
    }
}
