using AdvanceDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvanceDataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasKey(u => u.Id);
        }
    }

}

