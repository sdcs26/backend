using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena los diferentes estados que pueden tener el sistema de informacion.
/// </summary>
public partial class Estado
{
    /// <summary>
    /// Identificador único del estado.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del estado.
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
