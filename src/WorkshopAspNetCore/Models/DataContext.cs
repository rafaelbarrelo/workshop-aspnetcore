using Microsoft.EntityFrameworkCore;

namespace WorkshopAspNetCore.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
    }
}