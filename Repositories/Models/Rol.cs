﻿using System;
using System.Collections.Generic;

namespace Sowing_O2.Repositories.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string Rol1 { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
