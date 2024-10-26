using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;
using Sowing_O2.Repositories;

namespace Sowing_O2.Services
{
    public class SemillaService : ISemillaService
    {
        private readonly ISemillaRepositories _semillaRepository;

        public SemillaService(ISemillaRepositories semillaRepository)
        {
            _semillaRepository = semillaRepository;
        }

        public async Task<IEnumerable<SemillaDto>> GetAllSemillasAsync()
        {
            var semillas = await _semillaRepository.GetAllSemillasAsync();
            
            return semillas.Select(s => new SemillaDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Codigo = s.Codigo,
                Descripcion = s.Descripcion,
                Cantidad = s.Cantidad,
                IdCategoria = s.IdCategoria,
                Ubicacion = s.Ubicacion
            });
        }

        public async Task<SemillaDto> GetSemillaByIdAsync(int id)
        {
            var semilla = await _semillaRepository.GetSemillaByIdAsync(id);
            return semilla != null ? new SemillaDto
            {
                Id = semilla.Id,
                Nombre = semilla.Nombre,
                Codigo = semilla.Codigo,
                Descripcion = semilla.Descripcion,
                Cantidad = semilla.Cantidad,
                IdCategoria = semilla.IdCategoria,
                Ubicacion = semilla.Ubicacion
            } : null;
        }

        public async Task<int> AddSemillaAsync(CreateSemillaDto createSemillaDto)
        {
            var semilla = new Semilla
            {
                Nombre = createSemillaDto.Nombre,
                Codigo = createSemillaDto.Codigo,
                Descripcion = createSemillaDto.Descripcion,
                Cantidad = createSemillaDto.Cantidad,
                IdCategoria = createSemillaDto.IdCategoria,
                Ubicacion = createSemillaDto.Ubicacion
            };

            await _semillaRepository.AddSemillaAsync(semilla);
            return semilla.Id;  // ID autogenerado por la base de datos
        }

        public async Task UpdateSemillaAsync(SemillaDto semillaDto)
        {
            var semilla = new Semilla
            {
                Id = semillaDto.Id,
                Nombre = semillaDto.Nombre,
                Codigo = semillaDto.Codigo,
                Descripcion = semillaDto.Descripcion,
                Cantidad = semillaDto.Cantidad,
                IdCategoria = semillaDto.IdCategoria,
                Ubicacion = semillaDto.Ubicacion
            };
            await _semillaRepository.UpdateSemillaAsync(semilla);
        }

        public async Task DeleteSemillaAsync(int id)
        {
            await _semillaRepository.DeleteSemillaAsync(id);
        }
    }
}
