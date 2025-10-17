using System;
using System.Collections.Generic;

namespace Management_system.Models;

public class CompraViewDetalle
{
    public List<Solicitud> Solicitudes { get; set; }
    public List<Compra> Compras { get; set; }
    public List<SolicitudDetalle> SolicitudDetalles { get; set; }
}

