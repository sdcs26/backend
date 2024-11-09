using Sowing_O2.Repositories.Models;

namespace Sowing_O2.Repositories
{
    public interface IPedidoRepositories
    {
        Task<Pedido> CrearPedidoAsync(Pedido pedido);
        Task<Pedido> ObtenerPedidoPorNumeroAsync(string numeroPedido);
        Task<List<Pedido>> ObtenerTodosLosPedidosAsync();
        Task ActualizarPedidoAsync(Pedido pedido);
        Task EliminarPedidoAsync(int pedidoId);
    }
}
