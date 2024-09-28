using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;
using Sowing_O2.Repositories;
using static Sowing_O2.Utilities.Encriptacion;
using Microsoft.EntityFrameworkCore;

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

        public Usuario Login(LoginDto loginDto)
        {
            var usuario = _usuarioRepository.GetUsuarioPorCorreo(loginDto.Correo);

            if (usuario == null || !_securityService.VerifyPassword(loginDto.Contrasena, usuario.Contrasena))
            {
                throw new Exception("Credenciales inválidas.");
            }
            return usuario; // Retorna el usuario autenticado
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
