using Sowing_O2.Repositories.Models;

namespace Sowing_O2.Repositories
{
    public interface ISemillaRepositories
    {
        Task<IEnumerable<Semilla>> GetAllSemillasAsync();
        Task<Semilla> GetSemillaByIdAsync(int id);
        Task AddSemillaAsync(Semilla semilla);
        Task UpdateSemillaAsync(Semilla semilla);
        Task DeleteSemillaAsync(int id);
    }
}
