using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

public partial class Semilla
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Cantidad { get; set; }

    public int IdCategoria { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

    public virtual ICollection<Ubicacion> Ubicacions { get; set; } = new List<Ubicacion>();
}
