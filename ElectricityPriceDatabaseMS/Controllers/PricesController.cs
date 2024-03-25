using Microsoft.AspNetCore.Mvc;
using ElectricityPriceDatabaseMS.Data;
using ElectricityPriceDatabaseMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ElectricityPriceDatabaseMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PricesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ElectricityPrice>>> GetElectricityPrices()
        {
            return await _context.ElectricityPrices.ToListAsync();
        }

        // POST: api/Prices
        [HttpPost]
        public async Task<ActionResult<List<ElectricityPrice>>> PostElectricityPrices([FromBody] List<ElectricityPrice> electricityPrices)
        {
            _context.ElectricityPrices.AddRange(electricityPrices);
            await _context.SaveChangesAsync();
            return Ok(electricityPrices);
        }


        // PUT: api/Prices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElectricityPrice(int id, ElectricityPrice electricityPrice)
        {
            if (id != electricityPrice.Id)
            {
                return BadRequest();
            }

            _context.Entry(electricityPrice).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Prices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElectricityPrice(int id)
        {
            var electricityPrice = await _context.ElectricityPrices.FindAsync(id);
            if (electricityPrice == null)
            {
                return NotFound();
            }

            _context.ElectricityPrices.Remove(electricityPrice);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Prices/range //extension
        [HttpGet("range")]
        public async Task<ActionResult<IEnumerable<ElectricityPrice>>> GetElectricityPricesFromRange(
            DateTime startDate,
            DateTime endDate,
            int pageSize = 10,
            int page = 1)
        {
            var query = _context.ElectricityPrices
                                .Where(ep => ep.StartDate >= startDate && ep.EndDate <= endDate)
                                .OrderBy(ep => ep.StartDate)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize);

            return await query.ToListAsync();
        }


        [HttpGet("price-difference")]
        public async Task<ActionResult<decimal>> GetPriceDifferenceFromRange(
            DateTime startDate,
            DateTime endDate,
            decimal fixedPricePerHour)
        {
            // Calculate the total hours in the range
            var totalHours = (endDate - startDate).TotalHours;

            // Calculate the total cost at the fixed price
            var totalFixedPrice = fixedPricePerHour * (decimal)totalHours;

            // Calculate the total variable price from the database
            var totalVariablePrice = await _context.ElectricityPrices
                                                   .Where(ep => ep.StartDate >= startDate && ep.EndDate <= endDate)
                                                   .SumAsync(ep => ep.Price);

            // Calculate the difference
            var priceDifference = totalVariablePrice - totalFixedPrice;

            return priceDifference;
        }



    }
}
