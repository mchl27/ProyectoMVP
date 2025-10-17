using Management_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Management_system.Controllers
{
    public class MainController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public MainController(DbManagementSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Idx()
        {
            return View();
        }

        // CODE BLOCK
        public IActionResult Comercial()
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                // 1. 
                // Calculate the total sum of sales, including IVA
                decimal totalVentas = _context.RuteroDetalles
                    .Include(rd => rd.IdUsuarioNavigation)
                    .Where(rd => rd.IdUsuarioNavigation.Username == userName)
                    .Sum(c => c.Acumulado ?? 0);

                ViewBag.TotalVentas = totalVentas;

                // 2.
                // Calculate how many customers have been made
                int totalClientes = _context.RuteroDetalles
                    .Include(rd => rd.IdRuteroNavigation)
                    .Where(rd => rd.IdUsuarioNavigation.Username == userName && rd.IdRuteroNavigation.Estado == "Abierta")
                    .Count();

                ViewBag.TotalClientes = totalClientes;

                // 3.
                // Calculate how many quotations have been made
                int totalRuterosAbiertos = _context.Ruteros
                    .Where(r => r.IdUsuarioNavigation.Username == userName && r.Estado == "Abierta")
                    .Count();

                ViewBag.TotalRuterosAbiertos = totalRuterosAbiertos;

                // 4.
                // Calculate how many quotes have been closed
                int totalRuterosCerrados = _context.Ruteros
                    .Where(r => r.IdUsuarioNavigation.Username == userName && r.Estado == "Cerrado")
                    .Count();

                ViewBag.TotalRuterosCerrados = totalRuterosCerrados;
            }
            else
            {
                // 1. 
                // Calculate the total sum of sales, including IVA
                decimal totalVentas = _context.RuteroDetalles
                    .Sum(rd => rd.Acumulado ?? 0);

                ViewBag.TotalVentas = totalVentas;

                // 2.
                // Calculate how many customers have been made
                int totalClientes = _context.RuteroDetalles
                    .Include(rd => rd.IdRuteroNavigation)
                    .Where(rd => rd.IdRuteroNavigation.Estado == "Abierta")
                    .Count();

                ViewBag.TotalClientes = totalClientes;

                // 3.
                // Calculate how many quotations have been made
                int totalRuterosAbiertos = _context.Ruteros
                    .Where(r => r.Estado == "Abierta")
                    .Count();

                ViewBag.TotalRuterosAbiertos = totalRuterosAbiertos;

                // 4.
                // Calculate how many quotes have been closed
                int totalRuterosCerrados = _context.Ruteros
                    .Where(r => r.Estado == "Cerrado")
                    .Count();

                ViewBag.TotalRuterosCerrados = totalRuterosCerrados;
            }



            //// 5.
            //var ventasPorMes = _context.CotizacionDetalles
            //.Where(cd => cd.IdCotizacionNavigation.Fecha.HasValue)
            //.GroupBy(cd => new { Año = cd.IdCotizacionNavigation.Fecha.Value.Year, Mes = cd.IdCotizacionNavigation.Fecha.Value.Month })
            //.Select(g => new
            //{
            //    Año = g.Key.Año,
            //    Mes = g.Key.Mes,
            //    TotalVentas = g.Sum(cd => cd.TotalConIva ?? 0)
            //})
            //.OrderBy(g => g.Año).ThenBy(g => g.Mes)
            //.ToList();

            //ViewBag.VentasPorMes = ventasPorMes;


            //var ventasPorUsuarioMes = _context.CotizacionDetalles
            //.Where(cd => cd.IdCotizacionNavigation.Fecha.HasValue && cd.IdCotizacionNavigation.IdUsuario.HasValue)
            //.GroupBy(cd => new { Año = cd.IdCotizacionNavigation.Fecha.Value.Year, Mes = cd.IdCotizacionNavigation.Fecha.Value.Month, Usuario = cd.IdCotizacionNavigation.IdUsuario })
            //.Select(g => new
            //{
            //    Año = g.Key.Año,
            //    Mes = g.Key.Mes,
            //    Usuario = g.Key.Usuario,
            //    TotalVentas = g.Sum(cd => cd.TotalConIva ?? 0)
            //})
            //.OrderBy(g => g.Año).ThenBy(g => g.Mes)
            //.ToList();

            //ViewBag.VentasPorUsuarioMes = ventasPorUsuarioMes;


            return View();
        }


        // CODE BLOCK
        public IActionResult Solicitud()
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                // 1. 
                // Calculate the total sum of sales, including IVA
                decimal totalSolicitado = _context.SolicitudDetalles
                    .Include(sd => sd.IdSolicitudNavigation)
                        .ThenInclude(s => s.IdUsuarioNavigation)
                    .Where(s => s.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName)
                    .Sum(c => (c.Cantidad ?? 0) * (c.PrecioCosto ?? 0));

                ViewBag.TotalSolicitado = totalSolicitado;

                // 2.
                // Calculate how many quotations have been made
                int totalSolicitudesAnuladas = _context.Solicituds
                    .Where(s => s.IdUsuarioNavigation.Username == userName && s.Estado == "Anulado")
                    .Count();

                ViewBag.TotalSolicitudesAnuladas = totalSolicitudesAnuladas;

                // 3.
                // Calculate how many users have been made
                var estadosPermitidos = new[] { "Abierta", "Revisada", "Urgente", "Legalizar" };
                int totalSolicitudesAbiertas = _context.Solicituds
                    .Where(s => s.IdUsuarioNavigation.Username == userName &&
                                estadosPermitidos.Contains(s.Estado))
                    .Count();

                ViewBag.TotalSolicitudesAbiertas = totalSolicitudesAbiertas;

                // 4.
                // Calculate how many quotes have been closed
                int totalSolicitudesCerradas = _context.Solicituds
                    .Where(s => s.IdUsuarioNavigation.Username == userName && s.Estado == "OC Creada")
                    .Count();

                ViewBag.TotalSolicitudesCerradas = totalSolicitudesCerradas;
            }
            else
            {
                // 1. 
                // Calculate the total sum of sales, including IVA
                decimal totalSolicitado = _context.SolicitudDetalles
                    .Sum(c => (c.Cantidad ?? 0) * (c.PrecioCosto ?? 0));

                ViewBag.TotalSolicitado = totalSolicitado;

                // 2.
                // Calculate how many quotations have been made
                int totalSolicitudesAnuladas = _context.Solicituds
                    .Where(s => s.Estado == "Anulado")
                    .Count();

                ViewBag.TotalSolicitudesAnuladas = totalSolicitudesAnuladas;

                // 3.
                // Calculate how many users have been made
                int totalSolicitudesAbiertas = _context.Solicituds
                    .Where(s => s.Estado == "Abierta" || s.Estado == "Revisada" || s.Estado == "Urgente" || s.Estado == "Legalizar")
                    .Count();

                ViewBag.TotalSolicitudesAbiertas = totalSolicitudesAbiertas;

                // 4.
                // Calculate how many quotes have been closed
                int totalSolicitudesCerradas = _context.Solicituds
                    .Where(s => s.Estado == "OC Creada")
                    .Count();

                ViewBag.TotalSolicitudesCerradas = totalSolicitudesCerradas;
            }



            return View();
        }

        // CODE BLOCK
        public IActionResult Compras()
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                // 1. 
                // Calculate the total sum of sales, including IVA
                decimal totalSolicitado = _context.SolicitudDetalles
                    .Sum(c => (c.Cantidad ?? 0) * (c.PrecioCosto ?? 0));

                ViewBag.TotalSolicitado = totalSolicitado;

                // 2.
                // Calculate how many users have been made
                int totalSolicitudesAbiertas = _context.Solicituds
                    .Where(s => s.Estado == "Abierta" || s.Estado == "Revisada" || s.Estado == "Urgente" || s.Estado == "Legalizar")
                    .Count();

                ViewBag.TotalSolicitudesAbiertas = totalSolicitudesAbiertas;

                // 3.
                // Calculate how many users have been made
                int totalComprasAbiertas = _context.Compras
                    .Where(c => c.Estado == "Abierta")
                    .Count();

                ViewBag.TotalComprasAbiertas = totalComprasAbiertas;

                // 4.
                // Calculate how many quotes have been closed
                int totalComprasParciales = _context.Compras
                    .Where(c => c.Estado == "Parcial")
                    .Count();

                ViewBag.TotalComprasParciales = totalComprasParciales;

                // 5.
                // Calculate how many quotes have been closed
                int totalComprasRecibidas = _context.Compras
                    .Where(c => c.Estado == "Recibido")
                    .Count();

                ViewBag.TotalComprasRecibidas = totalComprasRecibidas;
            }
            else
            {
                // 1. 
                // Calculate the total sum of sales, including IVA
                decimal totalSolicitado = _context.SolicitudDetalles
                    .Sum(c => (c.Cantidad ?? 0) * (c.PrecioCosto ?? 0));

                ViewBag.TotalSolicitado = totalSolicitado;

                // 2.
                // Calculate how many users have been made
                int totalSolicitudesAbiertas = _context.Solicituds
                    .Where(s => s.Estado == "Abierta" || s.Estado == "Revisada" || s.Estado == "Urgente" || s.Estado == "Legalizar")
                    .Count();

                ViewBag.TotalSolicitudesAbiertas = totalSolicitudesAbiertas;

                // 3.
                // Calculate how many users have been made
                int totalComprasAbiertas = _context.Compras
                    .Where(c => c.Estado == "Abierta")
                    .Count();

                ViewBag.TotalComprasAbiertas = totalComprasAbiertas;

                // 4.
                // Calculate how many quotes have been closed
                int totalComprasParciales = _context.Compras
                    .Where(c => c.Estado == "Parcial")
                    .Count();

                ViewBag.TotalComprasParciales = totalComprasParciales;

                // 5.
                // Calculate how many quotes have been closed
                int totalComprasRecibidas = _context.Compras
                    .Where(c => c.Estado == "Recibido")
                    .Count();

                ViewBag.TotalComprasRecibidas = totalComprasRecibidas;
            }



            //// 5.
            //var ventasPorMes = _context.CotizacionDetalles
            //.Where(cd => cd.IdCotizacionNavigation.Fecha.HasValue)
            //.GroupBy(cd => new { Año = cd.IdCotizacionNavigation.Fecha.Value.Year, Mes = cd.IdCotizacionNavigation.Fecha.Value.Month })
            //.Select(g => new
            //{
            //    Año = g.Key.Año,
            //    Mes = g.Key.Mes,
            //    TotalVentas = g.Sum(cd => cd.TotalConIva ?? 0)
            //})
            //.OrderBy(g => g.Año).ThenBy(g => g.Mes)
            //.ToList();

            //ViewBag.VentasPorMes = ventasPorMes;


            //var ventasPorUsuarioMes = _context.CotizacionDetalles
            //.Where(cd => cd.IdCotizacionNavigation.Fecha.HasValue && cd.IdCotizacionNavigation.IdUsuario.HasValue)
            //.GroupBy(cd => new { Año = cd.IdCotizacionNavigation.Fecha.Value.Year, Mes = cd.IdCotizacionNavigation.Fecha.Value.Month, Usuario = cd.IdCotizacionNavigation.IdUsuario })
            //.Select(g => new
            //{
            //    Año = g.Key.Año,
            //    Mes = g.Key.Mes,
            //    Usuario = g.Key.Usuario,
            //    TotalVentas = g.Sum(cd => cd.TotalConIva ?? 0)
            //})
            //.OrderBy(g => g.Año).ThenBy(g => g.Mes)
            //.ToList();

            //ViewBag.VentasPorUsuarioMes = ventasPorUsuarioMes;


            return View();
        }

        public IActionResult Contabilidad()
        {
            var pagosPendientes = _context.Pagos
                .Include(p => p.IdCompraNavigation)
                .Where(p => (p.Estado == "En proceso" || p.Estado == "Cerrada") && p.Observaciones1 == "Abierta")
                .ToList();

            // Count the number Payments pending
            var conteoPagos = pagosPendientes.Count;

            // Pass counter to the view
            ViewBag.ContadorPagos = conteoPagos;

            return View(pagosPendientes);
        }

        // CODE BLOCK
        public IActionResult Logistica()
        {

            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                // 1. Novedad General
                var novedadesUsuario = _context.Novedads
                    .Where(s => s.IdUsuarioNavigation.Username == userName)
                    .ToList();

                ViewBag.TotalNovedadesGenerales = novedadesUsuario.Count;
                ViewBag.TotalNovedadesGeneralesAbiertas = novedadesUsuario.Count(n => n.Estado == "Abierta");
                ViewBag.TotalNovedadesGeneralesProgramadas = novedadesUsuario.Count(n => n.Estado == "Programada");
                ViewBag.TotalNovedadesGeneralesCerradas = novedadesUsuario.Count(n => n.Estado == "Cerrada");


                // 2. Novedad Compras
                var novedadesCompras = _context.NovedadCompras
                    .ToList();

                ViewBag.TotalNovedadesCompras = novedadesCompras.Count;
                ViewBag.TotalNovedadesComprasAbiertas = novedadesCompras.Count(nc => nc.Estado == "Abierta");
                ViewBag.TotalNovedadesComprasProgramadas = novedadesCompras.Count(nc => nc.Estado == "Programada");
                ViewBag.TotalNovedadesComprasCerradas = novedadesCompras.Count(nc => nc.Estado == "Cerrada");


                // 3. Novedad Comercial
                var novedadesComerciales = _context.NovedadComercials
                    .Where(s => s.IdUsuarioNavigation.Username == userName)
                    .ToList();

                ViewBag.TotalNovedadesComerciales = novedadesComerciales.Count;
                ViewBag.TotalNovedadesComercialesAbiertas = novedadesComerciales.Count(ncm => ncm.Estado == "Abierta");
                ViewBag.TotalNovedadesComercialesProgramadas = novedadesComerciales.Count(ncm => ncm.Estado == "Programada");
                ViewBag.TotalNovedadesComercialesCerradas = novedadesComerciales.Count(ncm => ncm.Estado == "Cerrada");


                // 4. Ruta
                var rutas = _context.Ruta
                    .ToList();

                ViewBag.TotalRutas = rutas.Count;
                ViewBag.TotalRutasAbiertas = rutas.Count(r => r.Estado == "Abierta");
                ViewBag.TotalRutasCerradas = rutas.Count(r => r.Estado == "Cerrada");

            }
            else
            {
                // 1. Novedad General
                var novedadesUsuario = _context.Novedads
                    .ToList();

                ViewBag.TotalNovedadesGenerales = novedadesUsuario.Count;
                ViewBag.TotalNovedadesGeneralesAbiertas = novedadesUsuario.Count(n => n.Estado == "Abierta");
                ViewBag.TotalNovedadesGeneralesProgramadas = novedadesUsuario.Count(n => n.Estado == "Programada");
                ViewBag.TotalNovedadesGeneralesCerradas = novedadesUsuario.Count(n => n.Estado == "Cerrada");


                // 2. Novedad Compras
                var novedadesCompras = _context.NovedadCompras
                    .ToList();

                ViewBag.TotalNovedadesCompras = novedadesCompras.Count;
                ViewBag.TotalNovedadesComprasAbiertas = novedadesCompras.Count(n => n.Estado == "Abierta");
                ViewBag.TotalNovedadesComprasProgramadas = novedadesCompras.Count(n => n.Estado == "Programada");
                ViewBag.TotalNovedadesComprasCerradas = novedadesCompras.Count(n => n.Estado == "Cerrada");


                // 3. Novedad Comercial
                var novedadesComerciales = _context.NovedadComercials
                    .ToList();

                ViewBag.TotalNovedadesComerciales = novedadesComerciales.Count;
                ViewBag.TotalNovedadesComercialesAbiertas = novedadesComerciales.Count(n => n.Estado == "Abierta");
                ViewBag.TotalNovedadesComercialesProgramadas = novedadesComerciales.Count(n => n.Estado == "Programada");
                ViewBag.TotalNovedadesComercialesCerradas = novedadesComerciales.Count(n => n.Estado == "Cerrada");


                // 4. Ruta
                var rutas = _context.Ruta
                    .ToList();

                ViewBag.TotalRutas = rutas.Count;
                ViewBag.TotalRutasAbiertas = rutas.Count(r => r.Estado == "Abierta");
                ViewBag.TotalRutasCerradas = rutas.Count(r => r.Estado == "Cerrada");
            }


            return View();
        }

        // CODE BLOCK
        public IActionResult RecepcionMercancia()
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                // 1. 
                // Contar los proveedores únicos en RecepcionMercancia
                int totalProveedores = _context.RecepcionMercancia
                    .Where(r => r.IdCompraNavigation.IdSolicitudNavigation != null) // Filtrar registros válidos
                    .Select(r => r.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation) // Seleccionar solo IdProveedor
                    .Distinct() // Obtener valores únicos
                    .Count(); // Contar la cantidad de proveedores únicos

                ViewBag.TotalProveedores = totalProveedores;

                // 2.
                // Calculate how many users have been made
                int totalComprasAbiertas = _context.Compras
                    .Where(c => c.Estado == "Abierta")
                    .Count();

                ViewBag.TotalComprasAbiertas = totalComprasAbiertas;

                // 3.
                // Calculate how many users have been made
                int totalRecepcionRevision = _context.RecepcionMercancia
                    .Where(c => c.Estado == "En Revision")
                    .Count();

                ViewBag.TotalRecepcionRevision = totalRecepcionRevision;

                // 4.
                // Calculate how many quotes have been closed
                int totalRecepcionParcial = _context.RecepcionMercancia
                    .Where(c => c.Estado == "Parcial")
                    .Count();

                ViewBag.TotalRecepcionParcial = totalRecepcionParcial;

                // 5.
                // Calculate how many quotes have been closed
                int totalRecepcionRecibida = _context.RecepcionMercancia
                    .Where(c => c.Estado == "Recibida")
                    .Count();

                ViewBag.TotalRecepcionRecibida = totalRecepcionRecibida;
            }
            else
            {
                // 1. 
                // Contar los proveedores únicos en RecepcionMercancia
                int totalProveedores = _context.RecepcionMercancia
                    .Where(r => r.IdCompraNavigation.IdSolicitudNavigation != null) // Filtrar registros válidos
                    .Select(r => r.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation) // Seleccionar solo IdProveedor
                    .Distinct() // Obtener valores únicos
                    .Count(); // Contar la cantidad de proveedores únicos

                ViewBag.TotalProveedores = totalProveedores;

                // 2.
                // Calculate how many users have been made
                int totalComprasAbiertas = _context.Compras
                    .Where(c => c.Estado == "Abierta")
                    .Count();

                ViewBag.TotalComprasAbiertas = totalComprasAbiertas;

                // 3.
                // Calculate how many users have been made
                int totalRecepcionRevision = _context.RecepcionMercancia
                    .Where(c => c.Estado == "En Revision")
                    .Count();

                ViewBag.TotalRecepcionRevision = totalRecepcionRevision;

                // 4.
                // Calculate how many quotes have been closed
                int totalRecepcionParcial = _context.RecepcionMercancia
                    .Where(c => c.Estado == "Parcial")
                    .Count();

                ViewBag.TotalRecepcionParcial = totalRecepcionParcial;

                // 5.
                // Calculate how many quotes have been closed
                int totalRecepcionRecibida = _context.RecepcionMercancia
                    .Where(c => c.Estado == "Recibida")
                    .Count();

                ViewBag.TotalRecepcionRecibida = totalRecepcionRecibida;
            }



            //// 5.
            //var ventasPorMes = _context.CotizacionDetalles
            //.Where(cd => cd.IdCotizacionNavigation.Fecha.HasValue)
            //.GroupBy(cd => new { Año = cd.IdCotizacionNavigation.Fecha.Value.Year, Mes = cd.IdCotizacionNavigation.Fecha.Value.Month })
            //.Select(g => new
            //{
            //    Año = g.Key.Año,
            //    Mes = g.Key.Mes,
            //    TotalVentas = g.Sum(cd => cd.TotalConIva ?? 0)
            //})
            //.OrderBy(g => g.Año).ThenBy(g => g.Mes)
            //.ToList();

            //ViewBag.VentasPorMes = ventasPorMes;


            //var ventasPorUsuarioMes = _context.CotizacionDetalles
            //.Where(cd => cd.IdCotizacionNavigation.Fecha.HasValue && cd.IdCotizacionNavigation.IdUsuario.HasValue)
            //.GroupBy(cd => new { Año = cd.IdCotizacionNavigation.Fecha.Value.Year, Mes = cd.IdCotizacionNavigation.Fecha.Value.Month, Usuario = cd.IdCotizacionNavigation.IdUsuario })
            //.Select(g => new
            //{
            //    Año = g.Key.Año,
            //    Mes = g.Key.Mes,
            //    Usuario = g.Key.Usuario,
            //    TotalVentas = g.Sum(cd => cd.TotalConIva ?? 0)
            //})
            //.OrderBy(g => g.Año).ThenBy(g => g.Mes)
            //.ToList();

            //ViewBag.VentasPorUsuarioMes = ventasPorUsuarioMes;


            return View();
        }

        // CODE BLOCK
        public IActionResult TalentoHumano()
        {
            return View();
        }

        public IActionResult Tarea()
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                // 1. 
                // Calculate the total sum of sales, including IVA
                decimal totalSolicitado = _context.SolicitudDetalles
                    .Include(sd => sd.IdSolicitudNavigation)
                        .ThenInclude(s => s.IdUsuarioNavigation)
                    .Where(s => s.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName)
                    .Sum(c => (c.Cantidad ?? 0) * (c.PrecioCosto ?? 0));

                ViewBag.TotalSolicitado = totalSolicitado;

                // 2.
                // Calculate how many quotations have been made
                int totalSolicitudesAnuladas = _context.Solicituds
                    .Where(s => s.IdUsuarioNavigation.Username == userName && s.Estado == "Anulado")
                    .Count();

                ViewBag.TotalSolicitudesAnuladas = totalSolicitudesAnuladas;

                // 3.
                // Calculate how many users have been made
                var estadosPermitidos = new[] { "Abierta", "Revisada", "Urgente", "Legalizar" };
                int totalSolicitudesAbiertas = _context.Solicituds
                    .Where(s => s.IdUsuarioNavigation.Username == userName &&
                                estadosPermitidos.Contains(s.Estado))
                    .Count();

                ViewBag.TotalSolicitudesAbiertas = totalSolicitudesAbiertas;

                // 4.
                // Calculate how many quotes have been closed
                int totalSolicitudesCerradas = _context.Solicituds
                    .Where(s => s.IdUsuarioNavigation.Username == userName && s.Estado == "OC Creada")
                    .Count();

                ViewBag.TotalSolicitudesCerradas = totalSolicitudesCerradas;
            }
            else
            {
                // 1. 
                // Calculate the total sum of sales, including IVA
                decimal totalSolicitado = _context.SolicitudDetalles
                    .Sum(c => (c.Cantidad ?? 0) * (c.PrecioCosto ?? 0));

                ViewBag.TotalSolicitado = totalSolicitado;

                // 2.
                // Calculate how many quotations have been made
                int totalSolicitudesAnuladas = _context.Solicituds
                    .Where(s => s.Estado == "Anulado")
                    .Count();

                ViewBag.TotalSolicitudesAnuladas = totalSolicitudesAnuladas;

                // 3.
                // Calculate how many users have been made
                int totalSolicitudesAbiertas = _context.Solicituds
                    .Where(s => s.Estado == "Abierta" || s.Estado == "Revisada" || s.Estado == "Urgente" || s.Estado == "Legalizar")
                    .Count();

                ViewBag.TotalSolicitudesAbiertas = totalSolicitudesAbiertas;

                // 4.
                // Calculate how many quotes have been closed
                int totalSolicitudesCerradas = _context.Solicituds
                    .Where(s => s.Estado == "OC Creada")
                    .Count();

                ViewBag.TotalSolicitudesCerradas = totalSolicitudesCerradas;
            }



            return View();
        }

        public IActionResult Configuration()
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if ((userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" && 
                 userRole != "Soporte TI" && userRole != "Editor de Informacion")) && (userArea == null || userArea != "Compras"))
            {
                // Redirect the user to the main home page if they do not have the appropriate roles
                return RedirectToAction("Index", "Main");
            }

            return View();
        }

    }
}
