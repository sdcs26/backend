using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Tabla que almacena los datos de los usuarios registrados en el sistema.
/// </summary>
public partial class Usuario
{
    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Número de documento del usuario.
    /// </summary>
    public int NumDocumento { get; set; }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    public string Apellido { get; set; } = null!;

    /// <summary>
    /// Correo electrónico del usuario.
    /// </summary>
    public string Correo { get; set; } = null!;

    /// <summary>
    /// Contraseña cifrada del usuario.
    /// </summary>
    public string Contrasena { get; set; } = null!;

    /// <summary>
    /// ID del rol asignado al usuario.
    /// </summary>
    public int IdRol { get; set; }

    public bool? IsActive { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}
