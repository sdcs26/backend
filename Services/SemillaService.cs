using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;
using Sowing_O2.Repositories;

namespace Sowing_O2.Services
{
    public class SemillaService : ISemillaService
    {
        private readonly ISemillaRepositories _semillaRepository;
        private readonly IMovimientoRepositories _movimientoRepository;

        public SemillaService(ISemillaRepositories semillaRepository, IMovimientoRepositories movimientoRepository)
        {
            _semillaRepository = semillaRepository;
            _movimientoRepository = movimientoRepository;
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
            // Verificar si la ubicación ya está ocupada
            var ubicacionExistente = await _semillaRepository.FindByUbicacionAsync(createSemillaDto.Ubicacion);
            if (ubicacionExistente != null)
            {
                throw new Exception("La ubicación ya está ocupada por otra semilla.");
            }

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
            return semilla.Id;
        }


        public async Task UpdateCantidadAsync(int id, int nuevaCantidad)
        {
            var semilla = await _semillaRepository.GetSemillaByIdAsync(id);
            if (semilla == null) throw new Exception("Semilla no encontrada");

            semilla.Cantidad = nuevaCantidad;
            await _semillaRepository.UpdateSemillaAsync(semilla);
        }

        public async Task TrasladarSemillaAsync(int id, TrasladoDto trasladoDto)
        {
            var semilla = await _semillaRepository.GetSemillaByIdAsync(id);
            if (semilla == null) throw new Exception("Semilla no encontrada");

            var movimiento = new Movimiento
            {
                IdSemilla = id,
                AnteriorUbi = semilla.Ubicacion,
                NuevaUbi = trasladoDto.NuevaUbicacion,
                FechaMovi = DateTime.Now,
                IdUsuario = trasladoDto.IdUsuario
            };

            semilla.Ubicacion = trasladoDto.NuevaUbicacion;

            await _semillaRepository.UpdateSemillaAsync(semilla);
            await _movimientoRepository.AddMovimientoAsync(movimiento);
        }


        public async Task DeleteSemillaAsync(int id)
        {
            await _semillaRepository.DeleteSemillaAsync(id);
        }
    }
}
