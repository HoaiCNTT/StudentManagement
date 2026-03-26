using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace DBConnect.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        // Simple in-memory thread-safe store for demo / local development.
        private static readonly ConcurrentDictionary<int, Product> _store = new();
        private static int _nextId = 0;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        // GET /Product
        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            return _store.Values.OrderBy(p => p.Id);
        }

        // GET /Product/{id}
        [HttpGet("{id:int}")]
        public ActionResult<Product> Get(int id)
        {
            if (_store.TryGetValue(id, out var product))
                return product;
            return NotFound();
        }

        // POST /Product
        [HttpPost]
        public ActionResult<Product> Create([FromBody] ProductCreateDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Product name is required.");

            var id = System.Threading.Interlocked.Increment(ref _nextId);
            var product = new Product
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            _store[id] = product;
            _logger.LogInformation("Created product {Id}", id);

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        // PUT /Product/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] ProductUpdateDto dto)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Product name is required.");

            if (!_store.ContainsKey(id))
                return NotFound();

            var updated = new Product
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            _store[id] = updated;
            _logger.LogInformation("Updated product {Id}", id);

            return NoContent();
        }

        // DELETE /Product/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (_store.TryRemove(id, out _))
            {
                _logger.LogInformation("Deleted product {Id}", id);
                return NoContent();
            }

            return NotFound();
        }
    }

    // Simple DTOs and model used by the controller.
    public class Product
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal Price { get; init; }
    }

    public class ProductCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}