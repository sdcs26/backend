using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;

namespace Sowing_O2.Repositories
{
    public class TokenRevocadoRepositories
    {
        private readonly SowingO2PruebaContext _context;

        public TokenRevocadoRepositories(SowingO2PruebaContext context)
        {
            _context = context;
        }

        public void AddTokenRevocado(string token)
        {
            var revokedToken = new TokenRevocado
            {
                Token = token,
                FechaRevocado = DateTime.UtcNow
            };
            _context.TokenRevocados.Add(revokedToken);
            _context.SaveChanges();
        }

        public bool IsTokenRevoked(string token)
        {
            return _context.TokenRevocados.Any(rt => rt.Token == token);
        }
    }
}
