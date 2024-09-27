using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

public partial class Ubicacion
{
    public int Id { get; set; }

    public int IdSemilla { get; set; }

    public string CodigoUbi { get; set; } = null!;

    public virtual Semilla IdSemillaNavigation { get; set; } = null!;
}
