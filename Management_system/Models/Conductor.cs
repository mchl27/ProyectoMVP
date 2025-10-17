using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Conductor
{
    public int IdConductor { get; set; }

    public int? Identificacion { get; set; }

    public string? Nombre { get; set; }

    public string? PlacaVehiculo { get; set; }

    public int? Telefono { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Rutum> Ruta { get; set; } = new List<Rutum>();

    public virtual ICollection<VehiculoTrazabilidad> VehiculoTrazabilidads { get; set; } = new List<VehiculoTrazabilidad>();
}
