using Microsoft.AspNetCore.Mvc;
using WebApplication2.models;
namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        // Lista de ejemplo para pruebas en memoria
        private static List<Categoria> categorias = new List<Categoria>
        {
            new Categoria { Id = 1, Nombre = "Cereal", Descripcion = "Semillas de cereales como trigo, maíz, arroz." },
            new Categoria { Id = 2, Nombre = "Oleaginosa", Descripcion = "Semillas de plantas que producen aceite, como girasol o soja." }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetCategorias()
        {
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public ActionResult<Categoria> GetCategoria(int id)
        {
            var categoria = categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> CreateCategoria(Categoria nuevaCategoria)
        {
            nuevaCategoria.Id = categorias.Max(c => c.Id) + 1;
            categorias.Add(nuevaCategoria);
            return CreatedAtAction(nameof(GetCategoria), new { id = nuevaCategoria.Id }, nuevaCategoria);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategoria(int id, Categoria categoriaActualizada)
        {
            var categoria = categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Nombre = categoriaActualizada.Nombre;
            categoria.Descripcion = categoriaActualizada.Descripcion;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategoria(int id)
        {
            var categoria = categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            categorias.Remove(categoria);
            return NoContent();
        }
    }
}
