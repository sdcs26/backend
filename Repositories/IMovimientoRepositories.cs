using Sowing_O2.Repositories.Models;

namespace Sowing_O2.Repositories
{
    public interface IMovimientoRepositories
    {
        Task AddMovimientoAsync(Movimiento movimiento);
    }
}
