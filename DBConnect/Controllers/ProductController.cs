using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBConnect.Data;
using DBConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DBConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(AppDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created product {Id}", product.Id);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated product {Id}", id);
            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted product {Id}", id);
            return NoContent();
        }

        // DELETE: api/Product/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("Danh sách id không hợp lệ.");

            var products = await _context.Products
                                         .Where(p => ids.Contains(p.Id))
                                         .ToListAsync();

            if (!products.Any())
                return NotFound("Không tìm thấy sản phẩm nào.");

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted {Count} products", products.Count);
            return NoContent();
        }
    }
}