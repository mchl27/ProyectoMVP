using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Cargo
{
    public int IdCargo { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Vendedor> Vendedors { get; set; } = new List<Vendedor>();
}
