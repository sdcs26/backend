using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena los movimientos de los elementos de un lugar a otro.
/// </summary>
public partial class Movimiento
{
    /// <summary>
    /// Identificador único del movimiento.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID del elemento o semilla que ha sido movido.
    /// </summary>
    public int IdSemilla { get; set; }

    /// <summary>
    /// Ubicación anterior del elemento antes del movimiento.
    /// </summary>
    public string AnteriorUbi { get; set; } = null!;

    /// <summary>
    /// Nueva ubicación del elemento después del movimiento.
    /// </summary>
    public string NuevaUbi { get; set; } = null!;

    /// <summary>
    /// Fecha en la que se realizó el movimiento.
    /// </summary>
    public DateTime FechaMovi { get; set; }

    /// <summary>
    /// ID del usuario que realizó el movimiento.
    /// </summary>
    public int IdUsuario { get; set; }

    public virtual Semilla IdSemillaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
