using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class NovedadComercial
{
    public int IdNovedadComercial { get; set; }

    public DateTime? Fecha { get; set; }

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

    public string? Consecutivo { get; set; }

    public string? Link { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? Observaciones1 { get; set; }

    public string? Observaciones2 { get; set; }

    public string? Observaciones3 { get; set; }

    public string? Estado { get; set; }

    public string? EstadoLogistica { get; set; }

    public string? ObservacionesLogistica { get; set; }

    public int? IdEmpresa { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Rutum> Ruta { get; set; } = new List<Rutum>();
}
