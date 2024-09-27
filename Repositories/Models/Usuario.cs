using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public int NumDocumento { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
