using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class CompraDetalle
{
    public int IdCompraDetalle { get; set; }

    public int? IdCompra { get; set; }

    public int? IdSolicitudDetalle { get; set; }

    public string? ProveedorSugerido { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? Cantidad { get; set; }

    public decimal? Ahorro { get; set; }

    public string? ProveedorSugerido1 { get; set; }

    public decimal? PrecioUnitario1 { get; set; }

    public decimal? Cantidad1 { get; set; }

    public decimal? Ahorro1 { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual SolicitudDetalle? IdSolicitudDetalleNavigation { get; set; }
}
