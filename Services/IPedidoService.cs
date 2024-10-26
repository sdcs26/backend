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
    public interface IPedidoService
    {
        Task<PedidoDTO> CrearPedidoAsync(PedidoDTO pedidoDto);
        Task<List<PedidoDTO>> ObtenerTodosLosPedidosAsync();
        Task CambiarEstadoPedidoAsync(string numeroPedido, int nuevoEstadoId);
    }

}
