using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.models;
using WebApplication2.Models; // Asegúrate de que el namespace sea correcto

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoInventario>>> GetMovimientosInventario()
        {
            var inventarioMovimientos = await _context.Inventario.ToListAsync();
            return Ok(inventarioMovimientos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoInventario>> GetMovimientoInventario(int id)
        {
            var movimiento = await _context.Inventario.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            return Ok(movimiento);
        }

        [HttpPost]
        public async Task<ActionResult<MovimientoInventario>> CreateMovimientoInventario(MovimientoInventario nuevoMovimiento)
        {
            _context.Inventario.Add(nuevoMovimiento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovimientoInventario), new { id = nuevoMovimiento.Id }, nuevoMovimiento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovimientoInventario(int id, MovimientoInventario movimientoActualizado)
        {
            if (id != movimientoActualizado.Id)
            {
                return BadRequest();
            }

            _context.Entry(movimientoActualizado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimientoInventarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimientoInventario(int id)
        {
            var movimiento = await _context.Inventario.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }

            _context.Inventario.Remove(movimiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovimientoInventarioExists(int id)
        {
            return _context.Inventario.Any(e => e.Id == id);
        }
    }
}
