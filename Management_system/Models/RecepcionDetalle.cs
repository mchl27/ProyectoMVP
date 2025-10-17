using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class RecepcionDetalle
{
    public int IdRecepcionDetalle { get; set; }

    public int? IdRecepcion { get; set; }

    public int? IdSolicitudDetalle { get; set; }

    public decimal? CantidadRecibida { get; set; }

    public string? EstadoProducto { get; set; }

    public string? Observaciones { get; set; }

    public decimal? Muestra { get; set; }

    public string? EstadoCalidad { get; set; }

    public string? ObservacionesCalidad { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? Observaciones1 { get; set; }

    public string? Observaciones2 { get; set; }

    public string? Observaciones3 { get; set; }

    public decimal? Campo1 { get; set; }

    public decimal? Campo2 { get; set; }

    public decimal? Campo3 { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public virtual ICollection<Calidad> Calidads { get; set; } = new List<Calidad>();

    public virtual RecepcionMercancium? IdRecepcionNavigation { get; set; }

    public virtual SolicitudDetalle? IdSolicitudDetalleNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<RecepcionDetalleDqr> RecepcionDetalleDqrs { get; set; } = new List<RecepcionDetalleDqr>();
}
