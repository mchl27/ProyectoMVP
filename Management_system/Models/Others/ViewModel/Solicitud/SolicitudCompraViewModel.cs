using System;
using System.Collections.Generic;

namespace Management_system.Models;

public class SolicitudCompraIndexViewModel
{
    public Solicitud Solicitudes { get; set; }
    public List<SolicitudDetalle> SolicitudDetalles { get; set; }
}

public class SolicitudCompraDetailsViewModel
{
    public Solicitud Solicitudes { get; set; }
    public List<SolicitudDetalle> SolicitudDetalles { get; set; }
}

public class SolicitudCompraPDFViewModel
{
    public Solicitud Solicitudes { get; set; }
    public List<SolicitudDetalle> SolicitudDetalles { get; set; }
}

public class SolicitudCompraPDF1ViewModel
{
    public Solicitud Solicitudes { get; set; }
    public List<SolicitudDetalle> SolicitudDetalles { get; set; }
}

public class SolicitudCompraEditViewModel
{
    public Solicitud Solicitudes { get; set; }
    public List<SolicitudDetalle> SolicitudDetalles { get; set; }
}

public class SolicitudCompraSeguimientoViewModel
{
    public Solicitud Solicitudes { get; set; }
    public List<SolicitudDetalle> SolicitudDetalles { get; set; }
}

public class SolicitudCompraSeguimientoPDFViewModel
{
    public Solicitud Solicitudes { get; set; }
    public List<SolicitudDetalle> SolicitudDetalles { get; set; }
}

// VIEW COMPRA
public class SolicitudCompraReviewViewModel
{
    public Solicitud Solicitudes { get; set; }
    public List<SolicitudDetalle> SolicitudDetalles { get; set; }
}