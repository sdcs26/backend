using Microsoft.AspNetCore.Mvc;
using Sowing_O2.Dtos;
using Sowing_O2.Services;

namespace Sowing_O2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemillaController : ControllerBase
    {
        private readonly ISemillaService _semillaService;

        public SemillaController(ISemillaService semillaService)
        {
            _semillaService = semillaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SemillaDto>>> GetAll()
        {
            return Ok(await _semillaService.GetAllSemillasAsync());
        }

        [HttpGet("Listar{id}")]
        public async Task<ActionResult<SemillaDto>> Get(int id)
        {
            var semilla = await _semillaService.GetSemillaByIdAsync(id);
            if (semilla == null)
                return NotFound();
            return Ok(semilla);
        }

        [HttpPost]
        [Route("CrearSemilla")]
        public async Task<ActionResult> Create(CreateSemillaDto createSemillaDto)
        {
            var semillaId = await _semillaService.AddSemillaAsync(createSemillaDto);
            return CreatedAtAction(nameof(Get), new { id = semillaId }, createSemillaDto);
        }

        [HttpPatch("cantidad{id}")]
        public async Task<ActionResult> UpdateCantidad(int id, [FromBody] int nuevaCantidad)
        {
            await _semillaService.UpdateCantidadAsync(id, nuevaCantidad);
            return NoContent();
        }

        [HttpPost("traslado{id}")]
        public async Task<ActionResult> TrasladarSemilla(int id, [FromBody] TrasladoDto trasladoDto)
        {
            await _semillaService.TrasladarSemillaAsync(id, trasladoDto);
            return NoContent();
        }


        [HttpDelete("Eliminar{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _semillaService.DeleteSemillaAsync(id);
            return NoContent();
        }
    }
}
