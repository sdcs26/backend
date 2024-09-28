using Microsoft.AspNetCore.Mvc;
using Sowing_O2.Services;
using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;

namespace Sowing_O2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController: ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        [HttpPost]
        [Route("CrearUsuario")]
        public IActionResult CrearUsuario([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                if (usuarioDto == null)
                {
                    return BadRequest("El objeto UsuarioDto no puede ser nulo.");
                }

                _usuarioService.CrearUsuario(usuarioDto);
                return Ok("Usuario creado exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var usuario = _usuarioService.Login(loginDto);
                return Ok(new { mensaje = "Inicio de sesión exitoso", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ObtenerUsuarios()
        {
            var usuarios = _usuarioService.ObtenerUsuarios();
            return Ok(usuarios);
        }
    }
}

