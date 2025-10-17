using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Novedad
{
    public int IdNovedad { get; set; }

    public DateTime? Fecha { get; set; }

    public string? TipoNovedad { get; set; }

    public DateOnly? FechaSalida { get; set; }

    public string? Empresa { get; set; }

    public string? Direccion { get; set; }

    public string? CiudadBarrio { get; set; }

    public int? IdUsuario { get; set; }

    public string? Contacto { get; set; }

    public string? Telefono { get; set; }

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

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Rutum> Ruta { get; set; } = new List<Rutum>();
}
