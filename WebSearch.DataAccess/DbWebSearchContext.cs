using Microsoft.EntityFrameworkCore;
using WebSearch.Core.Models;

namespace WebSearch.DataAccess
{
    public class WebSearchDbContext : DbContext
    {
        public WebSearchDbContext(DbContextOptions<WebSearchDbContext> options) : base(options)
        {
        }

        public DbSet<Search> Searches { get; set; }
        public DbSet<SearchRun> SearchRuns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the primary key for Search
            modelBuilder.Entity<Search>()
                .HasKey(s => s.SearchId);

            // Configure the primary key for SearchRun
            modelBuilder.Entity<SearchRun>()
                .HasKey(sr => sr.SearchRunId);

            // Configure the one-to-many relationship
            modelBuilder.Entity<Search>()
                .HasMany(s => s.SearchRuns)
                .WithOne()
                .HasForeignKey(sr => sr.SearchId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
