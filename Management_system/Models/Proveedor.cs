using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public int? Nit { get; set; }

    public string? Nombre { get; set; }

    public string? Email { get; set; }

    public string? Ciudad { get; set; }

    public int? Telefono { get; set; }

    public string? Descripcion { get; set; }

    public string? Direccion { get; set; }

    public int? IdEmpresa { get; set; }

    public string? Categoria { get; set; }

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();
}
