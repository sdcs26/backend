using Sowing_O2.Dtos;

namespace Sowing_O2.Services
{
    public interface ISemillaService
    {
        Task<IEnumerable<SemillaDto>> GetAllSemillasAsync();
        Task<SemillaDto> GetSemillaByIdAsync(int id);
        Task<int> AddSemillaAsync(CreateSemillaDto createSemillaDto);
        Task UpdateSemillaAsync(SemillaDto semillaDto);
        Task DeleteSemillaAsync(int id);
    }
}
