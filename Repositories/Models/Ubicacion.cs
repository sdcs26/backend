using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena las ubicaciones de las semillas.
/// </summary>
public partial class Ubicacion
{
    /// <summary>
    /// Identificador único de la ubicación.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID de la semilla relacionada a la ubicación.
    /// </summary>
    public int IdSemilla { get; set; }

    /// <summary>
    /// Código que identifica la ubicación de la semilla.
    /// </summary>
    public string CodigoUbi { get; set; } = null!;

    public virtual Semilla IdSemillaNavigation { get; set; } = null!;
}
