using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Rutum
{
    public int IdRuta { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdNovedadCompras { get; set; }

    public int? IdNovedadComercial { get; set; }

    public int? IdNovedadGeneral { get; set; }

    public int? IdConductor { get; set; }

    public int? Valor { get; set; }

    public string? Recibido { get; set; }

    public string? NGuia { get; set; }

    public int? IdEstado { get; set; }

    public string? Causa { get; set; }

    public string? Observaciones { get; set; }

    public string? Link { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? Observaciones1 { get; set; }

    public string? Observaciones2 { get; set; }

    public string? Observaciones3 { get; set; }

    public decimal? Campo1 { get; set; }

    public decimal? Campo2 { get; set; }

    public decimal? Campo3 { get; set; }

    public int? Campo4 { get; set; }

    public int? Campo5 { get; set; }

    public int? Campo6 { get; set; }

    public string? Estado { get; set; }

    public string? EstadoLogistica { get; set; }

    public string? ObservacionesLogistica { get; set; }

    public DateOnly? FechaRuta { get; set; }

    public DateTime? FechaCierre { get; set; }

    public virtual Conductor? IdConductorNavigation { get; set; }

    public virtual EstadoEntrega? IdEstadoNavigation { get; set; }

    public virtual NovedadComercial? IdNovedadComercialNavigation { get; set; }

    public virtual NovedadCompra? IdNovedadComprasNavigation { get; set; }

    public virtual Novedad? IdNovedadGeneralNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
