using Microsoft.EntityFrameworkCore;
using Sowing_O2.Repositories.Models;
using System;

namespace Sowing_O2.Repositories
{
    public class PedidoRepositories : IPedidoRepositories
    {
        private readonly SowingO2PruebaContext _context;

        public PedidoRepositories(SowingO2PruebaContext context)
        {
            _context = context;
        }

        public async Task<Pedido> CrearPedidoAsync(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> ObtenerPedidoPorNumeroAsync(string numeroPedido)
        {
            return await _context.Pedidos
                .Include(p => p.Estado)
                .Include(p => p.PedidoDetalles)
                .ThenInclude(pd => pd.Semilla)
                .FirstOrDefaultAsync(p => p.NumeroPedido == numeroPedido);
        }

        public async Task<List<Pedido>> ObtenerTodosLosPedidosAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Estado)
                .Include(p => p.PedidoDetalles)
                .ThenInclude(pd => pd.Semilla)
                .ToListAsync();
        }

        public async Task ActualizarPedidoAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarPedidoAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }
    }

}
