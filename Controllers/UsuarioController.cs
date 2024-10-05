using Microsoft.AspNetCore.Mvc;
using Sowing_O2.Services;
using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Sowing_O2.Utilities;

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
        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                await _usuarioService.CrearUsuarioAsync(usuarioDto);
                return Ok(new { mensaje = "Usuario creado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        [HttpPost("RecuperarContraseña")]
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

        [HttpPost("ConfirmarTokenContraseña")]
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
        [HttpGet]
        public IActionResult ObtenerUsuarios()
        {
            var usuarios = _usuarioService.ObtenerUsuarios();
            return Ok(usuarios);
        }
    }
}

