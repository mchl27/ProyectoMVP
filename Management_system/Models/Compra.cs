using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public int? IdSolicitud { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaCompra { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public string? OrdenCompra { get; set; }

    public string? Estado { get; set; }

    public string? Observaciones { get; set; }

    public string? Pago { get; set; }

    public string? ObservacionesPago { get; set; }

    public string? Logistica { get; set; }

    public string? ObservacionesLogistica { get; set; }

    public string? Bodega { get; set; }

    public string? ObservacionesBodega { get; set; }

    public string? LinkOc { get; set; }

    public string? OrdenCompra1 { get; set; }

    public string? LinkOc1 { get; set; }

    public string? OrdenCompra2 { get; set; }

    public string? LinkOc2 { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? Observaciones1 { get; set; }

    public string? Observaciones2 { get; set; }

    public string? Observaciones3 { get; set; }

    public decimal? Total { get; set; }

    public string? Aprobado { get; set; }

    public DateTime? FechaAprobado { get; set; }

    public int? IdEmpresa { get; set; }

    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual Solicitud? IdSolicitudNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<NovedadCompra> NovedadCompras { get; set; } = new List<NovedadCompra>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<RecepcionMercancium> RecepcionMercancia { get; set; } = new List<RecepcionMercancium>();
}
