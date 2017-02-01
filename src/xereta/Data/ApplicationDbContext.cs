using Microsoft.EntityFrameworkCore;
using xereta.Models;

namespace xereta.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<PublicWorker> publicWorkers {get; set;}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./xereta.db");
        }
    }
}