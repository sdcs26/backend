using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena los pedidos realizados por los usuarios.
/// </summary>
public partial class Pedido
{
    /// <summary>
    /// Identificador único del pedido.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID del usuario que realizó el pedido.
    /// </summary>
    public int IdUsuario { get; set; }

    /// <summary>
    /// Fecha en la que se realizó el pedido.
    /// </summary>
    public DateTime FechaPedido { get; set; }

    /// <summary>
    /// Estado actual del pedido (por ejemplo: En proceso, Enviado, Entregado).
    /// </summary>
    public string IdEstado { get; set; } = null!;

    /// <summary>
    /// Número total de ítems en el pedido.
    /// </summary>
    public int TotalItem { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
