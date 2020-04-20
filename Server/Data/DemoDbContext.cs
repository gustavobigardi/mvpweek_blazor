using Microsoft.EntityFrameworkCore;
using MVPWeek.Server.Models;

namespace MVPWeek.Server.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participant>()
                .HasIndex(e => e.Email)
                .IsUnique();
        }
    }
}