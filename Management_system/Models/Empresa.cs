using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Empresa
{
    public int IdEmpresa { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Devolucion> Devolucions { get; set; } = new List<Devolucion>();

    public virtual ICollection<MRequerimiento> MRequerimientos { get; set; } = new List<MRequerimiento>();

    public virtual ICollection<NovedadComercial> NovedadComercials { get; set; } = new List<NovedadComercial>();

    public virtual ICollection<NovedadCompra> NovedadCompras { get; set; } = new List<NovedadCompra>();

    public virtual ICollection<Novedad> Novedads { get; set; } = new List<Novedad>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Proveedor> Proveedors { get; set; } = new List<Proveedor>();

    public virtual ICollection<RecepcionMercancium> RecepcionMercancia { get; set; } = new List<RecepcionMercancium>();

    public virtual ICollection<RuteroDetalle> RuteroDetalles { get; set; } = new List<RuteroDetalle>();

    public virtual ICollection<Rutero> Ruteros { get; set; } = new List<Rutero>();

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();

    public virtual ICollection<ThSeleccionPersonalSolicitud> ThSeleccionPersonalSolicituds { get; set; } = new List<ThSeleccionPersonalSolicitud>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Vendedor> Vendedors { get; set; } = new List<Vendedor>();
}
