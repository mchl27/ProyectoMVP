using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? Empresa { get; set; }

    public string? Referencia { get; set; }

    public string? Descripcion { get; set; }

    public string? Unidad { get; set; }

    public string? Estado { get; set; }

    public string? Link { get; set; }

    public string? Proveedor { get; set; }

    public DateTime? UltimoIngreso { get; set; }

    public decimal? Precio { get; set; }

    public int? IdEmpresa { get; set; }

    public virtual ICollection<CalidadReferencia> CalidadReferencia { get; set; } = new List<CalidadReferencia>();

    public virtual ICollection<DevolucionDetalle> DevolucionDetalles { get; set; } = new List<DevolucionDetalle>();

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual ICollection<SolicitudDetalle> SolicitudDetalles { get; set; } = new List<SolicitudDetalle>();
}
