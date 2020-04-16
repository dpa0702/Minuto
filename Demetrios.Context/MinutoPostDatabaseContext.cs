using Microsoft.EntityFrameworkCore;
using Demetrios.Models;

namespace Demetrios.Context
{
    public class MinutoPostDatabaseContext : DbContext
    {
        public MinutoPostDatabaseContext(
            DbContextOptions<MinutoPostDatabaseContext> dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<MinutoPost> MinutoPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MinutoPost>()
                .HasKey(x => x.id);
        }
    }
}
