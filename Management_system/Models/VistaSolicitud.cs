using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class VistaSolicitud
{
    public int IdRequerimiento { get; set; }

    public DateTime? FechaRequerimiento { get; set; }

    public string? OrdenCompra { get; set; }

    public DateTime? FechaCompra { get; set; }

    public string? NovedadCompra { get; set; }

    public DateTime? FechaRecepcionMercancia { get; set; }

    public decimal? CantidadRecibida { get; set; }

    public string? EstadoProducto { get; set; }

    public string? ObservacionesRecepcion { get; set; }
}
