using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;
using Sowing_O2.Repositories;
using static Sowing_O2.Utilities.Encriptacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sowing_O2.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Sowing_O2.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepositories _usuarioRepository;
        private readonly ISecurityService _securityService;
        private readonly JwtSettingsDto _jwtSettings;
        private readonly IEmailSender _emailSender;

        public UsuarioService(UsuarioRepositories usuarioRepository, ISecurityService securityService, JwtSettingsDto jwtSettings, IEmailSender emailSender)
        {
            _usuarioRepository = usuarioRepository;
            _securityService = securityService;
            _jwtSettings = jwtSettings;
            _emailSender = emailSender;
        }

        public async Task CrearUsuarioAsync(UsuarioDto usuarioDto)
        {
            try
            {
                // Hash de la contraseña
                var hashedPassword = _securityService.HashPassword(usuarioDto.Contrasena);

                // Esperar la respuesta de la consulta del usuario por correo
                var usuarioExistente = await _usuarioRepository.GetUsuarioPorCorreo(usuarioDto.Correo);
                if (usuarioExistente != null)
                {
                    throw new Exception("Ya existe un usuario con este correo electrónico.");
                }

                // Consultar si ya existe un usuario con el mismo número de documento
                var usuarioPorDocumento = _usuarioRepository.GetUsuarioPorNumeroDocumento(usuarioDto.NumDocumento);
                if (usuarioPorDocumento != null)
                {
                    throw new Exception("Ya existe un usuario con este número de documento.");
                }

                // Crear el nuevo usuario
                var usuario = new Usuario
                {
                    NumDocumento = usuarioDto.NumDocumento,
                    Nombre = usuarioDto.Nombre,
                    Apellido = usuarioDto.Apellido,
                    Correo = usuarioDto.Correo,
                    Contrasena = hashedPassword,
                    IdRol = usuarioDto.IdRol
                };

                // Guardar en la base de datos
                await _emailSender.SendEmailAsync(usuarioDto.Correo, "Registro exitoso", @"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Registro Exitoso</title>
                    </head>
                    <body>
                        <div>
                            <img src='https://i.pinimg.com/originals/ad/68/4f/ad684f97e72be279d6e1f462411d7a03.jpg' alt='Correo Bienvenida'>
                        </div>
                    </body>
                    </html>");
                _usuarioRepository.AddUsuario(usuario);
            }
            catch (DbUpdateException dbEx)
            {
                // Registrar el error interno para diagnóstico
                throw new Exception($"Error al guardar el usuario: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}");
            }
        }


        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            var usuario = await _usuarioRepository.GetUsuarioPorCorreo(loginDto.Correo);

            if (usuario == null || !_securityService.VerifyPassword(loginDto.Contrasena, usuario.Contrasena))
            {
                throw new Exception("Credenciales inválidas.");
            }

            // Generar token JWT
            var loginResponseDto = JwtUtility.GenTokenkey(usuario, _jwtSettings);

            return loginResponseDto;
        }
        

        public List<UsuarioDto> ObtenerUsuarios()
        {
            var usuarios = _usuarioRepository.GetAllUsuarios();
            return usuarios.Select(u => new UsuarioDto
            {
                NumDocumento = u.NumDocumento,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Correo,
                IdRol = u.IdRol
            }).ToList();
        }
    }
}
