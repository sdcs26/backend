using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication2.models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemillasController : ControllerBase
    {
        // Datos de ejemplo en memoria para pruebas rápidas
        private static List<Semilla> semillas = new List<Semilla>
        {
            new Semilla { Id = 1, Nombre = "Maíz", Categoria = "Cereal", CantidadEnInventario = 100, Proveedor = "Proveedor A", FechaDeIngreso = DateTime.Now, Ubicacion = "Bodega 1" },
            new Semilla { Id = 2, Nombre = "Girasol", Categoria = "Oleaginosa", CantidadEnInventario = 50, Proveedor = "Proveedor B", FechaDeIngreso = DateTime.Now, Ubicacion = "Bodega 2" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Semilla>> GetSemillas()
        {
            return Ok(semillas);
        }

        [HttpGet("{id}")]
        public ActionResult<Semilla> GetSemilla(int id)
        {
            var semilla = semillas.FirstOrDefault(s => s.Id == id);
            if (semilla == null)
            {
                return NotFound();
            }
            return Ok(semilla);
        }

        [HttpPost]
        public ActionResult<Semilla> CreateSemilla(Semilla nuevaSemilla)
        {
            nuevaSemilla.Id = semillas.Max(s => s.Id) + 1;
            semillas.Add(nuevaSemilla);
            return CreatedAtAction(nameof(GetSemilla), new { id = nuevaSemilla.Id }, nuevaSemilla);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSemilla(int id, Semilla semillaActualizada)
        {
            var semilla = semillas.FirstOrDefault(s => s.Id == id);
            if (semilla == null)
            {
                return NotFound();
            }

            semilla.Nombre = semillaActualizada.Nombre;
            semilla.Categoria = semillaActualizada.Categoria;
            semilla.CantidadEnInventario = semillaActualizada.CantidadEnInventario;
            semilla.Proveedor = semillaActualizada.Proveedor;
            semilla.FechaDeIngreso = semillaActualizada.FechaDeIngreso;
            semilla.Ubicacion = semillaActualizada.Ubicacion;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSemilla(int id)
        {
            var semilla = semillas.FirstOrDefault(s => s.Id == id);
            if (semilla == null)
            {
                return NotFound();
            }

            semillas.Remove(semilla);
            return NoContent();
        }
    }
}