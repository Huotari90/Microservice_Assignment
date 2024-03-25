//ElectricityPriceDatabaseMS
//ElectricityPrice.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace ElectricityPriceDatabaseMS.Models
{
    public class ElectricityPrice
    {
        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        
    }
}
