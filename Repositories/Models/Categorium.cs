using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena las categorías de las semillas.
/// </summary>
public partial class Categorium
{
    /// <summary>
    /// Identificador único de la categoría.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre de la categoría.
    /// </summary>
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Semilla> Semillas { get; set; } = new List<Semilla>();
}
