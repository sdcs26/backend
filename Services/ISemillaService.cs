using Sowing_O2.Dtos;

namespace Sowing_O2.Services
{
    public interface ISemillaService
    {
        Task<IEnumerable<SemillaDto>> GetAllSemillasAsync();
        Task<SemillaDto> GetSemillaByIdAsync(int id);
        Task<int> AddSemillaAsync(CreateSemillaDto createSemillaDto);
        Task UpdateCantidadAsync(int id, int nuevaCantidad);
        Task TrasladarSemillaAsync(int id, TrasladoDto trasladoDto);
        Task DeleteSemillaAsync(int id);
    }
}
