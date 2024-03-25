using Microsoft.EntityFrameworkCore;
using ElectricityPriceDatabaseMS.Models;

namespace ElectricityPriceDatabaseMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ElectricityPrice> ElectricityPrices { get; set; }
        
    }
}

