using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public int? Nit { get; set; }

    public string? Nombre { get; set; }

    public string? Email { get; set; }

    public string? Ciudad { get; set; }

    public int? Telefono { get; set; }

    public string? Direccion { get; set; }

    public int? IdEmpresa { get; set; }

    public string? Categoria { get; set; }

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual ICollection<NovedadComercial> NovedadComercials { get; set; } = new List<NovedadComercial>();

    public virtual ICollection<RuteroDetalle> RuteroDetalles { get; set; } = new List<RuteroDetalle>();

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();
}
