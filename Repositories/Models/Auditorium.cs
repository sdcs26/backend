using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

/// <summary>
/// Registro de las acciones realizadas sobre los usuarios
/// </summary>
public partial class Auditorium
{
    /// <summary>
    /// Identificador único para la auditoría
    /// </summary>
    public int IdAuditoria { get; set; }

    /// <summary>
    /// Tipo de acción realizada (Inserción, Eliminación)
    /// </summary>
    public string? Accion { get; set; }

    /// <summary>
    /// ID del usuario sobre el que se realizó la acción
    /// </summary>
    public int? IdUsuario { get; set; }

    /// <summary>
    /// Número de documento del usuario afectado
    /// </summary>
    public string? NumDocumento { get; set; }

    /// <summary>
    /// Nombre del usuario afectado
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Apellido del usuario afectado
    /// </summary>
    public string? Apellido { get; set; }

    /// <summary>
    /// Correo electrónico del usuario afectado
    /// </summary>
    public string? Correo { get; set; }

    /// <summary>
    /// Contraseña del usuario afectado
    /// </summary>
    public string? Contrasena { get; set; }

    /// <summary>
    /// ID del rol asignado al usuario afectado
    /// </summary>
    public int? IdRol { get; set; }

    /// <summary>
    /// Fecha y hora en que se realizó la acción
    /// </summary>
    public DateTime? Fecha { get; set; }
}
