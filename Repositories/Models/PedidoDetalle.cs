using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

public partial class PedidoDetalle
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int SemillaId { get; set; }

    public int Cantidad { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Semilla Semilla { get; set; } = null!;
}
