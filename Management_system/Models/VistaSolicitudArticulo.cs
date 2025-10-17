using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class VistaSolicitudArticulo
{
    public int IdSolicitud { get; set; }

    public string? ConsecutivoSolicitud { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public int? IdSolicitudDetalle { get; set; }

    public int? IdProducto { get; set; }

    public decimal? CantidadSolicitud { get; set; }

    public decimal? PrecioCosto { get; set; }

    public decimal? PrecioVenta { get; set; }

    public string? OrdenCompra { get; set; }

    public DateTime? FechaCompra { get; set; }

    public string? EstadoCompra { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public string? EstadoRecepcion { get; set; }

    public decimal? CantidadRecibida { get; set; }

    public string? EstadoProducto { get; set; }

    public string? ObservacionesRecepcion { get; set; }

    public DateTime? FechaPago { get; set; }

    public string? EstadoPago { get; set; }

    public DateTime? FechaPagoDetalle { get; set; }

    public string? ComprobantePago { get; set; }

    public string? UsuarioSolicitud { get; set; }

    public string? UsuarioCompra { get; set; }

    public string? UsuarioRecepcion { get; set; }

    public string? UsuarioPago { get; set; }
}
