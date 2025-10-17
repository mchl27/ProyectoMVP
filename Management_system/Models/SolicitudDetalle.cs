using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class SolicitudDetalle
{
    public int IdSolicitudDetalle { get; set; }

    public int? IdSolicitud { get; set; }

    public int? IdProducto { get; set; }

    public string? Observaciones { get; set; }

    public decimal? Cantidad { get; set; }

    public decimal? PrecioCosto { get; set; }

    public decimal? PrecioVenta { get; set; }

    public decimal? Rentabilidad { get; set; }

    public string? Negociacion { get; set; }

    public string? ObservacionCompras { get; set; }

    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Solicitud? IdSolicitudNavigation { get; set; }

    public virtual ICollection<RecepcionDetalle> RecepcionDetalles { get; set; } = new List<RecepcionDetalle>();
}
