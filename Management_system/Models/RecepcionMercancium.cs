using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class RecepcionMercancium
{
    public int IdRecepcion { get; set; }

    public int? IdCompra { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public string? Estado { get; set; }

    public string? Observaciones { get; set; }

    public string? DocumentoRecibido { get; set; }

    public string? NumeroDocumento { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? Observaciones1 { get; set; }

    public string? Observaciones2 { get; set; }

    public string? Observaciones3 { get; set; }

    public int? IdUsuarioModificacion { get; set; }

    public int? IdEmpresa { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual Usuario? IdUsuarioModificacionNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<RecepcionDetalle> RecepcionDetalles { get; set; } = new List<RecepcionDetalle>();

    public virtual ICollection<RecepcionDocumental> RecepcionDocumentals { get; set; } = new List<RecepcionDocumental>();

    public virtual ICollection<Recepcionbodega> Recepcionbodegas { get; set; } = new List<Recepcionbodega>();
}
