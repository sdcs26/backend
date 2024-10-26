using Sowing_O2.Dtos;
using Sowing_O2.Repositories.Models;
using Sowing_O2.Repositories;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using static Sowing_O2.Utilities.Encriptacion;
using Microsoft.EntityFrameworkCore;

namespace Sowing_O2.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepositories _pedidoRepository;
        private readonly ISemillaRepositories _semillaRepository;

        public PedidoService(IPedidoRepositories pedidoRepository, ISemillaRepositories semillaRepository)
        {
            _pedidoRepository = pedidoRepository;
            _semillaRepository = semillaRepository;
        }

        public async Task<PedidoDTO> CrearPedidoAsync(PedidoDTO pedidoDto)
        {
            // Verificar si el número de pedido ya existe
            var pedidoExistente = await _pedidoRepository.ObtenerPedidoPorNumeroAsync(pedidoDto.NumeroPedido);
            if (pedidoExistente != null)
            {
                throw new Exception("El número de pedido ya existe en la base de datos");
            }

            // Verificar si hay suficiente stock para cada producto
            foreach (var detalle in pedidoDto.Detalles)
            {
                var semilla = await _semillaRepository.GetSemillaByIdAsync(detalle.SemillaId);
                if (semilla == null || semilla.Cantidad < detalle.Cantidad)
                {
                    throw new Exception($"Cantidad insuficiente para el producto: {semilla.Nombre}");
                }

                // Descontar la cantidad de semilla del inventario
                semilla.Cantidad -= detalle.Cantidad;
                await _semillaRepository.UpdateSemillaAsync(semilla);
            }

            // Crear el pedido
            var pedido = new Pedido
            {
                NumeroPedido = pedidoDto.NumeroPedido,
                EstadoId = pedidoDto.EstadoId,
                FechaCreacion = DateTime.Now,
                NotasEnvio = pedidoDto.NotasEnvio,
                PedidoDetalles = pedidoDto.Detalles.Select(d => new PedidoDetalle
                {
                    SemillaId = d.SemillaId,
                    Cantidad = d.Cantidad
                }).ToList()
            };

            var nuevoPedido = await _pedidoRepository.CrearPedidoAsync(pedido);
            return pedidoDto; // Aquí podrías retornar un DTO transformado
        }

        public async Task<List<PedidoDTO>> ObtenerTodosLosPedidosAsync()
        {
            var pedidos = await _pedidoRepository.ObtenerTodosLosPedidosAsync();
            return pedidos.Select(p => new PedidoDTO
            {
                NumeroPedido = p.NumeroPedido,
                EstadoId = p.EstadoId,
                NotasEnvio = p.NotasEnvio,
                Detalles = p.PedidoDetalles.Select(d => new PedidoDetalleDTO
                {
                    SemillaId = d.SemillaId,
                    Cantidad = d.Cantidad
                }).ToList()
            }).ToList();
        }

        public async Task CambiarEstadoPedidoAsync(string numeroPedido, int nuevoEstadoId)
        {
            var pedido = await _pedidoRepository.ObtenerPedidoPorNumeroAsync(numeroPedido);
            if (pedido == null)
            {
                throw new Exception("Pedido no encontrado");
            }

            pedido.EstadoId = nuevoEstadoId;
            await _pedidoRepository.ActualizarPedidoAsync(pedido);
        }
    }

}
