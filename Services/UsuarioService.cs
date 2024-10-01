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

namespace Sowing_O2.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepositories _usuarioRepository;
        private readonly ISecurityService _securityService;
        public UsuarioService(UsuarioRepositories usuarioRepository, ISecurityService securityService)
        {
            _usuarioRepository = usuarioRepository;
            _securityService = securityService;
        }

        public void CrearUsuario(UsuarioDto usuarioDto)
        {
            try
            {
                var hashedPassword = _securityService.HashPassword(usuarioDto.Contrasena);
                var usuarioExistente = _usuarioRepository.GetUsuarioPorCorreo(usuarioDto.Correo);
                if (usuarioExistente != null)
                {
                    throw new Exception("Ya existe un usuario con este correo electrónico.");
                }

                var usuarioPorDocumento = _usuarioRepository.GetUsuarioPorNumeroDocumento(usuarioDto.NumDocumento);
                if (usuarioPorDocumento != null)
                {
                    throw new Exception("Ya existe un usuario con este número de documento.");
                }

                var usuario = new Usuario
                {
                    NumDocumento = usuarioDto.NumDocumento,
                    Nombre = usuarioDto.Nombre,
                    Apellido = usuarioDto.Apellido,
                    Correo = usuarioDto.Correo,
                    Contrasena = hashedPassword,
                    IdRol = usuarioDto.IdRol
                };

                _usuarioRepository.AddUsuario(usuario);
            }
            catch (DbUpdateException dbEx)
            {
                // Registra el error interno para diagnóstico
                throw new Exception($"Error al guardar el usuario: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}");
            }
        }

        public LoginResponseDto Login(LoginDto loginDto)
        {
            var usuario = _usuarioRepository.GetUsuarioPorCorreo(loginDto.Correo);

            if (usuario == null || !_securityService.VerifyPassword(loginDto.Contrasena, usuario.Contrasena))
            {
                throw new Exception("Credenciales inválidas.");
            }

            // Generar token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("Gabi@Ivan@Sergi@45Software601@24"); // Clave secreta de firma
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Retornar el objeto de respuesta que contiene el usuario y el token
            return new LoginResponseDto
            {
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Token = tokenString
            };
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
