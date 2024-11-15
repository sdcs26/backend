using Microsoft.AspNetCore.Mvc;
using Sowing_O2.Services;
using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Sowing_O2.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Sowing_O2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController: ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly TokenRevocadoService _authService;
        
        private readonly RecuperarContrasenaService _recuperarContrasenaService;

        public UsuarioController(UsuarioService usuarioService, TokenRevocadoService authService, RecuperarContrasenaService recuperarContrasenaService)
        {
            _usuarioService = usuarioService;
            _authService = authService;
            _recuperarContrasenaService = recuperarContrasenaService;
        }

        [Authorize]
        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                
                var usuarioLogueadoRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (usuarioLogueadoRol == null || usuarioLogueadoRol != "2")
                {
                    return Forbid("Acceso denegado: Solo los gerentes pueden crear usuarios.");
                }

                await _usuarioService.CrearUsuarioAsync(usuarioDto);
                return Ok(new { mensaje = "Usuario creado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("EliminarUsuario/{correo}")]
        public async Task<IActionResult> DeleteUsuario(string correo)
        {
            try
            {
                
                var usuarioLogueadoRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (usuarioLogueadoRol == null || usuarioLogueadoRol != "2") 
                {
                    return Forbid("Acceso denegado: Solo los gerentes pueden eliminar usuarios.");
                }

                
                var resultado = await _usuarioService.EliminarUsuarioAsync(correo);

                if (!resultado)
                {
                    return NotFound(new { mensaje = "El usuario no existe o no pudo ser eliminado." });
                }

                return Ok(new { mensaje = "Usuario eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = $"Error al eliminar el usuario: {ex.Message}" });
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var response = await _usuarioService.Login(loginDto);
                return Ok(new { mensaje = "Inicio de sesión exitoso", response });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CerrarSesion")]
        public IActionResult Logout([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return BadRequest("El campo Authorization es requerido.");
                }

                string token = authorizationHeader.Replace("Bearer ", "");
                _authService.Logout(token);
                return Ok(new { mensaje = "Sesión cerrada exitosamente." });
            }
            catch (Exception ex)

            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("RecuperarContrasena")]
        public async Task<IActionResult> EnviarToken([FromBody] RecuperarContrasenaDto dto)
        {
            try
            {
                await _recuperarContrasenaService.EnviarRecuperacionContrasenaAsync(dto.Correo);
                return Ok(new { mensaje = "Correo de recuperación enviado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ConfirmarTokenContrasena")]
        public async Task<IActionResult> Confirmar([FromBody] ConfirmarRecuperarContrasenaDto dto)
        {
            try
            {
                await _recuperarContrasenaService.ConfirmarRecuperacionContrasenaAsync(dto);
                return Ok(new { mensaje = "Contraseña actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "2")] 
        [HttpGet("listarUsuarios")]
        public IActionResult ObtenerUsuarios()
        {
            var usuarios = _usuarioService.ObtenerUsuarios();
            return Ok(usuarios);
        }
        [HttpPatch("InhabilitarUsuario/{correo}")]
        public async Task<IActionResult> InhabilitarUsuario(string correo)
        {
            var resultado = await _usuarioService.InhabilitarUsuarioAsync(correo);
            return resultado ? Ok(new { mensaje = "Usuario inhabilitado con éxito" }) : NotFound(new { mensaje = "Usuario no encontrado" });
        }

        [HttpPatch("ActivarUsuario/{correo}")]
        public async Task<IActionResult> ActivarUsuario(string correo)
        {
            var resultado = await _usuarioService.ActivarUsuarioAsync(correo);
            return resultado ? Ok(new { mensaje = "Usuario activado con éxito" }) : NotFound(new { mensaje = "Usuario no encontrado" });
        }
        [HttpGet("ServicioProtegido")]
        public IActionResult ServicioProtegido()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!_authService.IsTokenValid(token))
            {
                return Unauthorized("Token inválido o revocado.");
            }
            return Ok("Acceso permitido.");
        }

    }
}

