using System;
using System.Collections.Generic;

namespace Management_system.Models.Others.Analyze;

public class AnalyzeRecepcionViewModel
{
    public int TotalRecepciones { get; set; }
    public List<RecepcionesPorMes> RecepcionesPorMes { get; set; }
    public List<string> Asesores { get; set; }
    public List<string> Usuarios { get; set; }
    public List<RecepcionPorUsuario> RecepcionPorUsuarios { get; set; }
    public List<RecepcionPorAsesor> RecepcionPorAsesores { get; set; }
    public List<ProductoMasRecepcionados> ProductoMasRecepcionados { get; set; }
    public List<RecepcionPorProveedor> RecepcionPorProveedores { get; set; }
}

public class RecepcionesPorMes
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Count { get; set; }
}



public class RecepcionPorUsuario
{
    public string Usuario { get; set; }
    public int Count { get; set; }
}


public class RecepcionPorAsesor
{
    public string Asesor { get; set; }
    public int Count { get; set; }
}


public class ProductoMasRecepcionados
{
    public string Referencia { get; set; }
    public string Descripcion { get; set; }
    public decimal TotalCantidad { get; set; }
}

public class RecepcionPorProveedor
{
    public string Proveedor { get; set; }
    public decimal TotalComprado { get; set; }
}