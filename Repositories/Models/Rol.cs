using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena los diferentes roles que pueden ser asignados a los usuarios.
/// </summary>
public partial class Rol
{
    /// <summary>
    /// Identificador único del rol.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del rol asignado (por ejemplo: Administrador, Usuario).
    /// </summary>
    public string Rol1 { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
