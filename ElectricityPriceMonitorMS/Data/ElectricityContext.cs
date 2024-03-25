//ElectricityPriceMonitorMS
//ElectricityContext.cs
using ElectricityPriceMonitorMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace ElectricityPriceMonitorMS.Data
{
    public class ElectricityContext : DbContext
    {
        public DbSet<ElectricityPrice> ElectricityPrices { get; set; }

        public ElectricityContext(DbContextOptions options) : base(options)
        {
        }

        
    }
}

