using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

public partial class Categorium
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Semilla> Semillas { get; set; } = new List<Semilla>();
}
