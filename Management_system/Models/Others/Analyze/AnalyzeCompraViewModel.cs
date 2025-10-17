using System;
using System.Collections.Generic;

namespace Management_system.Models.Others.Analyze;

public class AnalyzeCompraViewModel
{
    public int TotalCompras { get; set; }
    public List<CompraPorMes> ComprasPorMes { get; set; }
    public List<string> Asesores { get; set; }
    public List<string> Usuarios { get; set; }
    public List<string> Proveedores { get; set; }
    public List<CompraPorUsuario> ComprasPorUsuario { get; set; }
    public List<CompraPorAsesor> ComprasPorAsesor { get; set; }
    public List<CompraPorProveedor> ComprasPorProveedor { get; set; }
    public List<CompraValorPorMes> ComprasValorPorMes { get; set; }
    public List<CompraValorPorProveedor> ComprasValorPorProveedor { get; set; }
    public List<CompraValorPorUsuario> ComprasValorPorUsuario { get; set; }
}

public class CompraPorMes
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Count { get; set; }
}

public class CompraPorUsuario
{
    public string Usuario { get; set; }
    public int Count { get; set; }
}

public class CompraPorAsesor
{
    public string Asesor { get; set; }
    public int Count { get; set; }
}

public class CompraPorProveedor
{
    public string Proveedor { get; set; }
    public int Count { get; set; }
}


public class CompraValorPorMes
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalValor { get; set; }
}

public class CompraValorPorProveedor
{
    public string Proveedor { get; set; }
    public decimal TotalValor { get; set; }
}

public class CompraValorPorUsuario
{
    public string Usuario { get; set; }
    public decimal TotalValor { get; set; }
}
