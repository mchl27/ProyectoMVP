using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class VistaNovedadesComercialesEnRutum
{
    public int IdRuta { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public int IdNovedad { get; set; }

    public DateTime? FechaNovedad { get; set; }

    public string? TipoDocumento { get; set; }

    public int? NumeroDocumento { get; set; }

    public string? TipoServicio { get; set; }

    public DateOnly? FechaSalida { get; set; }

    public string? Empresa { get; set; }

    public string? Direccion { get; set; }

    public int? IdCliente { get; set; }

    public int? IdUsuario { get; set; }

    public string? TipoPago { get; set; }

    public string? Observaciones { get; set; }

    public int? IdConductor { get; set; }

    public int? Valor { get; set; }

    public string? Recibido { get; set; }

    public string? NGuia { get; set; }

    public int? IdEstado { get; set; }

    public string? Causa { get; set; }

    public string? ObservacionesRuta { get; set; }
}
