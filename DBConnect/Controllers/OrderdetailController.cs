using System.Threading.Tasks;
using DBConnect.Data;
using DBConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrderDetailController(AppDbContext context) => _context = context;

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDetail>> Get(int id)
        {
            var od = await _context.OrderDetails.Include(d => d.Product).FirstOrDefaultAsync(d => d.Id == id);
            if (od == null) return NotFound();
            return Ok(od);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDetail detail)
        {
            if (id != detail.Id) return BadRequest();
            _context.Entry(detail).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var od = await _context.OrderDetails.FindAsync(id);
            if (od == null) return NotFound();
            _context.OrderDetails.Remove(od);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
