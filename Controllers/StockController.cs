using backend.Models;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/v1/stock")] 
    [ApiController]
    

    public class StockController : ControllerBase
    {
        private readonly StockService stockService;

        public StockController(StockService stockService)
        {
            this.stockService = stockService;
        }

      
        [HttpPatch("{id}")]
        public async Task<IActionResult> PutStock(
            int id,
            [FromBody] Stock updatedStock)
        {
            var stock = await stockService.updateStock(id, updatedStock);

            if (stock == null)
                return NotFound();

            return Ok(stock);
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await stockService.getAllStocks();

            if (stocks == null)
                return NotFound();

            return Ok(stocks);
        }

        [HttpPost]
        public async Task<IActionResult> PostStock(
            [FromBody] Stock stock)
        {
            var stockCreated = await stockService.createStock(stock);

            if (stockCreated == null)
            {
                return BadRequest(new
                {
                    status = "Failed",
                    message = "Stock already exists"
                });
            }

            return Created("", stockCreated);
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await stockService.deleteStock(id);

            if (stock == null)
                return NotFound();

            return NoContent();
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById(int id)
        {
            var stock = await stockService.getStockByID(id);

            if (stock == null)
                return NotFound();

            return Ok(stock);
        }
    }
}