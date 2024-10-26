using Microsoft.EntityFrameworkCore;
using Sowing_O2.Repositories.Models;

namespace Sowing_O2.Repositories
{
    public class SemillaRepository : ISemillaRepositories
    {
        private readonly SowingO2PruebaContext _context;

        public SemillaRepository(SowingO2PruebaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Semilla>> GetAllSemillasAsync()
        {
            return await _context.Semillas.ToListAsync();
        }

        public async Task<Semilla> GetSemillaByIdAsync(int id)
        {
            return await _context.Semillas.FindAsync(id);
        }

        public async Task AddSemillaAsync(Semilla semilla)
        {
            _context.Semillas.Add(semilla);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSemillaAsync(Semilla semilla)
        {
            _context.Semillas.Update(semilla);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSemillaAsync(int id)
        {
            var semilla = await _context.Semillas.FindAsync(id);
            if (semilla != null)
            {
                _context.Semillas.Remove(semilla);
                await _context.SaveChangesAsync();
            }
        }
    }
}
