using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class VistaNovedadesGeneralesEnRutum
{
    public int IdRuta { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public int IdNovedad { get; set; }

    public DateTime? FechaNovedad { get; set; }

    public string? TipoNovedad { get; set; }

    public DateOnly? FechaSalida { get; set; }

    public string? Empresa { get; set; }

    public string? Direccion { get; set; }

    public string? CiudadBarrio { get; set; }

    public int? IdUsuario { get; set; }

    public string? Contacto { get; set; }

    public string? Telefono { get; set; }

    public string? Observaciones { get; set; }

    public int? IdConductor { get; set; }

    public int? Valor { get; set; }

    public string? Recibido { get; set; }

    public string? NGuia { get; set; }

    public int? IdEstado { get; set; }

    public string? Causa { get; set; }

    public string? ObservacionesRuta { get; set; }
}
