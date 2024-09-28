using Sowing_O2.Repositories.Models;
using Sowing_O2.Services;
using Sowing_O2.Dtos;
using Sowing_O2.Controllers;

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
        public Usuario GetUsuarioPorCorreo(string correo)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Correo == correo);
        }

        public Usuario GetUsuarioPorNumeroDocumento(int numDocumento)
        {
            return _context.Usuarios.FirstOrDefault(u => u.NumDocumento == numDocumento);
        }

    }
}
