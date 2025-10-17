using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Vendedor
{
    public int IdVendedor { get; set; }

    public int? Identificacion { get; set; }

    public string? Nombre { get; set; }

    public int? IdArea { get; set; }

    public int? Cargo { get; set; }

    public string? Estado { get; set; }

    public int? IdEmpresa { get; set; }

    public virtual Cargo? CargoNavigation { get; set; }

    public virtual ICollection<Devolucion> Devolucions { get; set; } = new List<Devolucion>();

    public virtual Area? IdAreaNavigation { get; set; }

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual ICollection<Rutero> Ruteros { get; set; } = new List<Rutero>();

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();
}
