using Microsoft.AspNetCore.Mvc;
using Sowing_O2.Dtos;
using Sowing_O2.Services;

namespace Sowing_O2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearPedido([FromBody] PedidoDTO pedidoDto)
        {
            try
            {
                var nuevoPedido = await _pedidoService.CrearPedidoAsync(pedidoDto);
                return Ok(nuevoPedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ObtenerTodosLosPedidos()
        {
            var pedidos = await _pedidoService.ObtenerTodosLosPedidosAsync();
            return Ok(pedidos);
        }

        [HttpPut("cambiar-estado")]
        public async Task<IActionResult> CambiarEstado([FromBody] string numeroPedido, [FromQuery] int nuevoEstadoId)
        {
            try
            {
                await _pedidoService.CambiarEstadoPedidoAsync(numeroPedido, nuevoEstadoId);
                return Ok("Estado actualizado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
