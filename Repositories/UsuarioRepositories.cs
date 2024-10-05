using Sowing_O2.Repositories.Models;
using Sowing_O2.Services;
using Sowing_O2.Dtos;
using Sowing_O2.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Sowing_O2.Repositories
{
    public class UsuarioRepositories
    {
        private readonly SowingO2PruebaContext _context;

        public UsuarioRepositories(SowingO2PruebaContext context)
        {
            _context = context;
        }

        public void AddUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public List<Usuario> GetAllUsuarios()
        {
            return _context.Usuarios.ToList();
        }
        public async Task<UsuarioDto> GetUsuarioPorCorreo(string correo)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo.Equals(correo));

            if (user == null) return null;

            return new UsuarioDto
            {
                NumDocumento = user.NumDocumento,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Correo = user.Correo,
                Contrasena = user.Contrasena,
                IdRol = user.IdRol
            };
        }
        public Usuario GetUsuarioPorNumeroDocumento(int numDocumento)
        {
            return _context.Usuarios.FirstOrDefault(u => u.NumDocumento == numDocumento);
        }
        public async Task UpdateUsuario(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarRegistrosDuplicadosPorCorreoAsync(string correo)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC EliminarUsuariosDuplicadosPorCorreo @Correo", new SqlParameter("@Correo", correo));
        }
        public async Task<Usuario> GetUsuarioPorCorreoModelo(string correo)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo.Equals(correo));
        }

    }
}
