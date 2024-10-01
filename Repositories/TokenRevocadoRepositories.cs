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

        public void AddTokenRevocado(TokenRevocadoDto tokenRevocadoDto)
        {
            var tokenRevocado = new TokenRevocado
            {
                Token = tokenRevocadoDto.Token,
                fechaRevocado = DateTime.UtcNow
            };

            _context.TokenRevocado.Add(tokenRevocado);
            _context.SaveChanges();
        }
        public bool IsTokenRevoked(string token)
        {
            return _context.TokenRevocado.Any(rt => rt.Token == token);
        }
    }
}
