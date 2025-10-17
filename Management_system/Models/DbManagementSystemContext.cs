using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Management_system.Models;

public partial class DbManagementSystemContext : DbContext
{
    public DbManagementSystemContext()
    {
    }

    public DbManagementSystemContext(DbContextOptions<DbManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<Calidad> Calidads { get; set; }

    public virtual DbSet<CalidadDetalle> CalidadDetalles { get; set; }

    public virtual DbSet<CalidadReferencia> CalidadReferencias { get; set; }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<ChatMensaje> ChatMensajes { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<CompraDetalle> CompraDetalles { get; set; }

    public virtual DbSet<Conductor> Conductors { get; set; }

    public virtual DbSet<Devolucion> Devolucions { get; set; }

    public virtual DbSet<DevolucionDetalle> DevolucionDetalles { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<EstadoEntrega> EstadoEntregas { get; set; }

    public virtual DbSet<MInventario> MInventarios { get; set; }

    public virtual DbSet<MPrestamo> MPrestamos { get; set; }

    public virtual DbSet<MRequerimiento> MRequerimientos { get; set; }

    public virtual DbSet<MRequerimientosEstado> MRequerimientosEstados { get; set; }

    public virtual DbSet<MSeguimientoPrestamo> MSeguimientoPrestamos { get; set; }

    public virtual DbSet<MSeguimientoRequerimiento> MSeguimientoRequerimientos { get; set; }

    public virtual DbSet<MTipoPrestamo> MTipoPrestamos { get; set; }

    public virtual DbSet<MTipoRequerimiento> MTipoRequerimientos { get; set; }

    public virtual DbSet<Novedad> Novedads { get; set; }

    public virtual DbSet<NovedadComercial> NovedadComercials { get; set; }

    public virtual DbSet<NovedadCompra> NovedadCompras { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PagosCombrobante> PagosCombrobantes { get; set; }

    public virtual DbSet<PagosDetalle> PagosDetalles { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<RecepcionContabilidad> RecepcionContabilidads { get; set; }

    public virtual DbSet<RecepcionDetalle> RecepcionDetalles { get; set; }

    public virtual DbSet<RecepcionDetalleDqr> RecepcionDetalleDqrs { get; set; }

    public virtual DbSet<RecepcionDocumental> RecepcionDocumentals { get; set; }

    public virtual DbSet<RecepcionMercancium> RecepcionMercancia { get; set; }

    public virtual DbSet<Recepcionbodega> Recepcionbodegas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Rutero> Ruteros { get; set; }

    public virtual DbSet<RuteroDetalle> RuteroDetalles { get; set; }

    public virtual DbSet<RuteroRevision> RuteroRevisions { get; set; }

    public virtual DbSet<RuteroVenta> RuteroVentas { get; set; }

    public virtual DbSet<RuteroVisita> RuteroVisitas { get; set; }

    public virtual DbSet<Rutum> Ruta { get; set; }

    public virtual DbSet<Solicitud> Solicituds { get; set; }

    public virtual DbSet<SolicitudDetalle> SolicitudDetalles { get; set; }

    public virtual DbSet<SolicitudDocumento> SolicitudDocumentos { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<TareasAdjunto> TareasAdjuntos { get; set; }

    public virtual DbSet<TareasAlerta> TareasAlertas { get; set; }

    public virtual DbSet<TareasSub> TareasSubs { get; set; }

    public virtual DbSet<TareasSubAdjunto> TareasSubAdjuntos { get; set; }

    public virtual DbSet<TareasTrazabilidad> TareasTrazabilidads { get; set; }

    public virtual DbSet<ThDescargoCaso> ThDescargoCasos { get; set; }

    public virtual DbSet<ThDescargoCitacion> ThDescargoCitacions { get; set; }

    public virtual DbSet<ThDescargosDiligenciar> ThDescargosDiligenciars { get; set; }

    public virtual DbSet<ThDescargosProceso> ThDescargosProcesos { get; set; }

    public virtual DbSet<ThRequerimiento> ThRequerimientos { get; set; }

    public virtual DbSet<ThRequerimientosDocumento> ThRequerimientosDocumentos { get; set; }

    public virtual DbSet<ThSeguimientoRequerimiento> ThSeguimientoRequerimientos { get; set; }

    public virtual DbSet<ThSeguimientoSolicitud> ThSeguimientoSolicituds { get; set; }

    public virtual DbSet<ThSeleccionPersonalCandidato> ThSeleccionPersonalCandidatos { get; set; }

    public virtual DbSet<ThSeleccionPersonalCaso> ThSeleccionPersonalCasos { get; set; }

    public virtual DbSet<ThSeleccionPersonalContratacione> ThSeleccionPersonalContrataciones { get; set; }

    public virtual DbSet<ThSeleccionPersonalEntrevista> ThSeleccionPersonalEntrevistas { get; set; }

    public virtual DbSet<ThSeleccionPersonalExamene> ThSeleccionPersonalExamenes { get; set; }

    public virtual DbSet<ThSeleccionPersonalSolicitud> ThSeleccionPersonalSolicituds { get; set; }

    public virtual DbSet<ThSolicitudPermiso> ThSolicitudPermisos { get; set; }

    public virtual DbSet<ThSolicitudesDescargo> ThSolicitudesDescargos { get; set; }

    public virtual DbSet<ThSolicitudesDescargosTestigo> ThSolicitudesDescargosTestigos { get; set; }

    public virtual DbSet<ThTrabajadore> ThTrabajadores { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    public virtual DbSet<VehiculoDocumento> VehiculoDocumentos { get; set; }

    public virtual DbSet<VehiculoTrazabilidad> VehiculoTrazabilidads { get; set; }

    public virtual DbSet<Vendedor> Vendedors { get; set; }

    public virtual DbSet<VistaNovedadesComercialesEnRutum> VistaNovedadesComercialesEnRuta { get; set; }

    public virtual DbSet<VistaNovedadesComprasEnRutum> VistaNovedadesComprasEnRuta { get; set; }

    public virtual DbSet<VistaNovedadesGeneralesEnRutum> VistaNovedadesGeneralesEnRuta { get; set; }

    public virtual DbSet<VistaSolicitud> VistaSolicituds { get; set; }

    public virtual DbSet<VistaSolicitudArticulo> VistaSolicitudArticulos { get; set; }

    public virtual DbSet<VistaSolicitudDetalle> VistaSolicitudDetalles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=UDC_DES01\\SQLEXPRESS;Database=DB_management_system;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.IdArea).HasName("PK__Area__F9279C8014C574E6");

            entity.ToTable("Area");

            entity.Property(e => e.IdArea).HasColumnName("Id_area");
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__835B9D145D34EFA0");

            entity.Property(e => e.IdAuditoria).HasColumnName("Id_auditoria");
            entity.Property(e => e.Area).HasMaxLength(100);
            entity.Property(e => e.Cargo).HasMaxLength(100);
            entity.Property(e => e.Documento).HasMaxLength(200);
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Usuario).HasMaxLength(100);
        });

        modelBuilder.Entity<Calidad>(entity =>
        {
            entity.HasKey(e => e.IdCalidad).HasName("PK__Calidad__16A7418DE2789E25");

            entity.ToTable("Calidad");

            entity.Property(e => e.IdCalidad).HasColumnName("Id_calidad");
            entity.Property(e => e.Consecutivo)
                .HasMaxLength(17)
                .HasComputedColumnSql("('SGC-CLD-REV-'+right('00000'+CONVERT([nvarchar](10),[Id_calidad]),(5)))", true);
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaInspeccion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_inspeccion");
            entity.Property(e => e.IdRecepcionDetalle).HasColumnName("Id_recepcion_detalle");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Resultado).HasMaxLength(50);

            entity.HasOne(d => d.IdRecepcionDetalleNavigation).WithMany(p => p.Calidads)
                .HasForeignKey(d => d.IdRecepcionDetalle)
                .HasConstraintName("FK__Calidad__Consecu__04E4BC85");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Calidads)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Calidad__Id_usua__05D8E0BE");
        });

        modelBuilder.Entity<CalidadDetalle>(entity =>
        {
            entity.HasKey(e => e.IdCalidadDetalle).HasName("PK__Calidad___B804ECAC64294D40");

            entity.ToTable("Calidad_detalle");

            entity.Property(e => e.IdCalidadDetalle).HasColumnName("Id_calidad_detalle");
            entity.Property(e => e.IdCalidad).HasColumnName("Id_calidad");
            entity.Property(e => e.IdCalidadReferencias).HasColumnName("Id_calidad_referencias");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Promedio).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.RangoMax)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Rango_max");
            entity.Property(e => e.RangoMin)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Rango_min");

            entity.HasOne(d => d.IdCalidadNavigation).WithMany(p => p.CalidadDetalles)
                .HasForeignKey(d => d.IdCalidad)
                .HasConstraintName("FK__Calidad_d__Id_ca__08B54D69");

            entity.HasOne(d => d.IdCalidadReferenciasNavigation).WithMany(p => p.CalidadDetalles)
                .HasForeignKey(d => d.IdCalidadReferencias)
                .HasConstraintName("FK__Calidad_d__Id_ca__09A971A2");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.CalidadDetalles)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Calidad_d__Id_us__0A9D95DB");
        });

        modelBuilder.Entity<CalidadReferencia>(entity =>
        {
            entity.HasKey(e => e.IdCalidadReferencias).HasName("PK__Calidad___AC16EB9DD67F47C4");

            entity.ToTable("Calidad_referencias");

            entity.Property(e => e.IdCalidadReferencias).HasColumnName("Id_calidad_referencias");
            entity.Property(e => e.Caracteristica1).HasMaxLength(150);
            entity.Property(e => e.Caracteristica10).HasMaxLength(150);
            entity.Property(e => e.Caracteristica11).HasMaxLength(150);
            entity.Property(e => e.Caracteristica12).HasMaxLength(150);
            entity.Property(e => e.Caracteristica2).HasMaxLength(150);
            entity.Property(e => e.Caracteristica3).HasMaxLength(150);
            entity.Property(e => e.Caracteristica4).HasMaxLength(150);
            entity.Property(e => e.Caracteristica5).HasMaxLength(150);
            entity.Property(e => e.Caracteristica6).HasMaxLength(150);
            entity.Property(e => e.Caracteristica7).HasMaxLength(150);
            entity.Property(e => e.Caracteristica8).HasMaxLength(150);
            entity.Property(e => e.Caracteristica9).HasMaxLength(150);
            entity.Property(e => e.IdProducto).HasColumnName("Id_producto");
            entity.Property(e => e.NombreProducto).HasMaxLength(255);
            entity.Property(e => e.RutaPdf)
                .HasMaxLength(255)
                .HasColumnName("RutaPDF");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.CalidadReferencia)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__Calidad_r__Id_pr__02084FDA");
        });

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PK__Cargo__170A99B415BD5985");

            entity.ToTable("Cargo");

            entity.Property(e => e.IdCargo).HasColumnName("Id_cargo");
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<ChatMensaje>(entity =>
        {
            entity.HasKey(e => e.IdMensajesPrivados).HasName("PK__ChatMens__C9B6BBDB7C44D8E5");

            entity.Property(e => e.IdMensajesPrivados).HasColumnName("Id_MensajesPrivados");
            entity.Property(e => e.FechaEdit).HasColumnType("datetime");
            entity.Property(e => e.FechaEnvio).HasColumnType("datetime");
            entity.Property(e => e.IdEmisor).HasColumnName("Id_Emisor");
            entity.Property(e => e.IdReceptor).HasColumnName("Id_Receptor");
            entity.Property(e => e.Leido).HasDefaultValue(false);

            entity.HasOne(d => d.IdEmisorNavigation).WithMany(p => p.ChatMensajeIdEmisorNavigations)
                .HasForeignKey(d => d.IdEmisor)
                .HasConstraintName("FK__ChatMensa__Id_Em__38EE7070");

            entity.HasOne(d => d.IdReceptorNavigation).WithMany(p => p.ChatMensajeIdReceptorNavigations)
                .HasForeignKey(d => d.IdReceptor)
                .HasConstraintName("FK__ChatMensa__Id_Re__39E294A9");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__FCE0399280D8277B");

            entity.ToTable("Cliente");

            entity.Property(e => e.IdCliente).HasColumnName("Id_cliente");
            entity.Property(e => e.Ciudad).HasMaxLength(255);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.Nombre).HasMaxLength(255);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Cliente__Id_empr__40C49C62");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__Compra__957E332F6195B19F");

            entity.ToTable("Compra");

            entity.Property(e => e.IdCompra).HasColumnName("Id_compra");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaAprobado)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_aprobado");
            entity.Property(e => e.FechaCompra)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_compra");
            entity.Property(e => e.FechaEntrega)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_entrega");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdSolicitud).HasColumnName("Id_solicitud");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.LinkOc).HasColumnName("LinkOC");
            entity.Property(e => e.LinkOc1).HasColumnName("LinkOC1");
            entity.Property(e => e.LinkOc2).HasColumnName("LinkOC2");
            entity.Property(e => e.ObservacionesBodega).HasColumnName("Observaciones_bodega");
            entity.Property(e => e.ObservacionesLogistica).HasColumnName("Observaciones_logistica");
            entity.Property(e => e.ObservacionesPago).HasColumnName("Observaciones_pago");
            entity.Property(e => e.OrdenCompra)
                .HasMaxLength(100)
                .HasColumnName("Orden_compra");
            entity.Property(e => e.OrdenCompra1)
                .HasMaxLength(100)
                .HasColumnName("Orden_compra1");
            entity.Property(e => e.OrdenCompra2)
                .HasMaxLength(100)
                .HasColumnName("Orden_compra2");
            entity.Property(e => e.Total).HasColumnType("numeric(12, 2)");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Compra__Id_empre__4B422AD5");

            entity.HasOne(d => d.IdSolicitudNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdSolicitud)
                .HasConstraintName("FK__Compra__Id_solic__693CA210");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Compra__Id_usuar__6A30C649");
        });

        modelBuilder.Entity<CompraDetalle>(entity =>
        {
            entity.HasKey(e => e.IdCompraDetalle).HasName("PK__Compra_D__A71C2D479B2A4BEF");

            entity.ToTable("Compra_Detalle");

            entity.Property(e => e.IdCompraDetalle).HasColumnName("Id_compra_detalle");
            entity.Property(e => e.Ahorro).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Ahorro1).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Cantidad).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Cantidad1).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.IdCompra).HasColumnName("Id_compra");
            entity.Property(e => e.IdSolicitudDetalle).HasColumnName("Id_solicitud_detalle");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Precio_unitario");
            entity.Property(e => e.PrecioUnitario1)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Precio_unitario1");
            entity.Property(e => e.ProveedorSugerido)
                .HasMaxLength(150)
                .HasColumnName("Proveedor_sugerido");
            entity.Property(e => e.ProveedorSugerido1)
                .HasMaxLength(150)
                .HasColumnName("Proveedor_sugerido1");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__Compra_De__Id_co__6D0D32F4");

            entity.HasOne(d => d.IdSolicitudDetalleNavigation).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.IdSolicitudDetalle)
                .HasConstraintName("FK__Compra_De__Id_so__6E01572D");
        });

        modelBuilder.Entity<Conductor>(entity =>
        {
            entity.HasKey(e => e.IdConductor).HasName("PK__Conducto__782821D840C95FBF");

            entity.ToTable("Conductor");

            entity.Property(e => e.IdConductor).HasColumnName("Id_conductor");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PlacaVehiculo)
                .HasMaxLength(50)
                .HasColumnName("Placa_vehiculo");
        });

        modelBuilder.Entity<Devolucion>(entity =>
        {
            entity.HasKey(e => e.IdDevolucion).HasName("PK__Devoluci__D3EFB726C9A225A7");

            entity.ToTable("Devolucion");

            entity.Property(e => e.IdDevolucion).HasColumnName("Id_devolucion");
            entity.Property(e => e.CausaDevolucion)
                .IsUnicode(false)
                .HasColumnName("Causa_devolucion");
            entity.Property(e => e.Contacto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Empresa)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmpresaOrigen).HasColumnName("Empresa_origen");
            entity.Property(e => e.FacturaNumero)
                .IsUnicode(false)
                .HasColumnName("Factura_numero");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.IdVendedor).HasColumnName("Id_vendedor");
            entity.Property(e => e.Observacion1).IsUnicode(false);
            entity.Property(e => e.Observacion2).IsUnicode(false);
            entity.Property(e => e.Observacion3).IsUnicode(false);
            entity.Property(e => e.Observacion4).IsUnicode(false);
            entity.Property(e => e.Observacion5).IsUnicode(false);
            entity.Property(e => e.Observacion6).IsUnicode(false);
            entity.Property(e => e.RemisionNumero)
                .IsUnicode(false)
                .HasColumnName("Remision_numero");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.EmpresaOrigenNavigation).WithMany(p => p.Devolucions)
                .HasForeignKey(d => d.EmpresaOrigen)
                .HasConstraintName("FK__Devolucio__Empre__236943A5");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Devolucions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Devolucio__Id_us__25518C17");

            entity.HasOne(d => d.IdVendedorNavigation).WithMany(p => p.Devolucions)
                .HasForeignKey(d => d.IdVendedor)
                .HasConstraintName("FK__Devolucio__Id_ve__245D67DE");
        });

        modelBuilder.Entity<DevolucionDetalle>(entity =>
        {
            entity.HasKey(e => e.IdDevolucionDetalle).HasName("PK__Devoluci__26ED432E7B974C07");

            entity.ToTable("Devolucion_detalle");

            entity.Property(e => e.IdDevolucionDetalle).HasColumnName("Id_devolucion_detalle");
            entity.Property(e => e.Cantidad).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.IdDevolucion).HasColumnName("Id_devolucion");
            entity.Property(e => e.IdProducto).HasColumnName("Id_producto");
            entity.Property(e => e.NumeroItem)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Numero_item");
            entity.Property(e => e.Valor)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("valor");

            entity.HasOne(d => d.IdDevolucionNavigation).WithMany(p => p.DevolucionDetalles)
                .HasForeignKey(d => d.IdDevolucion)
                .HasConstraintName("FK__Devolucio__Id_de__282DF8C2");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DevolucionDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__Devolucio__Id_pr__29221CFB");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("PK__Empresa__AF698F54F0F4876A");

            entity.ToTable("Empresa");

            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<EstadoEntrega>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado_E__AD3E5E1BAB5DD8DF");

            entity.ToTable("Estado_Entrega");

            entity.Property(e => e.IdEstado)
                .ValueGeneratedNever()
                .HasColumnName("Id_estado");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<MInventario>(entity =>
        {
            entity.HasKey(e => e.IdArticulo).HasName("PK__M_Invent__0F7274E9FF0542A9");

            entity.ToTable("M_Inventario");

            entity.Property(e => e.IdArticulo).HasColumnName("Id_articulo");
            entity.Property(e => e.CantidadDisponible)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Cantidad_disponible");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Ubicacion).HasMaxLength(100);
        });

        modelBuilder.Entity<MPrestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamo).HasName("PK__M_Presta__7AFBEF26F3673840");

            entity.ToTable("M_Prestamos");

            entity.Property(e => e.IdPrestamo).HasColumnName("Id_prestamo");
            entity.Property(e => e.Cantidad).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaDevolucion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_devolucion");
            entity.Property(e => e.FechaPrestamo)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_prestamo");
            entity.Property(e => e.IdArticulo).HasColumnName("Id_articulo");
            entity.Property(e => e.IdTipoPrestamo).HasColumnName("Id_tipo_prestamo");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");

            entity.HasOne(d => d.IdArticuloNavigation).WithMany(p => p.MPrestamos)
                .HasForeignKey(d => d.IdArticulo)
                .HasConstraintName("FK__M_Prestam__Id_ar__43D61337");

            entity.HasOne(d => d.IdTipoPrestamoNavigation).WithMany(p => p.MPrestamos)
                .HasForeignKey(d => d.IdTipoPrestamo)
                .HasConstraintName("FK__M_Prestam__Id_ti__44CA3770");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MPrestamos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__M_Prestam__Id_us__42E1EEFE");
        });

        modelBuilder.Entity<MRequerimiento>(entity =>
        {
            entity.HasKey(e => e.IdRequerimientos).HasName("PK__M_Requer__A6E3E174816065E4");

            entity.ToTable("M_Requerimientos");

            entity.Property(e => e.IdRequerimientos).HasColumnName("Id_requerimientos");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaRequerimiento)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_requerimiento");
            entity.Property(e => e.IdAsignado).HasColumnName("Id_asignado");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdObservador).HasColumnName("Id_observador");
            entity.Property(e => e.IdTipoRequerimiento).HasColumnName("Id_tipo_requerimiento");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.IdUsuariofinal).HasColumnName("Id_usuariofinal");
            entity.Property(e => e.Prioridad).HasMaxLength(50);

            entity.HasOne(d => d.IdAsignadoNavigation).WithMany(p => p.MRequerimientoIdAsignadoNavigations)
                .HasForeignKey(d => d.IdAsignado)
                .HasConstraintName("FK__M_Requeri__Id_as__725BF7F6");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.MRequerimientos)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__M_Requeri__Id_em__74444068");

            entity.HasOne(d => d.IdObservadorNavigation).WithMany(p => p.MRequerimientoIdObservadorNavigations)
                .HasForeignKey(d => d.IdObservador)
                .HasConstraintName("FK__M_Requeri__Id_ob__7167D3BD");

            entity.HasOne(d => d.IdTipoRequerimientoNavigation).WithMany(p => p.MRequerimientos)
                .HasForeignKey(d => d.IdTipoRequerimiento)
                .HasConstraintName("FK__M_Requeri__Id_ti__367C1819");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MRequerimientoIdUsuarioNavigations)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__M_Requeri__Id_us__37703C52");

            entity.HasOne(d => d.IdUsuariofinalNavigation).WithMany(p => p.MRequerimientoIdUsuariofinalNavigations)
                .HasForeignKey(d => d.IdUsuariofinal)
                .HasConstraintName("FK__M_Requeri__Id_us__73501C2F");
        });

        modelBuilder.Entity<MRequerimientosEstado>(entity =>
        {
            entity.HasKey(e => e.MRequerimientosEstado1).HasName("PK__M_Requer__E5257769BBC7465E");

            entity.ToTable("M_Requerimientos_estado");

            entity.Property(e => e.MRequerimientosEstado1).HasColumnName("M_Requerimientos_estado");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaRequerimientoEstado)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_requerimiento_estado");
            entity.Property(e => e.IdRequerimiento).HasColumnName("Id_requerimiento");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Prioridad).HasMaxLength(50);

            entity.HasOne(d => d.IdRequerimientoNavigation).WithMany(p => p.MRequerimientosEstados)
                .HasForeignKey(d => d.IdRequerimiento)
                .HasConstraintName("FK__M_Requeri__Id_re__7DCDAAA2");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MRequerimientosEstados)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__M_Requeri__Id_us__79FD19BE");
        });

        modelBuilder.Entity<MSeguimientoPrestamo>(entity =>
        {
            entity.HasKey(e => e.IdSeguimientoPrestamo).HasName("PK__M_Seguim__4C5CCB9E0B997A72");

            entity.ToTable("M_Seguimiento_prestamos");

            entity.Property(e => e.IdSeguimientoPrestamo).HasColumnName("Id_seguimiento_prestamo");
            entity.Property(e => e.CantidadDevuelta)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Cantidad_devuelta");
            entity.Property(e => e.FechaDevolucion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_devolucion");
            entity.Property(e => e.IdPrestamo).HasColumnName("Id_prestamo");

            entity.HasOne(d => d.IdPrestamoNavigation).WithMany(p => p.MSeguimientoPrestamos)
                .HasForeignKey(d => d.IdPrestamo)
                .HasConstraintName("FK__M_Seguimi__Id_pr__47A6A41B");
        });

        modelBuilder.Entity<MSeguimientoRequerimiento>(entity =>
        {
            entity.HasKey(e => e.IdSeguimientoRequerimientos).HasName("PK__M_seguim__65D12DE0EECD8D99");

            entity.ToTable("M_seguimiento_requerimientos");

            entity.Property(e => e.IdSeguimientoRequerimientos).HasColumnName("Id_seguimiento_requerimientos");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdRequerimientos).HasColumnName("Id_requerimientos");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Prioridad).HasMaxLength(50);

            entity.HasOne(d => d.IdRequerimientosNavigation).WithMany(p => p.MSeguimientoRequerimientos)
                .HasForeignKey(d => d.IdRequerimientos)
                .HasConstraintName("FK__M_seguimi__Id_re__3A4CA8FD");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MSeguimientoRequerimientos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__M_seguimi__Id_us__3B40CD36");
        });

        modelBuilder.Entity<MTipoPrestamo>(entity =>
        {
            entity.HasKey(e => e.IdTipoPrestamo).HasName("PK__M_Tipo_p__6D8217DE24800F04");

            entity.ToTable("M_Tipo_prestamo");

            entity.Property(e => e.IdTipoPrestamo).HasColumnName("Id_tipo_prestamo");
            entity.Property(e => e.IdArea).HasColumnName("Id_area");
            entity.Property(e => e.Nombre).HasMaxLength(150);

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.MTipoPrestamos)
                .HasForeignKey(d => d.IdArea)
                .HasConstraintName("FK__M_Tipo_pr__Id_ar__40058253");
        });

        modelBuilder.Entity<MTipoRequerimiento>(entity =>
        {
            entity.HasKey(e => e.IdTipoRequerimiento).HasName("PK__M_Tipo_r__9A79455CD03D6467");

            entity.ToTable("M_Tipo_requerimiento");

            entity.Property(e => e.IdTipoRequerimiento).HasColumnName("Id_tipo_requerimiento");
            entity.Property(e => e.IdArea).HasColumnName("Id_area");
            entity.Property(e => e.Nombre).HasMaxLength(50);

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.MTipoRequerimientos)
                .HasForeignKey(d => d.IdArea)
                .HasConstraintName("FK__M_Tipo_re__Id_ar__339FAB6E");
        });

        modelBuilder.Entity<Novedad>(entity =>
        {
            entity.HasKey(e => e.IdNovedad).HasName("PK__Novedad__D9E5E11F89E9514F");

            entity.ToTable("Novedad");

            entity.Property(e => e.IdNovedad).HasColumnName("Id_novedad");
            entity.Property(e => e.CiudadBarrio)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Ciudad_barrio");
            entity.Property(e => e.Consecutivo)
                .HasMaxLength(17)
                .HasComputedColumnSql("('SGC-LOG-NOV-'+right('00000'+CONVERT([nvarchar](10),[Id_novedad]),(5)))", true);
            entity.Property(e => e.Contacto)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Empresa)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstadoLogistica)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Estado_Logistica");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaSalida).HasColumnName("Fecha_salida");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Observaciones).IsUnicode(false);
            entity.Property(e => e.Observaciones1).IsUnicode(false);
            entity.Property(e => e.Observaciones2).IsUnicode(false);
            entity.Property(e => e.Observaciones3).IsUnicode(false);
            entity.Property(e => e.ObservacionesLogistica)
                .IsUnicode(false)
                .HasColumnName("Observaciones_Logistica");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoNovedad)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tipo_novedad");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Novedads)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Novedad__Id_empr__6319B466");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Novedads)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Novedad__Id_usua__151B244E");
        });

        modelBuilder.Entity<NovedadComercial>(entity =>
        {
            entity.HasKey(e => e.IdNovedadComercial).HasName("PK__Novedad___012BC3414FBA89FF");

            entity.ToTable("Novedad_comercial");

            entity.Property(e => e.IdNovedadComercial).HasColumnName("Id_novedad_comercial");
            entity.Property(e => e.Consecutivo)
                .HasMaxLength(19)
                .HasComputedColumnSql("('SGC-LOG-NOVCM-'+right('00000'+CONVERT([nvarchar](10),[Id_novedad_comercial]),(5)))", true);
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Empresa)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstadoLogistica)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Estado_Logistica");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaSalida).HasColumnName("Fecha_salida");
            entity.Property(e => e.IdCliente).HasColumnName("Id_cliente");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.NumeroDocumento).HasColumnName("Numero_Documento");
            entity.Property(e => e.Observaciones).IsUnicode(false);
            entity.Property(e => e.Observaciones1).IsUnicode(false);
            entity.Property(e => e.Observaciones2).IsUnicode(false);
            entity.Property(e => e.Observaciones3).IsUnicode(false);
            entity.Property(e => e.ObservacionesLogistica)
                .IsUnicode(false)
                .HasColumnName("Observaciones_Logistica");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tipo_documento");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tipo_pago");
            entity.Property(e => e.TipoServicio)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tipo_servicio");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.NovedadComercials)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Novedad_c__Id_cl__0D7A0286");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.NovedadComercials)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Novedad_c__Id_em__61316BF4");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.NovedadComercials)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Novedad_c__Id_us__0E6E26BF");
        });

        modelBuilder.Entity<NovedadCompra>(entity =>
        {
            entity.HasKey(e => e.IdNovedadCompras).HasName("PK__Novedad___541FAB546B5EC842");

            entity.ToTable("Novedad_compras");

            entity.Property(e => e.IdNovedadCompras).HasColumnName("Id_novedad_compras");
            entity.Property(e => e.CiudadBarrio)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Ciudad_barrio");
            entity.Property(e => e.Consecutivo)
                .HasMaxLength(18)
                .HasComputedColumnSql("('SGC-LOG-NOVC-'+right('00000'+CONVERT([nvarchar](10),[Id_novedad_compras]),(5)))", true);
            entity.Property(e => e.Contacto)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Empresa)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstadoLogistica)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Estado_Logistica");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaSalida).HasColumnName("Fecha_salida");
            entity.Property(e => e.IdCompra).HasColumnName("Id_compra");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Observaciones).IsUnicode(false);
            entity.Property(e => e.Observaciones1).IsUnicode(false);
            entity.Property(e => e.Observaciones2).IsUnicode(false);
            entity.Property(e => e.Observaciones3).IsUnicode(false);
            entity.Property(e => e.ObservacionesLogistica)
                .IsUnicode(false)
                .HasColumnName("Observaciones_Logistica");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoNovedad)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tipo_novedad");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.NovedadCompras)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__Novedad_c__Id_co__114A936A");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.NovedadCompras)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Novedad_c__Id_em__6225902D");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.NovedadCompras)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Novedad_c__Id_us__123EB7A3");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPagos).HasName("PK__Pagos__58B89D67BF32E738");

            entity.Property(e => e.IdPagos).HasColumnName("Id_pagos");
            entity.Property(e => e.Estado).HasMaxLength(100);
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Actualizacion");
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.FechaPago)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Pago");
            entity.Property(e => e.IdCompra).HasColumnName("Id_compra");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdRecepcioncontabilidad).HasColumnName("Id_recepcioncontabilidad");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.LinkDocumentos).HasColumnName("Link_documentos");
            entity.Property(e => e.LinkPago).HasColumnName("Link_pago");
            entity.Property(e => e.Observaciones).HasMaxLength(100);
            entity.Property(e => e.TipoPago)
                .HasMaxLength(100)
                .HasColumnName("Tipo_pago");
            entity.Property(e => e.Total).HasColumnType("numeric(12, 2)");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__Pagos__Id_compra__7A672E12");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Pagos__Id_empres__5C6CB6D7");

            entity.HasOne(d => d.IdRecepcioncontabilidadNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdRecepcioncontabilidad)
                .HasConstraintName("FK__Pagos__Id_recepc__5B78929E");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Pagos__Id_usuari__7B5B524B");
        });

        modelBuilder.Entity<PagosCombrobante>(entity =>
        {
            entity.HasKey(e => e.IdPagosCombrobante).HasName("PK__Pagos_co__0D5757C2D026D1E7");

            entity.ToTable("Pagos_combrobante");

            entity.Property(e => e.IdPagosCombrobante).HasColumnName("Id_pagos_combrobante");
            entity.Property(e => e.Campo1).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo2).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo3).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Comprobante).HasMaxLength(250);
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Actualizacion");
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.IdPagos).HasColumnName("Id_pagos");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.LinkDocumentos).HasColumnName("Link_documentos");
            entity.Property(e => e.Valor).HasColumnType("numeric(12, 2)");

            entity.HasOne(d => d.IdPagosNavigation).WithMany(p => p.PagosCombrobantes)
                .HasForeignKey(d => d.IdPagos)
                .HasConstraintName("FK__Pagos_com__Id_pa__5F492382");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.PagosCombrobantes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Pagos_com__Id_us__603D47BB");
        });

        modelBuilder.Entity<PagosDetalle>(entity =>
        {
            entity.HasKey(e => e.IdPagosDetalle).HasName("PK__Pagos_de__B506152BDA7C7CE2");

            entity.ToTable("Pagos_detalle");

            entity.Property(e => e.IdPagosDetalle).HasColumnName("Id_pagos_detalle");
            entity.Property(e => e.Campo1).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo2).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo3).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Comprobante).HasMaxLength(250);
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Actualizacion");
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.IdPagos).HasColumnName("Id_pagos");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.LinkDocumentos).HasColumnName("Link_documentos");
            entity.Property(e => e.Valor).HasColumnType("numeric(12, 2)");

            entity.HasOne(d => d.IdPagosNavigation).WithMany(p => p.PagosDetalles)
                .HasForeignKey(d => d.IdPagos)
                .HasConstraintName("FK__Pagos_det__Id_pa__7E37BEF6");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.PagosDetalles)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Pagos_det__Id_us__7F2BE32F");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__1D8EFF016B75FDFE");

            entity.ToTable("Producto");

            entity.Property(e => e.IdProducto).HasColumnName("Id_producto");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Empresa).HasMaxLength(50);
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.Precio).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Proveedor).HasMaxLength(255);
            entity.Property(e => e.Referencia).HasMaxLength(100);
            entity.Property(e => e.UltimoIngreso)
                .HasColumnType("datetime")
                .HasColumnName("Ultimo_ingreso");
            entity.Property(e => e.Unidad).HasMaxLength(50);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Producto__Id_emp__42ACE4D4");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__6704E5A86679862B");

            entity.ToTable("Proveedor");

            entity.Property(e => e.IdProveedor).HasColumnName("Id_proveedor");
            entity.Property(e => e.Ciudad).HasMaxLength(255);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.Nombre).HasMaxLength(255);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Proveedors)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Proveedor__Id_em__41B8C09B");
        });

        modelBuilder.Entity<RecepcionContabilidad>(entity =>
        {
            entity.HasKey(e => e.IdRecepcioncontabilidad).HasName("PK__Recepcio__29A5F6FDE6DE06C3");

            entity.ToTable("RecepcionContabilidad");

            entity.Property(e => e.IdRecepcioncontabilidad).HasColumnName("Id_recepcioncontabilidad");
            entity.Property(e => e.ArchivadoPor).HasColumnName("Archivado_por");
            entity.Property(e => e.DocumentosCompletos).HasColumnName("Documentos_completos");
            entity.Property(e => e.Estado).HasMaxLength(30);
            entity.Property(e => e.FechaArchivo)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_archivo");
            entity.Property(e => e.IdRecepcionDocumental).HasColumnName("Id_recepcionDocumental");

            entity.HasOne(d => d.ArchivadoPorNavigation).WithMany(p => p.RecepcionContabilidads)
                .HasForeignKey(d => d.ArchivadoPor)
                .HasConstraintName("FK__Recepcion__Archi__56B3DD81");

            entity.HasOne(d => d.IdRecepcionDocumentalNavigation).WithMany(p => p.RecepcionContabilidads)
                .HasForeignKey(d => d.IdRecepcionDocumental)
                .HasConstraintName("FK__Recepcion__Id_re__55BFB948");
        });

        modelBuilder.Entity<RecepcionDetalle>(entity =>
        {
            entity.HasKey(e => e.IdRecepcionDetalle).HasName("PK__Recepcio__39C7FB249FC1BF22");

            entity.ToTable("Recepcion_Detalle");

            entity.Property(e => e.IdRecepcionDetalle).HasColumnName("Id_recepcion_detalle");
            entity.Property(e => e.Campo1)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("campo1");
            entity.Property(e => e.Campo2)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("campo2");
            entity.Property(e => e.Campo3)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("campo3");
            entity.Property(e => e.CantidadRecibida)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Cantidad_recibida");
            entity.Property(e => e.EstadoCalidad)
                .HasMaxLength(100)
                .HasColumnName("Estado_calidad");
            entity.Property(e => e.EstadoProducto)
                .HasMaxLength(100)
                .HasColumnName("Estado_producto");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaRecepcion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_recepcion");
            entity.Property(e => e.IdRecepcion).HasColumnName("Id_recepcion");
            entity.Property(e => e.IdSolicitudDetalle).HasColumnName("Id_solicitud_detalle");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Muestra).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.ObservacionesCalidad).HasColumnName("Observaciones_calidad");

            entity.HasOne(d => d.IdRecepcionNavigation).WithMany(p => p.RecepcionDetalles)
                .HasForeignKey(d => d.IdRecepcion)
                .HasConstraintName("FK__Recepcion__Id_re__74AE54BC");

            entity.HasOne(d => d.IdSolicitudDetalleNavigation).WithMany(p => p.RecepcionDetalles)
                .HasForeignKey(d => d.IdSolicitudDetalle)
                .HasConstraintName("FK__Recepcion__Id_so__75A278F5");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RecepcionDetalles)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Recepcion__Id_us__4E1E9780");
        });

        modelBuilder.Entity<RecepcionDetalleDqr>(entity =>
        {
            entity.HasKey(e => e.IdDqr).HasName("PK__Recepcio__5EA13D97F9800CE6");

            entity.ToTable("Recepcion_detalle_DQR");

            entity.Property(e => e.IdDqr).HasColumnName("Id_DQR");
            entity.Property(e => e.Campo)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("campo");
            entity.Property(e => e.Campo1)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("campo1");
            entity.Property(e => e.Campo2)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("campo2");
            entity.Property(e => e.Campo3)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("campo3");
            entity.Property(e => e.Campo4).HasColumnName("campo4");
            entity.Property(e => e.Campo5).HasColumnName("campo5");
            entity.Property(e => e.Campo6).HasColumnName("campo6");
            entity.Property(e => e.Campo7).HasColumnName("campo7");
            entity.Property(e => e.CantRechazada)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Cant_rechazada");
            entity.Property(e => e.Cantidad).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Consecutivo)
                .HasMaxLength(17)
                .HasComputedColumnSql("('SGC-CLD-DQR-'+right('00000'+CONVERT([nvarchar](10),[Id_DQR]),(5)))", true);
            entity.Property(e => e.DescripcionGeneral).HasColumnName("Descripcion_general");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Fecha1).HasColumnType("datetime");
            entity.Property(e => e.Fecha2).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_cierre");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_creacion");
            entity.Property(e => e.IdRecepcionDetalle).HasColumnName("Id_recepcion_detalle");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.NaturalezaDqr).HasColumnName("NaturalezaDQR");
            entity.Property(e => e.TipoCantidad).HasColumnName("Tipo_cantidad");
            entity.Property(e => e.TipoCantidadRechazada).HasColumnName("Tipo_cantidad_rechazada");

            entity.HasOne(d => d.IdRecepcionDetalleNavigation).WithMany(p => p.RecepcionDetalleDqrs)
                .HasForeignKey(d => d.IdRecepcionDetalle)
                .HasConstraintName("FK__Recepcion__Conse__756D6ECB");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RecepcionDetalleDqrs)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Recepcion__Id_us__76619304");
        });

        modelBuilder.Entity<RecepcionDocumental>(entity =>
        {
            entity.HasKey(e => e.IdRecepcionDocumental).HasName("PK__Recepcio__75B105DDEA498B36");

            entity.ToTable("RecepcionDocumental");

            entity.Property(e => e.IdRecepcionDocumental).HasColumnName("Id_recepcionDocumental");
            entity.Property(e => e.EntregaCertificadoCalidad).HasColumnName("Entrega_certificado_calidad");
            entity.Property(e => e.EntregaOrdenCompra).HasColumnName("Entrega_orden_compra");
            entity.Property(e => e.EntregoCertificadoOrigen).HasColumnName("Entrego_certificado_origen");
            entity.Property(e => e.EntregoFactura).HasColumnName("Entrego_factura");
            entity.Property(e => e.EntregoPlano).HasColumnName("Entrego_plano_");
            entity.Property(e => e.EntregoRemision).HasColumnName("Entrego_remision");
            entity.Property(e => e.EstadoDocumentacion)
                .HasMaxLength(30)
                .HasColumnName("Estado_documentacion");
            entity.Property(e => e.FechaDocumentacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_documentacion");
            entity.Property(e => e.FechaEntrega)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_entrega");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_modificacion");
            entity.Property(e => e.FechaRecepcion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_recepcion");
            entity.Property(e => e.IdRecepcion).HasColumnName("Id_recepcion");
            entity.Property(e => e.IdUsuarioDocumentacion).HasColumnName("Id_usuario_documentacion");
            entity.Property(e => e.IdUsuarioRecepcion).HasColumnName("Id_usuario_recepcion");
            entity.Property(e => e.NumeroFactura).HasColumnName("Numero_factura");
            entity.Property(e => e.NumeroRemision).HasColumnName("Numero_remision");

            entity.HasOne(d => d.IdRecepcionNavigation).WithMany(p => p.RecepcionDocumentals)
                .HasForeignKey(d => d.IdRecepcion)
                .HasConstraintName("FK__Recepcion__Id_re__50FB042B");

            entity.HasOne(d => d.IdUsuarioDocumentacionNavigation).WithMany(p => p.RecepcionDocumentalIdUsuarioDocumentacionNavigations)
                .HasForeignKey(d => d.IdUsuarioDocumentacion)
                .HasConstraintName("FK__Recepcion__Id_us__52E34C9D");

            entity.HasOne(d => d.IdUsuarioRecepcionNavigation).WithMany(p => p.RecepcionDocumentalIdUsuarioRecepcionNavigations)
                .HasForeignKey(d => d.IdUsuarioRecepcion)
                .HasConstraintName("FK__Recepcion__Id_us__51EF2864");
        });

        modelBuilder.Entity<RecepcionMercancium>(entity =>
        {
            entity.HasKey(e => e.IdRecepcion).HasName("PK__Recepcio__9B60625D670EBE41");

            entity.Property(e => e.IdRecepcion).HasColumnName("Id_recepcion");
            entity.Property(e => e.DocumentoRecibido).HasColumnName("Documento_recibido");
            entity.Property(e => e.Estado).HasMaxLength(30);
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaRecepcion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_recepcion");
            entity.Property(e => e.IdCompra).HasColumnName("Id_compra");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.IdUsuarioModificacion).HasColumnName("Id_usuario_modificacion");
            entity.Property(e => e.NumeroDocumento).HasColumnName("Numero_documento");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.RecepcionMercancia)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("FK__Recepcion__Id_co__70DDC3D8");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.RecepcionMercancia)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Recepcion__Id_em__4D2A7347");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RecepcionMercanciumIdUsuarioNavigations)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Recepcion__Id_us__71D1E811");

            entity.HasOne(d => d.IdUsuarioModificacionNavigation).WithMany(p => p.RecepcionMercanciumIdUsuarioModificacionNavigations)
                .HasForeignKey(d => d.IdUsuarioModificacion)
                .HasConstraintName("FK__Recepcion__Id_us__4C364F0E");
        });

        modelBuilder.Entity<Recepcionbodega>(entity =>
        {
            entity.HasKey(e => e.IdRecepcionbodega).HasName("PK__Recepcio__E41418C775277496");

            entity.ToTable("Recepcionbodega");

            entity.Property(e => e.IdRecepcionbodega).HasColumnName("Id_recepcionbodega");
            entity.Property(e => e.DocumentosCompletos).HasColumnName("Documentos_completos");
            entity.Property(e => e.Estado).HasMaxLength(30);
            entity.Property(e => e.FechaRecibido)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_recibido");
            entity.Property(e => e.IdRecepcion).HasColumnName("Id_recepcion");
            entity.Property(e => e.RecibidoPor).HasColumnName("Recibido_por");

            entity.HasOne(d => d.IdRecepcionNavigation).WithMany(p => p.Recepcionbodegas)
                .HasForeignKey(d => d.IdRecepcion)
                .HasConstraintName("FK__Recepcion__Id_re__59904A2C");

            entity.HasOne(d => d.RecibidoPorNavigation).WithMany(p => p.Recepcionbodegas)
                .HasForeignKey(d => d.RecibidoPor)
                .HasConstraintName("FK__Recepcion__Recib__5A846E65");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2D95A894FF3BA970");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("Id_rol");
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<Rutero>(entity =>
        {
            entity.HasKey(e => e.IdRutero).HasName("PK__Rutero__421C552DD5743077");

            entity.ToTable("Rutero");

            entity.Property(e => e.IdRutero).HasColumnName("Id_rutero");
            entity.Property(e => e.Estado).IsUnicode(false);
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_cierre");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_creacion");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.IdVendedor).HasColumnName("Id_vendedor");
            entity.Property(e => e.Observaciones).IsUnicode(false);
            entity.Property(e => e.Vigente)
                .IsUnicode(false)
                .HasColumnName("vigente");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Ruteros)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Rutero__Id_empre__6F7F8B4B");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Ruteros)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Rutero__Id_usuar__7A3223E8");

            entity.HasOne(d => d.IdVendedorNavigation).WithMany(p => p.Ruteros)
                .HasForeignKey(d => d.IdVendedor)
                .HasConstraintName("FK__Rutero__Id_vende__793DFFAF");
        });

        modelBuilder.Entity<RuteroDetalle>(entity =>
        {
            entity.HasKey(e => e.IdRuteroDetalle).HasName("PK__Rutero_d__19A89D3ADE35C76A");

            entity.ToTable("Rutero_detalle");

            entity.Property(e => e.IdRuteroDetalle).HasColumnName("Id_Rutero_detalle");
            entity.Property(e => e.Acumulado).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Asesor).IsUnicode(false);
            entity.Property(e => e.Campo).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo1).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo2).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.CantAcumulada)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Cant_acumulada");
            entity.Property(e => e.CantProyeccion)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Cant_proyeccion");
            entity.Property(e => e.Cargo).IsUnicode(false);
            entity.Property(e => e.Compromiso).IsUnicode(false);
            entity.Property(e => e.Contacto).IsUnicode(false);
            entity.Property(e => e.Cumplimiento)
                .HasComputedColumnSql("(case when [Ventas_proyeccion]>(0) then [Acumulado]/[Ventas_proyeccion] else (0) end)", true)
                .HasColumnType("numeric(27, 15)");
            entity.Property(e => e.CumplimientoCant)
                .HasComputedColumnSql("(case when [Cant_proyeccion]>(0) then CONVERT([float],[Cant_acumulada])/[Cant_proyeccion] else (0) end)", false)
                .HasColumnName("Cumplimiento_cant");
            entity.Property(e => e.FechaAsignacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_asignacion");
            entity.Property(e => e.Gestion).IsUnicode(false);
            entity.Property(e => e.IdCliente).HasColumnName("Id_cliente");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdRutero).HasColumnName("Id_rutero");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Observacion).IsUnicode(false);
            entity.Property(e => e.Observacion1).IsUnicode(false);
            entity.Property(e => e.Observacion2).IsUnicode(false);
            entity.Property(e => e.Observacion3).IsUnicode(false);
            entity.Property(e => e.Quienes).IsUnicode(false);
            entity.Property(e => e.Telefono).IsUnicode(false);
            entity.Property(e => e.VentasProyeccion)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Ventas_proyeccion");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.RuteroDetalles)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Rutero_de__Cumpl__7D0E9093");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.RuteroDetalles)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Rutero_de__Id_em__7073AF84");

            entity.HasOne(d => d.IdRuteroNavigation).WithMany(p => p.RuteroDetalles)
                .HasForeignKey(d => d.IdRutero)
                .HasConstraintName("FK_RuteroDetalle_Rutero");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RuteroDetalles)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Rutero_de__Id_us__7E02B4CC");
        });

        modelBuilder.Entity<RuteroRevision>(entity =>
        {
            entity.HasKey(e => e.IdRuteroRevision).HasName("PK__Rutero_r__1E92CB8DFF11150E");

            entity.ToTable("Rutero_revision");

            entity.Property(e => e.IdRuteroRevision).HasColumnName("Id_Rutero_revision");
            entity.Property(e => e.Acuerdo).IsUnicode(false);
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Detalle).IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.IdRuteroDetalle).HasColumnName("Id_Rutero_detalle");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Motivo).IsUnicode(false);
            entity.Property(e => e.Observacion).IsUnicode(false);

            entity.HasOne(d => d.IdRuteroDetalleNavigation).WithMany(p => p.RuteroRevisions)
                .HasForeignKey(d => d.IdRuteroDetalle)
                .HasConstraintName("FK__Rutero_re__Id_Ru__0A688BB1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RuteroRevisions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Rutero_re__Id_us__0B5CAFEA");
        });

        modelBuilder.Entity<RuteroVenta>(entity =>
        {
            entity.HasKey(e => e.IdRuteroVentas).HasName("PK__Rutero_V__C38C49F4999D107D");

            entity.ToTable("Rutero_Ventas", tb => tb.HasTrigger("trg_UpdateAcumulado"));

            entity.Property(e => e.IdRuteroVentas).HasColumnName("Id_Rutero_Ventas");
            entity.Property(e => e.Campo).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo1).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo2).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.CantidadVendida)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Cantidad_vendida");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.IdRuteroDetalle).HasColumnName("Id_Rutero_detalle");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Mes).IsUnicode(false);
            entity.Property(e => e.NumeroVentas)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Numero_ventas");
            entity.Property(e => e.Observacion).IsUnicode(false);
            entity.Property(e => e.Observacion1).IsUnicode(false);
            entity.Property(e => e.Observacion2).IsUnicode(false);
            entity.Property(e => e.Observacion3).IsUnicode(false);
            entity.Property(e => e.TotalVentas)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Total_ventas");

            entity.HasOne(d => d.IdRuteroDetalleNavigation).WithMany(p => p.RuteroVenta)
                .HasForeignKey(d => d.IdRuteroDetalle)
                .HasConstraintName("FK__Rutero_Ve__Id_Ru__00DF2177");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RuteroVenta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Rutero_Ve__Id_us__01D345B0");
        });

        modelBuilder.Entity<RuteroVisita>(entity =>
        {
            entity.HasKey(e => e.IdRuteroVisitas).HasName("PK__Rutero_v__EE10401687DA242C");

            entity.ToTable("Rutero_visitas", tb => tb.HasTrigger("trg_UpdateVisitas"));

            entity.Property(e => e.IdRuteroVisitas).HasColumnName("Id_Rutero_visitas");
            entity.Property(e => e.Acuerdo).IsUnicode(false);
            entity.Property(e => e.Campo).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo1).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo2).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Detalle).IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.IdRuteroDetalle).HasColumnName("Id_Rutero_detalle");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Motivo).IsUnicode(false);
            entity.Property(e => e.Observacion).IsUnicode(false);
            entity.Property(e => e.Observacion1).IsUnicode(false);
            entity.Property(e => e.Observacion2).IsUnicode(false);
            entity.Property(e => e.Observacion3).IsUnicode(false);

            entity.HasOne(d => d.IdRuteroDetalleNavigation).WithMany(p => p.RuteroVisita)
                .HasForeignKey(d => d.IdRuteroDetalle)
                .HasConstraintName("FK__Rutero_vi__Id_Ru__05A3D694");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RuteroVisita)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Rutero_vi__Id_us__0697FACD");
        });

        modelBuilder.Entity<Rutum>(entity =>
        {
            entity.HasKey(e => e.IdRuta).HasName("PK__Ruta__4485231E159BB1D2");

            entity.Property(e => e.IdRuta).HasColumnName("Id_ruta");
            entity.Property(e => e.Campo1).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo2).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo3).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Causa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstadoLogistica)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Estado_Logistica");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaAsignacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_asignacion");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_cierre");
            entity.Property(e => e.FechaRuta).HasColumnName("Fecha_Ruta");
            entity.Property(e => e.IdConductor).HasColumnName("Id_conductor");
            entity.Property(e => e.IdEstado).HasColumnName("Id_estado");
            entity.Property(e => e.IdNovedadComercial).HasColumnName("Id_novedad_comercial");
            entity.Property(e => e.IdNovedadCompras).HasColumnName("Id_novedad_compras");
            entity.Property(e => e.IdNovedadGeneral).HasColumnName("Id_novedad_general");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.NGuia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("N_guia");
            entity.Property(e => e.Observaciones).IsUnicode(false);
            entity.Property(e => e.ObservacionesLogistica)
                .IsUnicode(false)
                .HasColumnName("Observaciones_Logistica");
            entity.Property(e => e.Recibido)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdConductorNavigation).WithMany(p => p.Ruta)
                .HasForeignKey(d => d.IdConductor)
                .HasConstraintName("FK__Ruta__Id_conduct__1EA48E88");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Ruta)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Ruta__Id_estado__1F98B2C1");

            entity.HasOne(d => d.IdNovedadComercialNavigation).WithMany(p => p.Ruta)
                .HasForeignKey(d => d.IdNovedadComercial)
                .HasConstraintName("FK__Ruta__Id_novedad__1CBC4616");

            entity.HasOne(d => d.IdNovedadComprasNavigation).WithMany(p => p.Ruta)
                .HasForeignKey(d => d.IdNovedadCompras)
                .HasConstraintName("FK__Ruta__Id_novedad__1BC821DD");

            entity.HasOne(d => d.IdNovedadGeneralNavigation).WithMany(p => p.Ruta)
                .HasForeignKey(d => d.IdNovedadGeneral)
                .HasConstraintName("FK__Ruta__Id_novedad__1DB06A4F");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Ruta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Ruta__Id_usuario__208CD6FA");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.HasKey(e => e.IdSolicitud).HasName("PK__Solicitu__38B3E6E3449A4294");

            entity.ToTable("Solicitud");

            entity.Property(e => e.IdSolicitud).HasColumnName("Id_solicitud");
            entity.Property(e => e.Consecutivo)
                .HasMaxLength(17)
                .HasComputedColumnSql("('SGC-COM-SOL-'+right('00000'+CONVERT([nvarchar](10),[Id_solicitud]),(5)))", true);
            entity.Property(e => e.ConsecutivoPorEmpresa).HasColumnName("Consecutivo_por_empresa");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Cierre");
            entity.Property(e => e.FechaEntrega)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_entrega");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_modificacion");
            entity.Property(e => e.FechaOc)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_OC");
            entity.Property(e => e.FechaRm)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_RM");
            entity.Property(e => e.IdCliente).HasColumnName("Id_cliente");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdProveedor).HasColumnName("Id_proveedor");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.IdUsuarioAprobado).HasColumnName("Id_usuario_aprobado");
            entity.Property(e => e.IdUsuarioAsignado).HasColumnName("Id_usuario_asignado");
            entity.Property(e => e.IdVendedor).HasColumnName("Id_vendedor");
            entity.Property(e => e.Negociacion).HasMaxLength(150);
            entity.Property(e => e.ObservacionesActualizacion).HasColumnName("Observaciones_actualizacion");
            entity.Property(e => e.ProveedorSugerido)
                .HasMaxLength(255)
                .HasColumnName("Proveedor_sugerido");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Solicitud__Conse__5FB337D6");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Solicitud__Id_em__43A1090D");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__Solicitud__Id_pr__60A75C0F");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.SolicitudIdUsuarioNavigations)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Solicitud__Id_us__619B8048");

            entity.HasOne(d => d.IdUsuarioAprobadoNavigation).WithMany(p => p.SolicitudIdUsuarioAprobadoNavigations)
                .HasForeignKey(d => d.IdUsuarioAprobado)
                .HasConstraintName("FK__Solicitud__Id_us__4589517F");

            entity.HasOne(d => d.IdUsuarioAsignadoNavigation).WithMany(p => p.SolicitudIdUsuarioAsignadoNavigations)
                .HasForeignKey(d => d.IdUsuarioAsignado)
                .HasConstraintName("FK__Solicitud__Id_us__44952D46");

            entity.HasOne(d => d.IdVendedorNavigation).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.IdVendedor)
                .HasConstraintName("FK__Solicitud__Id_ve__628FA481");
        });

        modelBuilder.Entity<SolicitudDetalle>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudDetalle).HasName("PK__Solicitu__53032C5502C8E517");

            entity.ToTable("Solicitud_detalle");

            entity.Property(e => e.IdSolicitudDetalle).HasColumnName("Id_solicitud_detalle");
            entity.Property(e => e.Cantidad).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.IdProducto).HasColumnName("Id_producto");
            entity.Property(e => e.IdSolicitud).HasColumnName("Id_solicitud");
            entity.Property(e => e.Negociacion).HasMaxLength(150);
            entity.Property(e => e.ObservacionCompras)
                .HasMaxLength(150)
                .HasColumnName("Observacion_compras");
            entity.Property(e => e.PrecioCosto)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Precio_costo");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Precio_venta");
            entity.Property(e => e.Rentabilidad)
                .HasComputedColumnSql("([Precio_venta]/[Precio_costo]-(1))", false)
                .HasColumnType("numeric(28, 15)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.SolicitudDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__Solicitud__Id_pr__66603565");

            entity.HasOne(d => d.IdSolicitudNavigation).WithMany(p => p.SolicitudDetalles)
                .HasForeignKey(d => d.IdSolicitud)
                .HasConstraintName("FK__Solicitud__Id_so__656C112C");
        });

        modelBuilder.Entity<SolicitudDocumento>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudDocumentos).HasName("PK__Solicitu__FF76CDCE33A39196");

            entity.ToTable("Solicitud_documentos");

            entity.Property(e => e.IdSolicitudDocumentos).HasColumnName("Id_solicitud_documentos");
            entity.Property(e => e.Campo1).HasMaxLength(255);
            entity.Property(e => e.Campo2).HasMaxLength(255);
            entity.Property(e => e.Campo3).HasMaxLength(255);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_actualizacion");
            entity.Property(e => e.IdSolicitud).HasColumnName("Id_solicitud");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Tipo).HasMaxLength(255);

            entity.HasOne(d => d.IdSolicitudNavigation).WithMany(p => p.SolicitudDocumentos)
                .HasForeignKey(d => d.IdSolicitud)
                .HasConstraintName("FK__Solicitud__Id_so__4A4E069C");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.SolicitudDocumentos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Solicitud__Id_us__4959E263");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.IdTarea).HasName("PK__Tareas__A1C9A84DF4F61E92");

            entity.Property(e => e.IdTarea).HasColumnName("Id_Tarea");
            entity.Property(e => e.Campo4).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo5).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo6).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCierre).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaLimite).HasColumnType("datetime");
            entity.Property(e => e.IdArea).HasColumnName("Id_area");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.IdUsuarioAsignada).HasColumnName("Id_usuarioAsignada");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.IdArea)
                .HasConstraintName("FK__Tareas__Id_area__2022C2A6");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Tareas__Id_empre__1F2E9E6D");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TareaIdUsuarioNavigations)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Tareas__Id_usuar__1E3A7A34");

            entity.HasOne(d => d.IdUsuarioAsignadaNavigation).WithMany(p => p.TareaIdUsuarioAsignadaNavigations)
                .HasForeignKey(d => d.IdUsuarioAsignada)
                .HasConstraintName("FK__Tareas__Id_usuar__2116E6DF");
        });

        modelBuilder.Entity<TareasAdjunto>(entity =>
        {
            entity.HasKey(e => e.IdTareasSubAdjuntos).HasName("PK__Tareas_a__A9555385F239F1C2");

            entity.ToTable("Tareas_adjuntos");

            entity.Property(e => e.IdTareasSubAdjuntos).HasColumnName("Id_TareasSub_adjuntos");
            entity.Property(e => e.Campo4).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo5).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo6).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdTarea).HasColumnName("Id_Tarea");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.TareasAdjuntos)
                .HasForeignKey(d => d.IdTarea)
                .HasConstraintName("FK__Tareas_ad__Id_Ta__24E777C3");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TareasAdjuntos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Tareas_ad__Id_us__23F3538A");
        });

        modelBuilder.Entity<TareasAlerta>(entity =>
        {
            entity.HasKey(e => e.IdTareasAlertas).HasName("PK__TareasAl__8AD8A99343FA30DD");

            entity.Property(e => e.IdTareasAlertas).HasColumnName("Id_TareasAlertas");
            entity.Property(e => e.Campo4).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo5).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo6).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.FechaAlerta).HasColumnType("datetime");
            entity.Property(e => e.IdTarea).HasColumnName("Id_Tarea");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Vista).HasDefaultValue(false);

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.TareasAlerta)
                .HasForeignKey(d => d.IdTarea)
                .HasConstraintName("FK__TareasAle__Id_Ta__3429BB53");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TareasAlerta)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TareasAle__Id_us__351DDF8C");
        });

        modelBuilder.Entity<TareasSub>(entity =>
        {
            entity.HasKey(e => e.IdTareasSub).HasName("PK__TareasSu__8733965FE0F965A5");

            entity.ToTable("TareasSub");

            entity.Property(e => e.IdTareasSub).HasColumnName("Id_TareasSub");
            entity.Property(e => e.Campo4).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo5).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo6).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdTarea).HasColumnName("Id_Tarea");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.TareasSubs)
                .HasForeignKey(d => d.IdTarea)
                .HasConstraintName("FK__TareasSub__Id_Ta__28B808A7");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TareasSubs)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TareasSub__Id_us__27C3E46E");
        });

        modelBuilder.Entity<TareasSubAdjunto>(entity =>
        {
            entity.HasKey(e => e.IdTareasSubAdjuntos).HasName("PK__TareasSu__A9555385CC4A0218");

            entity.ToTable("TareasSub_adjuntos");

            entity.Property(e => e.IdTareasSubAdjuntos).HasColumnName("Id_TareasSub_adjuntos");
            entity.Property(e => e.Campo4).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo5).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo6).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdTareasSub).HasColumnName("Id_TareasSub");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");

            entity.HasOne(d => d.IdTareasSubNavigation).WithMany(p => p.TareasSubAdjuntos)
                .HasForeignKey(d => d.IdTareasSub)
                .HasConstraintName("FK__TareasSub__Id_Ta__2B947552");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TareasSubAdjuntos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TareasSub__Id_us__2C88998B");
        });

        modelBuilder.Entity<TareasTrazabilidad>(entity =>
        {
            entity.HasKey(e => e.IdTareasTrazabilidad).HasName("PK__TareasTr__FA5DF7847BF48CCC");

            entity.ToTable("TareasTrazabilidad");

            entity.Property(e => e.IdTareasTrazabilidad).HasColumnName("Id_TareasTrazabilidad");
            entity.Property(e => e.Campo4).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo5).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Campo6).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdTarea).HasColumnName("Id_Tarea");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.TareasTrazabilidads)
                .HasForeignKey(d => d.IdTarea)
                .HasConstraintName("FK__TareasTra__Id_Ta__2F650636");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TareasTrazabilidads)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TareasTra__Id_us__30592A6F");
        });

        modelBuilder.Entity<ThDescargoCaso>(entity =>
        {
            entity.HasKey(e => e.IdDescargoCaso).HasName("PK__TH_Desca__9394DF579C8C93E9");

            entity.ToTable("TH_Descargo_caso");

            entity.Property(e => e.IdDescargoCaso).HasColumnName("Id_Descargo_caso");
            entity.Property(e => e.Campo6).HasColumnType("datetime");
            entity.Property(e => e.Campo8).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.Consecutivo)
                .HasMaxLength(18)
                .HasComputedColumnSql("('SGC-RRHH-DES-'+right('00000'+CONVERT([nvarchar](10),[Id_Descargo_caso]),(5)))", true);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCierre).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.IdSolicitudDes).HasColumnName("Id_solicitudDes");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");

            entity.HasOne(d => d.IdSolicitudDesNavigation).WithMany(p => p.ThDescargoCasos)
                .HasForeignKey(d => d.IdSolicitudDes)
                .HasConstraintName("FK__TH_Descar__Id_so__27F8EE98");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThDescargoCasos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_Descar__Id_us__2704CA5F");
        });

        modelBuilder.Entity<ThDescargoCitacion>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudDesTes).HasName("PK__TH_Desca__F3D77EF4D0914596");

            entity.ToTable("TH_Descargo_citacion");

            entity.Property(e => e.IdSolicitudDesTes).HasColumnName("Id_solicitudDesTes");
            entity.Property(e => e.Campo6).HasColumnType("datetime");
            entity.Property(e => e.Campo8).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCierre).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.IdDescargoCaso).HasColumnName("Id_Descargo_caso");
            entity.Property(e => e.IdTrabajador).HasColumnName("Id_trabajador");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");

            entity.HasOne(d => d.IdDescargoCasoNavigation).WithMany(p => p.ThDescargoCitacions)
                .HasForeignKey(d => d.IdDescargoCaso)
                .HasConstraintName("FK__TH_Descar__Id_De__2CBDA3B5");

            entity.HasOne(d => d.IdTrabajadorNavigation).WithMany(p => p.ThDescargoCitacions)
                .HasForeignKey(d => d.IdTrabajador)
                .HasConstraintName("FK__TH_Descar__Id_tr__2AD55B43");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThDescargoCitacions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_Descar__Id_us__2BC97F7C");
        });

        modelBuilder.Entity<ThDescargosDiligenciar>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudDesTes).HasName("PK__TH_Desca__F3D77EF46762CC7F");

            entity.ToTable("TH_Descargos_diligenciar");

            entity.Property(e => e.IdSolicitudDesTes).HasColumnName("Id_solicitudDesTes");
            entity.Property(e => e.Campo6).HasColumnType("datetime");
            entity.Property(e => e.Campo8).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCierre).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.IdDescargoCaso).HasColumnName("Id_Descargo_caso");
            entity.Property(e => e.IdTrabajador).HasColumnName("Id_trabajador");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");

            entity.HasOne(d => d.IdDescargoCasoNavigation).WithMany(p => p.ThDescargosDiligenciars)
                .HasForeignKey(d => d.IdDescargoCaso)
                .HasConstraintName("FK__TH_Descar__Id_De__318258D2");

            entity.HasOne(d => d.IdTrabajadorNavigation).WithMany(p => p.ThDescargosDiligenciars)
                .HasForeignKey(d => d.IdTrabajador)
                .HasConstraintName("FK__TH_Descar__Id_tr__2F9A1060");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThDescargosDiligenciars)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_Descar__Id_us__308E3499");
        });

        modelBuilder.Entity<ThDescargosProceso>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudDesTes).HasName("PK__TH_Desca__F3D77EF49157D12D");

            entity.ToTable("TH_Descargos_proceso");

            entity.Property(e => e.IdSolicitudDesTes).HasColumnName("Id_solicitudDesTes");
            entity.Property(e => e.Campo6).HasColumnType("datetime");
            entity.Property(e => e.Campo8).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCierre).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.IdDescargoCaso).HasColumnName("Id_Descargo_caso");
            entity.Property(e => e.IdTrabajador).HasColumnName("Id_trabajador");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.JefeInmediato).HasColumnName("Jefe_inmediato");
            entity.Property(e => e.RepTalentohumano).HasColumnName("Rep_talentohumano");

            entity.HasOne(d => d.IdDescargoCasoNavigation).WithMany(p => p.ThDescargosProcesos)
                .HasForeignKey(d => d.IdDescargoCaso)
                .HasConstraintName("FK__TH_Descar__Id_De__36470DEF");

            entity.HasOne(d => d.IdTrabajadorNavigation).WithMany(p => p.ThDescargosProcesos)
                .HasForeignKey(d => d.IdTrabajador)
                .HasConstraintName("FK__TH_Descar__Id_tr__345EC57D");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThDescargosProcesos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_Descar__Id_us__3552E9B6");
        });

        modelBuilder.Entity<ThRequerimiento>(entity =>
        {
            entity.HasKey(e => e.IdRequerimiento).HasName("PK__TH_Reque__306F8FD3D338BCC8");

            entity.ToTable("TH_Requerimiento");

            entity.Property(e => e.IdRequerimiento).HasColumnName("Id_requerimiento");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaRequerimiento)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_requerimiento");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Prioridad).HasMaxLength(50);
            entity.Property(e => e.TipoRequerimiento)
                .HasMaxLength(150)
                .HasColumnName("Tipo_requerimiento");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThRequerimientos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_Requer__Id_us__4A8310C6");
        });

        modelBuilder.Entity<ThRequerimientosDocumento>(entity =>
        {
            entity.HasKey(e => e.IdRequerimientoDocumento).HasName("PK__TH_Reque__A47D041E52E97561");

            entity.ToTable("TH_Requerimientos_documento");

            entity.Property(e => e.IdRequerimientoDocumento).HasColumnName("Id_requerimiento_documento");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaDocumento)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_documento");
            entity.Property(e => e.IdRequerimiento).HasColumnName("Id_requerimiento");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.LinkDocumento).HasColumnName("Link_documento");
            entity.Property(e => e.Prioridad).HasMaxLength(50);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(150)
                .HasColumnName("Tipo_documento");

            entity.HasOne(d => d.IdRequerimientoNavigation).WithMany(p => p.ThRequerimientosDocumentos)
                .HasForeignKey(d => d.IdRequerimiento)
                .HasConstraintName("FK__TH_Requer__Id_re__038683F8");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThRequerimientosDocumentos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_Requer__Id_us__047AA831");
        });

        modelBuilder.Entity<ThSeguimientoRequerimiento>(entity =>
        {
            entity.HasKey(e => e.IdSeguimientoRequerimiento).HasName("PK__TH_segui__76C39DC3E9811F1A");

            entity.ToTable("TH_seguimiento_requerimiento");

            entity.Property(e => e.IdSeguimientoRequerimiento).HasColumnName("Id_seguimiento_requerimiento");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdRequerimiento).HasColumnName("Id_requerimiento");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Prioridad).HasMaxLength(50);

            entity.HasOne(d => d.IdRequerimientoNavigation).WithMany(p => p.ThSeguimientoRequerimientos)
                .HasForeignKey(d => d.IdRequerimiento)
                .HasConstraintName("FK__TH_seguim__Id_re__4D5F7D71");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSeguimientoRequerimientos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_seguim__Id_us__4E53A1AA");
        });

        modelBuilder.Entity<ThSeguimientoSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdSeguimientoSolicitud).HasName("PK__TH_segui__829D5F8E98677F65");

            entity.ToTable("TH_seguimiento_solicitud");

            entity.Property(e => e.IdSeguimientoSolicitud).HasColumnName("Id_seguimiento_solicitud");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdSolicitud).HasColumnName("Id_solicitud");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Prioridad).HasMaxLength(50);

            entity.HasOne(d => d.IdSolicitudNavigation).WithMany(p => p.ThSeguimientoSolicituds)
                .HasForeignKey(d => d.IdSolicitud)
                .HasConstraintName("FK__TH_seguim__Id_so__540C7B00");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSeguimientoSolicituds)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_seguim__Id_us__55009F39");
        });

        modelBuilder.Entity<ThSeleccionPersonalCandidato>(entity =>
        {
            entity.HasKey(e => e.IdCandidato).HasName("PK__Th_selec__D01F62F284D4FEB1");

            entity.ToTable("Th_seleccion_personal_candidatos");

            entity.Property(e => e.IdCandidato).HasColumnName("Id_candidato");
            entity.Property(e => e.Documento)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EstadoPostulacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Estado_postulacion");
            entity.Property(e => e.FechaPostulacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_postulacion");
            entity.Property(e => e.HojaVidaUrl)
                .IsUnicode(false)
                .HasColumnName("Hoja_vida_url");
            entity.Property(e => e.IdCaso).HasColumnName("Id_caso");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Nombre_completo");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCasoNavigation).WithMany(p => p.ThSeleccionPersonalCandidatos)
                .HasForeignKey(d => d.IdCaso)
                .HasConstraintName("FK__Th_selecc__Id_ca__0EF836A4");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSeleccionPersonalCandidatos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Th_selecc__Id_us__0FEC5ADD");
        });

        modelBuilder.Entity<ThSeleccionPersonalCaso>(entity =>
        {
            entity.HasKey(e => e.IdCaso).HasName("PK__Th_selec__549388AD134084E8");

            entity.ToTable("Th_seleccion_personal_caso");

            entity.Property(e => e.IdCaso).HasColumnName("Id_caso");
            entity.Property(e => e.AreaSolicitante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Area_solicitante");
            entity.Property(e => e.CargoRequerido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Cargo_requerido");
            entity.Property(e => e.EstadoSolicitud)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Estado_solicitud");
            entity.Property(e => e.FechaCaso)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_caso");
            entity.Property(e => e.IdSolicitud).HasColumnName("Id_solicitud");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.LinkDocumento)
                .IsUnicode(false)
                .HasColumnName("Link_documento");
            entity.Property(e => e.MotivoSolicitud)
                .IsUnicode(false)
                .HasColumnName("Motivo_solicitud");
            entity.Property(e => e.NumeroVacantes).HasColumnName("Numero_vacantes");
            entity.Property(e => e.Perfil).IsUnicode(false);

            entity.HasOne(d => d.IdSolicitudNavigation).WithMany(p => p.ThSeleccionPersonalCasos)
                .HasForeignKey(d => d.IdSolicitud)
                .HasConstraintName("FK__Th_selecc__Id_so__0B27A5C0");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSeleccionPersonalCasos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Th_selecc__Id_us__0C1BC9F9");
        });

        modelBuilder.Entity<ThSeleccionPersonalContratacione>(entity =>
        {
            entity.HasKey(e => e.IdContratacion).HasName("PK__Th_selec__91F9D593DC1A5227");

            entity.ToTable("Th_seleccion_personal_contrataciones");

            entity.Property(e => e.IdContratacion).HasColumnName("Id_contratacion");
            entity.Property(e => e.AreaAsignada)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Area_asignada");
            entity.Property(e => e.Cargo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Empresa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaIngreso).HasColumnName("Fecha_ingreso");
            entity.Property(e => e.IdCandidato).HasColumnName("Id_candidato");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Salario).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TipoContrato)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tipo_contrato");

            entity.HasOne(d => d.IdCandidatoNavigation).WithMany(p => p.ThSeleccionPersonalContrataciones)
                .HasForeignKey(d => d.IdCandidato)
                .HasConstraintName("FK__Th_selecc__Id_ca__1A69E950");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSeleccionPersonalContrataciones)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Th_selecc__Id_us__1B5E0D89");
        });

        modelBuilder.Entity<ThSeleccionPersonalEntrevista>(entity =>
        {
            entity.HasKey(e => e.IdEntrevista).HasName("PK__Th_selec__14043325CA9293B5");

            entity.ToTable("Th_seleccion_personal_entrevistas");

            entity.Property(e => e.IdEntrevista).HasColumnName("Id_entrevista");
            entity.Property(e => e.Entrevistador)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaEntrevista)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_entrevista");
            entity.Property(e => e.IdCandidato).HasColumnName("Id_candidato");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Observaciones).IsUnicode(false);
            entity.Property(e => e.Resultado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoEntrevista)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tipo_entrevista");

            entity.HasOne(d => d.IdCandidatoNavigation).WithMany(p => p.ThSeleccionPersonalEntrevista)
                .HasForeignKey(d => d.IdCandidato)
                .HasConstraintName("FK__Th_selecc__Id_ca__12C8C788");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSeleccionPersonalEntrevista)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Th_selecc__Id_us__13BCEBC1");
        });

        modelBuilder.Entity<ThSeleccionPersonalExamene>(entity =>
        {
            entity.HasKey(e => e.IdExamen).HasName("PK__Th_selec__E9C6A57A883B80E5");

            entity.ToTable("Th_seleccion_personal_examenes");

            entity.Property(e => e.IdExamen).HasColumnName("Id_examen");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaExamenes).HasColumnName("Fecha_examenes");
            entity.Property(e => e.IdCandidato).HasColumnName("Id_candidato");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Resultado).IsUnicode(false);
            entity.Property(e => e.TipoExamen)
                .IsUnicode(false)
                .HasColumnName("Tipo_examen");

            entity.HasOne(d => d.IdCandidatoNavigation).WithMany(p => p.ThSeleccionPersonalExamenes)
                .HasForeignKey(d => d.IdCandidato)
                .HasConstraintName("FK__Th_selecc__Resul__1699586C");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSeleccionPersonalExamenes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Th_selecc__Id_us__178D7CA5");
        });

        modelBuilder.Entity<ThSeleccionPersonalSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdSolicitud).HasName("PK__Th_selec__38B3E6E349F579AF");

            entity.ToTable("Th_seleccion_personal_solicitud");

            entity.Property(e => e.IdSolicitud).HasColumnName("Id_solicitud");
            entity.Property(e => e.AreaSolicitante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Area_solicitante");
            entity.Property(e => e.CargoRequerido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Cargo_requerido");
            entity.Property(e => e.EstadoSolicitud)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Estado_solicitud");
            entity.Property(e => e.FechaSolicitud)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_solicitud");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.LinkDocumento)
                .IsUnicode(false)
                .HasColumnName("Link_documento");
            entity.Property(e => e.MotivoSolicitud)
                .IsUnicode(false)
                .HasColumnName("Motivo_solicitud");
            entity.Property(e => e.NumeroVacantes).HasColumnName("Numero_vacantes");
            entity.Property(e => e.Perfil).IsUnicode(false);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ThSeleccionPersonalSolicituds)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Th_selecc__Id_em__084B3915");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSeleccionPersonalSolicituds)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Th_selecc__Id_us__075714DC");
        });

        modelBuilder.Entity<ThSolicitudPermiso>(entity =>
        {
            entity.HasKey(e => e.IdSolicitud).HasName("PK__TH_Solic__38B3E6E37D34E6FA");

            entity.ToTable("TH_SolicitudPermiso");

            entity.Property(e => e.IdSolicitud).HasColumnName("Id_solicitud");
            entity.Property(e => e.Area).HasMaxLength(100);
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaPermiso).HasColumnName("Fecha_permiso");
            entity.Property(e => e.FechaSolicitud)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_solicitud");
            entity.Property(e => e.FirmaEmpleado).HasColumnName("Firma_empleado");
            entity.Property(e => e.FirmaJefe).HasColumnName("Firma_jefe");
            entity.Property(e => e.HoraLlegada).HasColumnName("Hora_llegada");
            entity.Property(e => e.HoraSalida).HasColumnName("Hora_salida");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.NumeroHoras).HasColumnName("Numero_horas");
            entity.Property(e => e.ReponeTiempo).HasColumnName("Repone_tiempo");
            entity.Property(e => e.TipoPermiso)
                .HasMaxLength(100)
                .HasColumnName("Tipo_permiso");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSolicitudPermisos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_Solici__Estad__51300E55");
        });

        modelBuilder.Entity<ThSolicitudesDescargo>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudDes).HasName("PK__TH_Solic__787F1F501F37B029");

            entity.ToTable("TH_SolicitudesDescargos");

            entity.Property(e => e.IdSolicitudDes).HasColumnName("Id_solicitudDes");
            entity.Property(e => e.Campo6).HasColumnType("datetime");
            entity.Property(e => e.Campo8).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.Consecutivo)
                .HasMaxLength(18)
                .HasComputedColumnSql("('SGC-RRHH-SOL-'+right('00000'+CONVERT([nvarchar](10),[Id_solicitudDes]),(5)))", true);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaCierre).HasColumnType("datetime");
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.IdTrabajador).HasColumnName("Id_trabajador");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");

            entity.HasOne(d => d.IdTrabajadorNavigation).WithMany(p => p.ThSolicitudesDescargos)
                .HasForeignKey(d => d.IdTrabajador)
                .HasConstraintName("FK__TH_Solici__Id_tr__1E6F845E");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSolicitudesDescargos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_Solici__Id_us__1F63A897");
        });

        modelBuilder.Entity<ThSolicitudesDescargosTestigo>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudDesTes).HasName("PK__TH_Solic__F3D77EF45059AD9C");

            entity.ToTable("TH_SolicitudesDescargosTestigos");

            entity.Property(e => e.IdSolicitudDesTes).HasColumnName("Id_solicitudDesTes");
            entity.Property(e => e.IdSolicitudDes).HasColumnName("Id_solicitudDes");
            entity.Property(e => e.IdTrabajador).HasColumnName("Id_trabajador");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");

            entity.HasOne(d => d.IdSolicitudDesNavigation).WithMany(p => p.ThSolicitudesDescargosTestigos)
                .HasForeignKey(d => d.IdSolicitudDes)
                .HasConstraintName("FK__TH_Solici__Id_so__24285DB4");

            entity.HasOne(d => d.IdTrabajadorNavigation).WithMany(p => p.ThSolicitudesDescargosTestigos)
                .HasForeignKey(d => d.IdTrabajador)
                .HasConstraintName("FK__TH_Solici__Id_tr__22401542");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ThSolicitudesDescargosTestigos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TH_Solici__Id_us__2334397B");
        });

        modelBuilder.Entity<ThTrabajadore>(entity =>
        {
            entity.HasKey(e => e.IdTrabajador).HasName("PK__TH_Traba__9FBC8E1EABC02E1B");

            entity.ToTable("TH_Trabajadores");

            entity.HasIndex(e => e.DocumentoIdentidad, "UQ__TH_Traba__049E81A95EFDD7F9").IsUnique();

            entity.Property(e => e.IdTrabajador).HasColumnName("Id_trabajador");
            entity.Property(e => e.DocumentoIdentidad).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__EF59F762028AD2C6");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Contraseña).HasMaxLength(20);
            entity.Property(e => e.Creacion).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(30);
            entity.Property(e => e.IdArea).HasColumnName("Id_area");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.IdRol).HasColumnName("Id_rol");
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdArea)
                .HasConstraintName("FK__Usuario__Id_area__534D60F1");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Usuario__Id_empr__52593CB8");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuario__Id_rol__5165187F");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PK__Vehiculo__89EBC2177030F80C");

            entity.ToTable("Vehiculo");

            entity.Property(e => e.IdVehiculo).HasColumnName("Id_vehiculo");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.PlacaVehiculo)
                .HasMaxLength(255)
                .HasColumnName("Placa_vehiculo");
        });

        modelBuilder.Entity<VehiculoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdDocumento).HasName("PK__Vehiculo__03E94CC6FF514658");

            entity.ToTable("Vehiculo_Documentos");

            entity.Property(e => e.IdDocumento).HasColumnName("Id_documento");
            entity.Property(e => e.FechaEmision).HasColumnName("Fecha_emision");
            entity.Property(e => e.FechaVencimiento).HasColumnName("Fecha_vencimiento");
            entity.Property(e => e.IdVehiculo).HasColumnName("Id_vehiculo");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Numero_documento");
            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.RutaArchivo)
                .HasMaxLength(500)
                .HasColumnName("Ruta_archivo");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Tipo_documento");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.VehiculoDocumentos)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Vehiculo___Id_ve__6D9742D9");
        });

        modelBuilder.Entity<VehiculoTrazabilidad>(entity =>
        {
            entity.HasKey(e => e.IdVehiculoTrazabilidad).HasName("PK__Vehiculo__929A783DAEE8A9F0");

            entity.ToTable("Vehiculo_trazabilidad");

            entity.Property(e => e.IdVehiculoTrazabilidad).HasColumnName("Id_vehiculo_trazabilidad");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FechaLlegada)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_llegada");
            entity.Property(e => e.FechaSalida)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_salida");
            entity.Property(e => e.GalonesConsumidos)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Galones_consumidos");
            entity.Property(e => e.GalonesTanqueados)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Galones_tanqueados");
            entity.Property(e => e.IdConductor).HasColumnName("Id_conductor");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.IdVehiculo).HasColumnName("Id_vehiculo");
            entity.Property(e => e.KilometrajeFinal).HasColumnName("Kilometraje_final");
            entity.Property(e => e.KilometrajeInicial).HasColumnName("Kilometraje_inicial");
            entity.Property(e => e.TotalRecorrido)
                .HasComputedColumnSql("([Kilometraje_final]-[Kilometraje_inicial])", true)
                .HasColumnName("Total_recorrido");

            entity.HasOne(d => d.IdConductorNavigation).WithMany(p => p.VehiculoTrazabilidads)
                .HasForeignKey(d => d.IdConductor)
                .HasConstraintName("FK__Vehiculo___Id_co__69C6B1F5");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.VehiculoTrazabilidads)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Vehiculo___Id_us__68D28DBC");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.VehiculoTrazabilidads)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Vehiculo___Id_ve__6ABAD62E");
        });

        modelBuilder.Entity<Vendedor>(entity =>
        {
            entity.HasKey(e => e.IdVendedor).HasName("PK__Vendedor__5257D23D23073455");

            entity.ToTable("Vendedor");

            entity.Property(e => e.IdVendedor).HasColumnName("Id_vendedor");
            entity.Property(e => e.Estado).HasMaxLength(30);
            entity.Property(e => e.IdArea).HasColumnName("Id_area");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_empresa");
            entity.Property(e => e.Nombre).HasMaxLength(255);

            entity.HasOne(d => d.CargoNavigation).WithMany(p => p.Vendedors)
                .HasForeignKey(d => d.Cargo)
                .HasConstraintName("FK__Vendedor__Cargo__571DF1D5");

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.Vendedors)
                .HasForeignKey(d => d.IdArea)
                .HasConstraintName("FK__Vendedor__Id_are__5629CD9C");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Vendedors)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Vendedor__Id_emp__3FD07829");
        });

        modelBuilder.Entity<VistaNovedadesComercialesEnRutum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaNovedadesComercialesEnRuta");

            entity.Property(e => e.Causa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Empresa)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaAsignacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_asignacion");
            entity.Property(e => e.FechaNovedad).HasColumnType("datetime");
            entity.Property(e => e.FechaSalida).HasColumnName("Fecha_salida");
            entity.Property(e => e.IdCliente).HasColumnName("Id_cliente");
            entity.Property(e => e.IdConductor).HasColumnName("Id_conductor");
            entity.Property(e => e.IdEstado).HasColumnName("Id_estado");
            entity.Property(e => e.IdRuta).HasColumnName("Id_ruta");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.NGuia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("N_guia");
            entity.Property(e => e.NumeroDocumento).HasColumnName("Numero_Documento");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ObservacionesRuta).IsUnicode(false);
            entity.Property(e => e.Recibido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tipo_documento");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tipo_pago");
            entity.Property(e => e.TipoServicio)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tipo_servicio");
        });

        modelBuilder.Entity<VistaNovedadesComprasEnRutum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaNovedadesComprasEnRuta");

            entity.Property(e => e.Causa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CiudadBarrio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ciudad_barrio");
            entity.Property(e => e.Contacto)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Empresa)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaAsignacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_asignacion");
            entity.Property(e => e.FechaNovedad).HasColumnType("datetime");
            entity.Property(e => e.FechaSalida).HasColumnName("Fecha_salida");
            entity.Property(e => e.IdConductor).HasColumnName("Id_conductor");
            entity.Property(e => e.IdEstado).HasColumnName("Id_estado");
            entity.Property(e => e.IdRuta).HasColumnName("Id_ruta");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.NGuia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("N_guia");
            entity.Property(e => e.Observaciones).IsUnicode(false);
            entity.Property(e => e.ObservacionesRuta).IsUnicode(false);
            entity.Property(e => e.Recibido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoNovedad)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tipo_novedad");
        });

        modelBuilder.Entity<VistaNovedadesGeneralesEnRutum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaNovedadesGeneralesEnRuta");

            entity.Property(e => e.Causa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CiudadBarrio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ciudad_barrio");
            entity.Property(e => e.Contacto)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Empresa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaAsignacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_asignacion");
            entity.Property(e => e.FechaNovedad).HasColumnType("datetime");
            entity.Property(e => e.FechaSalida).HasColumnName("Fecha_salida");
            entity.Property(e => e.IdConductor).HasColumnName("Id_conductor");
            entity.Property(e => e.IdEstado).HasColumnName("Id_estado");
            entity.Property(e => e.IdRuta).HasColumnName("Id_ruta");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.NGuia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("N_guia");
            entity.Property(e => e.Observaciones).IsUnicode(false);
            entity.Property(e => e.ObservacionesRuta).IsUnicode(false);
            entity.Property(e => e.Recibido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoNovedad)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tipo_novedad");
        });

        modelBuilder.Entity<VistaSolicitud>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaSolicitud");

            entity.Property(e => e.CantidadRecibida).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.EstadoProducto).HasMaxLength(100);
            entity.Property(e => e.FechaCompra).HasColumnType("datetime");
            entity.Property(e => e.FechaRecepcionMercancia).HasColumnType("datetime");
            entity.Property(e => e.FechaRequerimiento).HasColumnType("datetime");
            entity.Property(e => e.NovedadCompra)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.OrdenCompra).HasMaxLength(100);
        });

        modelBuilder.Entity<VistaSolicitudArticulo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaSolicitudArticulo");

            entity.Property(e => e.CantidadRecibida)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Cantidad_recibida");
            entity.Property(e => e.CantidadSolicitud).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.ComprobantePago).HasMaxLength(250);
            entity.Property(e => e.ConsecutivoSolicitud).HasMaxLength(17);
            entity.Property(e => e.EstadoCompra).HasMaxLength(50);
            entity.Property(e => e.EstadoPago).HasMaxLength(100);
            entity.Property(e => e.EstadoProducto)
                .HasMaxLength(100)
                .HasColumnName("Estado_producto");
            entity.Property(e => e.EstadoRecepcion).HasMaxLength(30);
            entity.Property(e => e.FechaCompra)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_compra");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.FechaPagoDetalle).HasColumnType("datetime");
            entity.Property(e => e.FechaRecepcion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_recepcion");
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.IdProducto).HasColumnName("Id_producto");
            entity.Property(e => e.IdSolicitud).HasColumnName("Id_solicitud");
            entity.Property(e => e.IdSolicitudDetalle).HasColumnName("Id_solicitud_detalle");
            entity.Property(e => e.OrdenCompra)
                .HasMaxLength(100)
                .HasColumnName("Orden_compra");
            entity.Property(e => e.PrecioCosto)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Precio_costo");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("Precio_venta");
            entity.Property(e => e.UsuarioCompra).HasMaxLength(255);
            entity.Property(e => e.UsuarioPago).HasMaxLength(255);
            entity.Property(e => e.UsuarioRecepcion).HasMaxLength(255);
            entity.Property(e => e.UsuarioSolicitud).HasMaxLength(255);
        });

        modelBuilder.Entity<VistaSolicitudDetalle>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaSolicitudDetalle");

            entity.Property(e => e.CantidadRecibida).HasColumnType("numeric(12, 2)");
            entity.Property(e => e.ComprobantePago).HasMaxLength(250);
            entity.Property(e => e.ConsecutivoNovedad).HasMaxLength(18);
            entity.Property(e => e.ConsecutivoSolicitud).HasMaxLength(17);
            entity.Property(e => e.EstadoCompra).HasMaxLength(50);
            entity.Property(e => e.EstadoPago).HasMaxLength(100);
            entity.Property(e => e.EstadoProducto).HasMaxLength(100);
            entity.Property(e => e.EstadoRecepcion).HasMaxLength(30);
            entity.Property(e => e.FechaCompra).HasColumnType("datetime");
            entity.Property(e => e.FechaNovedad).HasColumnType("datetime");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.FechaPagoDetalle).HasColumnType("datetime");
            entity.Property(e => e.FechaRecepcionMercancia).HasColumnType("datetime");
            entity.Property(e => e.FechaRequerimiento).HasColumnType("datetime");
            entity.Property(e => e.OrdenCompra).HasMaxLength(100);
            entity.Property(e => e.TipoNovedadCompra)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioCompra).HasMaxLength(255);
            entity.Property(e => e.UsuarioNovedad).HasMaxLength(255);
            entity.Property(e => e.UsuarioPago).HasMaxLength(255);
            entity.Property(e => e.UsuarioRecepcion).HasMaxLength(255);
            entity.Property(e => e.UsuarioSolicitud).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
