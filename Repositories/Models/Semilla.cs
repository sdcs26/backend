using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena información sobre las semillas.
/// </summary>
public partial class Semilla
{
    /// <summary>
    /// Identificador único de la semilla.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre de la semilla.
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Código único para identificar la semilla.
    /// </summary>
    public string Codigo { get; set; } = null!;

    /// <summary>
    /// Descripción de la semilla.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Cantidad de semillas disponibles.
    /// </summary>
    public int Cantidad { get; set; }

    /// <summary>
    /// ID de la categoría a la que pertenece la semilla.
    /// </summary>
    public int IdCategoria { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    public virtual ICollection<Ubicacion> Ubicacions { get; set; } = new List<Ubicacion>();
}
