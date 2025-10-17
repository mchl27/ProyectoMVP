using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public DateTime? Creacion { get; set; }

    public int? Identificacion { get; set; }

    public string? Nombre { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Contraseña { get; set; }

    public string? Estado { get; set; }

    public int? IdRol { get; set; }

    public int? IdEmpresa { get; set; }

    public int? IdArea { get; set; }

    public string? RutaFoto { get; set; }

    public virtual ICollection<CalidadDetalle> CalidadDetalles { get; set; } = new List<CalidadDetalle>();

    public virtual ICollection<Calidad> Calidads { get; set; } = new List<Calidad>();

    public virtual ICollection<ChatMensaje> ChatMensajeIdEmisorNavigations { get; set; } = new List<ChatMensaje>();

    public virtual ICollection<ChatMensaje> ChatMensajeIdReceptorNavigations { get; set; } = new List<ChatMensaje>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Devolucion> Devolucions { get; set; } = new List<Devolucion>();

    public virtual Area? IdAreaNavigation { get; set; }

    public virtual Empresa? IdEmpresaNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<MPrestamo> MPrestamos { get; set; } = new List<MPrestamo>();

    public virtual ICollection<MRequerimiento> MRequerimientoIdAsignadoNavigations { get; set; } = new List<MRequerimiento>();

    public virtual ICollection<MRequerimiento> MRequerimientoIdObservadorNavigations { get; set; } = new List<MRequerimiento>();

    public virtual ICollection<MRequerimiento> MRequerimientoIdUsuarioNavigations { get; set; } = new List<MRequerimiento>();

    public virtual ICollection<MRequerimiento> MRequerimientoIdUsuariofinalNavigations { get; set; } = new List<MRequerimiento>();

    public virtual ICollection<MRequerimientosEstado> MRequerimientosEstados { get; set; } = new List<MRequerimientosEstado>();

    public virtual ICollection<MSeguimientoRequerimiento> MSeguimientoRequerimientos { get; set; } = new List<MSeguimientoRequerimiento>();

    public virtual ICollection<NovedadComercial> NovedadComercials { get; set; } = new List<NovedadComercial>();

    public virtual ICollection<NovedadCompra> NovedadCompras { get; set; } = new List<NovedadCompra>();

    public virtual ICollection<Novedad> Novedads { get; set; } = new List<Novedad>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<PagosCombrobante> PagosCombrobantes { get; set; } = new List<PagosCombrobante>();

    public virtual ICollection<PagosDetalle> PagosDetalles { get; set; } = new List<PagosDetalle>();

    public virtual ICollection<RecepcionContabilidad> RecepcionContabilidads { get; set; } = new List<RecepcionContabilidad>();

    public virtual ICollection<RecepcionDetalleDqr> RecepcionDetalleDqrs { get; set; } = new List<RecepcionDetalleDqr>();

    public virtual ICollection<RecepcionDetalle> RecepcionDetalles { get; set; } = new List<RecepcionDetalle>();

    public virtual ICollection<RecepcionDocumental> RecepcionDocumentalIdUsuarioDocumentacionNavigations { get; set; } = new List<RecepcionDocumental>();

    public virtual ICollection<RecepcionDocumental> RecepcionDocumentalIdUsuarioRecepcionNavigations { get; set; } = new List<RecepcionDocumental>();

    public virtual ICollection<RecepcionMercancium> RecepcionMercanciumIdUsuarioModificacionNavigations { get; set; } = new List<RecepcionMercancium>();

    public virtual ICollection<RecepcionMercancium> RecepcionMercanciumIdUsuarioNavigations { get; set; } = new List<RecepcionMercancium>();

    public virtual ICollection<Recepcionbodega> Recepcionbodegas { get; set; } = new List<Recepcionbodega>();

    public virtual ICollection<Rutum> Ruta { get; set; } = new List<Rutum>();

    public virtual ICollection<RuteroDetalle> RuteroDetalles { get; set; } = new List<RuteroDetalle>();

    public virtual ICollection<RuteroRevision> RuteroRevisions { get; set; } = new List<RuteroRevision>();

    public virtual ICollection<RuteroVenta> RuteroVenta { get; set; } = new List<RuteroVenta>();

    public virtual ICollection<RuteroVisita> RuteroVisita { get; set; } = new List<RuteroVisita>();

    public virtual ICollection<Rutero> Ruteros { get; set; } = new List<Rutero>();

    public virtual ICollection<SolicitudDocumento> SolicitudDocumentos { get; set; } = new List<SolicitudDocumento>();

    public virtual ICollection<Solicitud> SolicitudIdUsuarioAprobadoNavigations { get; set; } = new List<Solicitud>();

    public virtual ICollection<Solicitud> SolicitudIdUsuarioAsignadoNavigations { get; set; } = new List<Solicitud>();

    public virtual ICollection<Solicitud> SolicitudIdUsuarioNavigations { get; set; } = new List<Solicitud>();

    public virtual ICollection<Tarea> TareaIdUsuarioAsignadaNavigations { get; set; } = new List<Tarea>();

    public virtual ICollection<Tarea> TareaIdUsuarioNavigations { get; set; } = new List<Tarea>();

    public virtual ICollection<TareasAdjunto> TareasAdjuntos { get; set; } = new List<TareasAdjunto>();

    public virtual ICollection<TareasAlerta> TareasAlerta { get; set; } = new List<TareasAlerta>();

    public virtual ICollection<TareasSubAdjunto> TareasSubAdjuntos { get; set; } = new List<TareasSubAdjunto>();

    public virtual ICollection<TareasSub> TareasSubs { get; set; } = new List<TareasSub>();

    public virtual ICollection<TareasTrazabilidad> TareasTrazabilidads { get; set; } = new List<TareasTrazabilidad>();

    public virtual ICollection<ThDescargoCaso> ThDescargoCasos { get; set; } = new List<ThDescargoCaso>();

    public virtual ICollection<ThDescargoCitacion> ThDescargoCitacions { get; set; } = new List<ThDescargoCitacion>();

    public virtual ICollection<ThDescargosDiligenciar> ThDescargosDiligenciars { get; set; } = new List<ThDescargosDiligenciar>();

    public virtual ICollection<ThDescargosProceso> ThDescargosProcesos { get; set; } = new List<ThDescargosProceso>();

    public virtual ICollection<ThRequerimiento> ThRequerimientos { get; set; } = new List<ThRequerimiento>();

    public virtual ICollection<ThRequerimientosDocumento> ThRequerimientosDocumentos { get; set; } = new List<ThRequerimientosDocumento>();

    public virtual ICollection<ThSeguimientoRequerimiento> ThSeguimientoRequerimientos { get; set; } = new List<ThSeguimientoRequerimiento>();

    public virtual ICollection<ThSeguimientoSolicitud> ThSeguimientoSolicituds { get; set; } = new List<ThSeguimientoSolicitud>();

    public virtual ICollection<ThSeleccionPersonalCandidato> ThSeleccionPersonalCandidatos { get; set; } = new List<ThSeleccionPersonalCandidato>();

    public virtual ICollection<ThSeleccionPersonalCaso> ThSeleccionPersonalCasos { get; set; } = new List<ThSeleccionPersonalCaso>();

    public virtual ICollection<ThSeleccionPersonalContratacione> ThSeleccionPersonalContrataciones { get; set; } = new List<ThSeleccionPersonalContratacione>();

    public virtual ICollection<ThSeleccionPersonalEntrevista> ThSeleccionPersonalEntrevista { get; set; } = new List<ThSeleccionPersonalEntrevista>();

    public virtual ICollection<ThSeleccionPersonalExamene> ThSeleccionPersonalExamenes { get; set; } = new List<ThSeleccionPersonalExamene>();

    public virtual ICollection<ThSeleccionPersonalSolicitud> ThSeleccionPersonalSolicituds { get; set; } = new List<ThSeleccionPersonalSolicitud>();

    public virtual ICollection<ThSolicitudPermiso> ThSolicitudPermisos { get; set; } = new List<ThSolicitudPermiso>();

    public virtual ICollection<ThSolicitudesDescargo> ThSolicitudesDescargos { get; set; } = new List<ThSolicitudesDescargo>();

    public virtual ICollection<ThSolicitudesDescargosTestigo> ThSolicitudesDescargosTestigos { get; set; } = new List<ThSolicitudesDescargosTestigo>();

    public virtual ICollection<VehiculoTrazabilidad> VehiculoTrazabilidads { get; set; } = new List<VehiculoTrazabilidad>();
}
