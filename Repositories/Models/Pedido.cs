using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public string NumeroPedido { get; set; } = null!;

    public int EstadoId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public string? NotasEnvio { get; set; }

    public virtual Estado Estado { get; set; } = null!;

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
