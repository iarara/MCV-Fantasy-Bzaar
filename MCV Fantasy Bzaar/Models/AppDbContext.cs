using Microsoft.EntityFrameworkCore;

namespace MCV_Fantasy_Bzaar.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BookDetails> Books { get; set; }
    }
}