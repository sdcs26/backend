using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena los tokens de recuperación de contraseña.
/// </summary>
public partial class RecuperacionToken
{
    /// <summary>
    /// Identificador único para el token de recuperación.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Correo electrónico del usuario que solicita la recuperación.
    /// </summary>
    public string Correo { get; set; } = null!;

    /// <summary>
    /// Token único generado para la recuperación de la contraseña.
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// Fecha de expiración del token de recuperación.
    /// </summary>
    public DateTime FechaExp { get; set; }
}
