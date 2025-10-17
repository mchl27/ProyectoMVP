using System;
using System.Collections.Generic;

namespace Management_system.Models.Others.Analyze;

public class AnalyzeSolicitudViewModel
{
    public int TotalSolicitudes { get; set; }
    public List<SolicitudPorMes> SolicitudesPorMes { get; set; }
    public List<VentasPorMes> VentasPorMes { get; set; } // Agregar este campo
    public List<string> Asesores { get; set; }
    public List<string> Usuarios { get; set; }
    public List<SolicitudPorUsuario> SolicitudesPorUsuario { get; set; }
    public List<SolicitudPorAsesor> SolicitudesPorAsesor { get; set; }
    public List<ProductoMasSolicitado> ProductosMasSolicitados { get; set; }
    public List<ComprasPorProveedor> ComprasPorProveedor { get; set; }
    public List<ComprasPorCliente> ComprasPorCliente { get; set; }
}

public class VentasPorMes
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalVentas { get; set; }
}

public class SolicitudPorMes
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Count { get; set; }
}

public class SolicitudPorUsuario
{
    public string Usuario { get; set; }
    public int Count { get; set; }
}

public class SolicitudPorAsesor
{
    public string Asesor { get; set; }
    public int Count { get; set; }
}

public class ProductoMasSolicitado
{
    public string Referencia { get; set; }
    public string Descripcion { get; set; }
    public decimal TotalCantidad { get; set; }
}

public class ComprasPorProveedor
{
    public string Proveedor { get; set; }
    public decimal TotalComprado { get; set; }
}

public class ComprasPorCliente
{
    public string Cliente { get; set; }
    public decimal TotalComprado { get; set; }
}