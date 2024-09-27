using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

public partial class Movimiento
{
    public int Id { get; set; }

    public int IdSemilla { get; set; }

    public string AnteriorUbi { get; set; } = null!;

    public string NuevaUbi { get; set; } = null!;

    public DateTime FechaMovi { get; set; }

    public int IdUsuario { get; set; }

    public virtual Semilla IdSemillaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
