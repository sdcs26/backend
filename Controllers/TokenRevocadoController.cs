using Microsoft.AspNetCore.Mvc;
using Sowing_O2.Dtos;
using Sowing_O2.Services;

namespace Sowing_O2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenRevocadoController : ControllerBase
    {
        private readonly ITokenRevocadoService _tokenRevocadoService;

        // Inyecta el servicio a través del constructor
        public TokenRevocadoController(ITokenRevocadoService tokenRevocadoService)
        {
            _tokenRevocadoService = tokenRevocadoService;
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout([FromBody] TokenRevocadoDto tokenRevocadoDto)
        {
            if (_tokenRevocadoService.IsTokenRevoked(tokenRevocadoDto.Token))
            {
                return BadRequest("El token ya ha sido revocado.");
            }

            _tokenRevocadoService.AddRevokedToken(tokenRevocadoDto);
            return Ok("Token revocado exitosamente.");
        }
    }
}
