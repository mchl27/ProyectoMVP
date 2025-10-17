using System;
using System.Collections.Generic;

namespace Management_system.Models.Other.ViewModel.Logistica;

public class LogisticaNViewModel
{
    public Novedad Novedades { get; set; }
    public Rutum Rutas { get; set; }
}


public class LogisticaNCViewModel
{
    public NovedadComercial Novedades { get; set; }
    public Rutum Rutas { get; set; }
}

public class LogisticaNCMViewModel
{
    public NovedadCompra Novedades { get; set; }
    public Rutum Rutas { get; set; }
}