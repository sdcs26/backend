using Sowing_O2.Repositories.Models;

namespace Sowing_O2.Repositories
{
    public class MovimientoRepositories : IMovimientoRepositories
    {
        private readonly SowingO2PruebaContext _context;

        public MovimientoRepositories(SowingO2PruebaContext context)
        {
            _context = context;
        }

        public async Task AddMovimientoAsync(Movimiento movimiento)
        {
            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();
        }
    }
}
