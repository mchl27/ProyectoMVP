using System;
using System.Collections.Generic;

namespace Management_system.Models;

public class CompraViewModel
{
    public Solicitud NuevaSolicitud { get; set; }
    public List<SolicitudDetalle> DetallesSolicitud { get; set; }
    public Compra NuevaCompra { get; set; }
    public List<CompraDetalle> Detalles { get; set; }
}



