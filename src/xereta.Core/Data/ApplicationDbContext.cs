using Microsoft.EntityFrameworkCore;
using xereta.Models;

namespace xereta.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<PublicWorker> publicWorkers {get; set;}
        public DbSet<Salary> salaries {get; set;}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./xereta.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Salary>().HasKey(e => new {e.Id, e.Year, e.Month});
            base.OnModelCreating(modelBuilder);
        }
    }
}