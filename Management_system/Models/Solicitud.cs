using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Solicitud
{
    public int IdSolicitud { get; set; }

    public DateTime? Fecha { get; set; }

    public int? IdCliente { get; set; }

    public int? IdProveedor { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdVendedor { get; set; }

    public string? Estado { get; set; }

    public string? Observaciones { get; set; }

    public string? ProveedorSugerido { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public string? Negociacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? ObservacionesActualizacion { get; set; }

    public string? Consecutivo { get; set; }

    public int? IdEmpresa { get; set; }

    public int? IdUsuarioAsignado { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public DateTime? FechaOc { get; set; }

    public DateTime? FechaRm { get; set; }

    public DateTime? FechaCierre { get; set; }

    public string? ConsecutivoPorEmpresa { get; set; }

    public int? IdUsuarioAprobado { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual Usuario? IdUsuarioAprobadoNavigation { get; set; }

    public virtual Usuario? IdUsuarioAsignadoNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual Vendedor? IdVendedorNavigation { get; set; }

    public virtual ICollection<SolicitudDetalle> SolicitudDetalles { get; set; } = new List<SolicitudDetalle>();

    public virtual ICollection<SolicitudDocumento> SolicitudDocumentos { get; set; } = new List<SolicitudDocumento>();
}
