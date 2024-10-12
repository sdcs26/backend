namespace WebApplication2.Controllers;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.models;

    [ApiController]
[Route("api/[controller]")]

public class InventarioController : ControllerBase
{
    // Datos de ejemplo en memoria para pruebas rápidas
    private static List<MovimientoInventario> inventario = new List<MovimientoInventario>
        {
            new MovimientoInventario { Id = 1, SemillaId = 1, FechaMovimiento = DateTime.Now, TipoMovimiento = "Ingreso", Cantidad = 100, Observaciones = "Ingreso inicial" },
            new MovimientoInventario { Id = 2, SemillaId = 2, FechaMovimiento = DateTime.Now, TipoMovimiento = "Egreso", Cantidad = 20, Observaciones = "Venta" }
        };

    [HttpGet]
    public ActionResult<IEnumerable<MovimientoInventario>> GetMovimientosInventario()
    {
        return Ok(inventario);
    }

    [HttpGet("{id}")]
    public ActionResult<MovimientoInventario> GetMovimientoInventario(int id)
    {
        var movimiento = inventario.FirstOrDefault(m => m.Id == id);
        if (movimiento == null)
        {
            return NotFound();
        }
        return Ok(movimiento);
    }

    [HttpPost]
    public ActionResult<MovimientoInventario> CreateMovimientoInventario(MovimientoInventario nuevoMovimiento)
    {
        nuevoMovimiento.Id = inventario.Max(m => m.Id) + 1;
        inventario.Add(nuevoMovimiento);
        return CreatedAtAction(nameof(GetMovimientoInventario), new { id = nuevoMovimiento.Id }, nuevoMovimiento);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMovimientoInventario(int id, MovimientoInventario movimientoActualizado)
    {
        var movimiento = inventario.FirstOrDefault(m => m.Id == id);
        if (movimiento == null)
        {
            return NotFound();
        }

        movimiento.SemillaId = movimientoActualizado.SemillaId;
        movimiento.FechaMovimiento = movimientoActualizado.FechaMovimiento;
        movimiento.TipoMovimiento = movimientoActualizado.TipoMovimiento;
        movimiento.Cantidad = movimientoActualizado.Cantidad;
        movimiento.Observaciones = movimientoActualizado.Observaciones;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovimientoInventario(int id)
    {
        var movimiento = inventario.FirstOrDefault(m => m.Id == id);
        if (movimiento == null)
        {
            return NotFound();
        }

        inventario.Remove(movimiento);
        return NoContent();
    }
}

