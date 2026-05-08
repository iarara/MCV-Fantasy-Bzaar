using Microsoft.EntityFrameworkCore;

namespace MCV_Fantasy_Bzaar.Models
{
    public class AppDbContext : DbContext
    {
    
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
        public AppDbContext() { }

        public DbSet<BookDetails> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Here I set up the connectiion string for the local SQL Server database,
                // which will be used to store any important data we may want to keep track of
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FantasyBzaarDB;Trusted_Connection=True;");
            }
        }
    }
}