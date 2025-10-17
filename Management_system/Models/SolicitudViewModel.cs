using System;
using System.Collections.Generic;

namespace Management_system.Models;

public class SolicitudViewModel
{
    public Solicitud NuevaSolicitud { get; set; }
    public List<SolicitudDetalle> DetallesSolicitud { get; set; }
}