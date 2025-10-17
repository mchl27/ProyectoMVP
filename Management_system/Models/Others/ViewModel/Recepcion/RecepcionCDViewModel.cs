using System;
using System.Collections.Generic;

namespace Management_system.Models.Other.ViewModel;

public class RecepcionCDViewModel
{
    public int IdSolicitudDetalle { get; set; }
    public int? IdSolicitud { get; set; }
    public int? IdProducto { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal? CantidadSolicitada { get; set; }
    public decimal? CantidadRecibida { get; set; }
    public string EstadoIngreso => CantidadRecibida > 0 ? "Ingresado" : "No hay ingreso";
}
