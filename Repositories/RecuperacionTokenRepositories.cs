using Microsoft.EntityFrameworkCore;
using Sowing_O2.Repositories.Models;

namespace Sowing_O2.Repositories
{
    public class RecuperacionTokenRepository
    {
        private readonly SowingO2PruebaContext _context;

        public RecuperacionTokenRepository(SowingO2PruebaContext context)
        {
            _context = context;
        }

        public async Task AddRecuperacionTokenAsync(RecuperacionToken token)
        {
            await _context.RecuperacionToken.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RecuperacionToken> GetTokenByCorreoAsync(string correo)
        {
            return await _context.RecuperacionToken.FirstOrDefaultAsync(t => t.Correo == correo);
        }

        public async Task<RecuperacionToken> GetTokenAsync(string token)
        {
            return await _context.RecuperacionToken.FirstOrDefaultAsync(t => t.Token == token);
        }

    }
}
