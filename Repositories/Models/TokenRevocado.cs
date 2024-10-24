using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena los tokens que han sido revocados.
/// </summary>
public partial class TokenRevocado
{
    /// <summary>
    /// Identificador único del token revocado.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Token revocado que ya no es válido para el acceso.
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// Fecha en la que el token fue revocado.
    /// </summary>
    public DateTime FechaRevocado { get; set; }
}
