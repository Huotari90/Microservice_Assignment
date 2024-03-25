//ElectricityPriceMonitorMS
//ElectricityPrice.cs
//Calls the API and deserialize the JSON data
using System;
using System.Text.Json.Serialization;

namespace ElectricityPriceMonitorMS.Models
{
    public class ElectricityPrice
    {
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
    }


}



