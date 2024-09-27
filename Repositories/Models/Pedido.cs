using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaPedido { get; set; }

    public string IdEstado { get; set; } = null!;

    public int TotalItem { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
