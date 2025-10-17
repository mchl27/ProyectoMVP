using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System.Drawing.Printing;
using System.Net;

namespace Management_system.Controllers
{
    public class VistaSolicitudController : Controller
    {

        private readonly DbManagementSystemContext _context;

        public VistaSolicitudController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: VistaSolicitudController
        public async Task<IActionResult> SeguimientoMain(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {

            IQueryable<SolicitudDetalle> query = _context.SolicitudDetalles
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdClienteNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdUsuarioNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdUsuarioNavigation.IdEmpresaNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(sd => sd.IdVendedorNavigation)
                .Include(sd => sd.IdProductoNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(sd => sd.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(sd => sd.IdSolicitudNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Cliente":
                        query = query.Where(sd => sd.IdSolicitudNavigation.IdClienteNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        query = query.Where(sd => sd.IdSolicitudNavigation.IdProveedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        query = query.Where(sd => sd.IdSolicitudNavigation.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Referencia":
                        query = query.Where(sd => sd.IdProductoNavigation.Referencia.Contains(searchTerm));
                        break;
                    case "Descripcion":
                        query = query.Where(sd => sd.IdProductoNavigation.Descripcion.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(sd => sd.IdSolicitudNavigation.Fecha >= fechaInicio && sd.IdSolicitudNavigation.Fecha < fechaFin);
            }


            query = query.OrderByDescending(sd => sd.IdSolicitudNavigation.IdSolicitud);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new SeguimientoSolicitudesViewModel
            {
                Solicitudes = paginatedItems,
                Pagination = new PaginationViewModelSolicitudesSegumiento
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        //GET: VistaSolicitudController / SegumientoD/Details
        public async Task<IActionResult> SeguimientoD(int? id)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;

            var SolicitudQuery = _context.Solicituds
            .Include(s => s.IdClienteNavigation)
            .Include(s => s.IdProveedorNavigation)
            .Include(s => s.IdUsuarioNavigation)
            .Include(s => s.IdVendedorNavigation)
            .Where(s => s.IdSolicitud == id);

            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                SolicitudQuery = SolicitudQuery.Where(c => c.IdUsuarioNavigation.Username == userName);
            }

            var solicitud = SolicitudQuery.FirstOrDefault();

            if (solicitud == null)
            {
                return RedirectToAction("Index", "SolicitudUsuarios");
            }

            var solicitudDetalles = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .Where(sd => sd.IdSolicitud == id)
                .ToList();

            var model = new SolicitudCompraSeguimientoViewModel
            {
                Solicitudes = solicitud,
                SolicitudDetalles = solicitudDetalles
            };

            return View(model);
        }

        // POST: Cotizacion/GenerarPDF
        public IActionResult GenerarPDF(int id)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;

            var SolicitudQuery = _context.Solicituds
            .Include(s => s.IdClienteNavigation)
            .Include(s => s.IdProveedorNavigation)
            .Include(s => s.IdUsuarioNavigation)
            .Include(s => s.IdVendedorNavigation)
            .Where(s => s.IdSolicitud == id);

            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                SolicitudQuery = SolicitudQuery.Where(c => c.IdUsuarioNavigation.Username == userName);
            }

            var solicitud = SolicitudQuery.FirstOrDefault();

            if (solicitud == null)
            {
                return RedirectToAction("Index", "SolicitudUsuarios");
            }

            var solicitudDetalles = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .Where(sd => sd.IdSolicitud == id)
                .ToList();

            var model = new SolicitudCompraSeguimientoPDFViewModel
            {
                Solicitudes = solicitud,
                SolicitudDetalles = solicitudDetalles
            };

            return new ViewAsPdf("SeguimientoDPDF", model)
            {
                FileName = $"{solicitud.Consecutivo}_{solicitud.IdClienteNavigation.Nombre}.pdf"
            };
        }


        public ActionResult Seguimiento(int? idsolicitud)
        {
            if (idsolicitud == null)
            {
                return NotFound();
            }

            // Buscar la solicitud por su ID
            VistaSolicitudDetalle vistaSolicitud = _context.VistaSolicitudDetalles.FirstOrDefault(s => s.IdRequerimiento == idsolicitud);

            if (vistaSolicitud == null)
            {
                return NotFound();
            }

            return View(vistaSolicitud);
        }


        // GET:ViewSolicitud / SeguimientoDetails
        public IActionResult SeguimientoDetails(int idSolicitud)
        {
            var receptionViewModel = new ReceptionViewModel();

            // Obtener la compra
            var compra = _context.Compras
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefault(c => c.IdSolicitud == idSolicitud);

            if (compra == null)
            {
                return NotFound();
            }

            receptionViewModel.NuevaCompra = compra;

            // Obtener la solicitud
            var solicitud = _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdVendedorNavigation)
                .FirstOrDefault(s => s.IdSolicitud == idSolicitud);

            if (solicitud == null)
            {
                return NotFound();
            }

            receptionViewModel.NuevaSolicitud = solicitud;

            var recepcion = _context.RecepcionMercancia
                .Include(r => r.IdCompraNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefault(r => r.IdCompraNavigation.IdSolicitud == idSolicitud);
            if (recepcion == null)
            {
                return NotFound();
            }

            receptionViewModel.NuevaRecepcion = recepcion;

            // Obtener los detalles de la solicitud
            var solicitudDetalles = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .Where(sd => sd.IdSolicitud == idSolicitud)
                .ToList();

            receptionViewModel.DetallesSolicitud = solicitudDetalles;

            // Obtener los detalles de compra
            var compraDetalles = _context.CompraDetalles
                .Include(cd => cd.IdCompraNavigation)
                .Include(cd => cd.IdSolicitudDetalleNavigation)
                .Where(cd => cd.IdSolicitudDetalleNavigation.IdSolicitud == idSolicitud)
                .ToList();

            receptionViewModel.Detalles = compraDetalles;


            var recepciondetalle = _context.RecepcionDetalles
                .Include(rd => rd.IdRecepcionNavigation)
                .Include(rd => rd.IdSolicitudDetalleNavigation)
                .Where(rd => rd.IdSolicitudDetalleNavigation.IdSolicitud == idSolicitud)
                .ToList();

            receptionViewModel.DetallesRecepcion = recepciondetalle;


            return View(receptionViewModel);
        }


        // GET: VistaSolicitudController
        public async Task<IActionResult> CSeguimientoMain(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<Compra> query = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdUsuarioNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation);

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(c => c.OrdenCompra.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        query = query.Where(c => c.IdSolicitudNavigation.IdProveedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        query = query.Where(c => c.IdSolicitudNavigation.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(s => s.FechaCompra >= fechaInicio && s.FechaCompra < fechaFin);
            }

            query = query.OrderByDescending(c => c.IdCompra);
            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new CSeguimientoSolicitudesViewModel
            {
                Compras = paginatedItems,
                Pagination = new CPaginationViewModelSolicitudesSegumiento
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        //GET: VistaSolicitudController / SegumientoD/Details
        public async Task<IActionResult> CSeguimientoD(int? id)
        {
            var solicitud = _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdVendedorNavigation)
                .FirstOrDefault(s => s.IdSolicitud == id);

            if (solicitud == null)
            {
                return NotFound();
            }

            //var userName = User.Identity.Name;
            //var userRole = User.FindFirst("Rol")?.Value;

            //if (userRole == null ||
            //    (userRole != "Administrador" && userRole != "Super Usuario"
            //    && userRole != "Desarrollador" && userRole != "Soporte TI"))
            //{
            //    solicitud = _context.Solicituds
            //   .Include(s => s.IdClienteNavigation)
            //   .Include(s => s.IdProveedorNavigation)
            //   .Include(s => s.IdUsuarioNavigation)
            //   .Include(s => s.IdVendedorNavigation)
            //   .FirstOrDefault(s => s.IdSolicitud == id && s.IdUsuarioNavigation.Username == userName);

            //    if (solicitud == null)
            //    {
            //        return RedirectToAction("Index", "SolicitudUsuarios");
            //    }
            //}

            ViewData["IdSolicitud"] = solicitud.IdSolicitud;
            ViewData["Fecha"] = solicitud.Fecha;
            ViewData["IdCliente"] = solicitud.IdClienteNavigation.Nombre;
            ViewData["IdProveedor"] = solicitud.IdProveedorNavigation.Nombre;
            ViewData["IdUsuario"] = solicitud.IdUsuarioNavigation.Username;
            ViewData["IdVendedor"] = solicitud.IdVendedorNavigation.Nombre;
            ViewData["Estado"] = solicitud.Estado;
            ViewData["Consecutivo"] = solicitud.Consecutivo;
            ViewData["Observaciones"] = solicitud.Observaciones;
            ViewData["ProveedorSugerido"] = solicitud.ProveedorSugerido;
            ViewData["FechaEntrega"] = solicitud.FechaEntrega;
            ViewData["Negociacion"] = solicitud.Negociacion;
            ViewData["FechaActualizacion"] = solicitud.FechaActualizacion;
            ViewData["ObservacionActualizacion"] = solicitud.ObservacionesActualizacion;

            var solicitudDetalles = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .Where(sd => sd.IdSolicitud == id)
                .ToList();

            ViewData["SolicitudDetalles"] = solicitudDetalles;

            return View();
        }

        public ActionResult CSeguimiento(int? idsolicitud)
        {
            if (idsolicitud == null)
            {
                return NotFound();
            }

            // Search for the application by its ID
            VistaSolicitud vistaSolicitud = _context.VistaSolicituds.FirstOrDefault(s => s.IdRequerimiento == idsolicitud);

            if (vistaSolicitud == null)
            {
                return NotFound();
            }

            return View(vistaSolicitud);
        }

        // GET:VistaSolicitud / CSeguimientoDetails
        public IActionResult CSeguimientoDetails(int idSolicitud)
        {
            var receptionViewModel = new ReceptionViewModel();

            // Obtener la compra
            var compra = _context.Compras
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefault(c => c.IdSolicitud == idSolicitud);

            if (compra == null)
            {
                return NotFound();
            }

            receptionViewModel.NuevaCompra = compra;

            // Obtener la solicitud
            var solicitud = _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdVendedorNavigation)
                .FirstOrDefault(s => s.IdSolicitud == idSolicitud);

            if (solicitud == null)
            {
                return NotFound();
            }

            receptionViewModel.NuevaSolicitud = solicitud;

            var recepcion = _context.RecepcionMercancia
                .Include(r => r.IdCompraNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefault(r => r.IdCompraNavigation.IdSolicitud == idSolicitud);
            if (recepcion == null)
            {
                return NotFound();
            }

            receptionViewModel.NuevaRecepcion = recepcion;

            // Obtener los detalles de la solicitud
            var solicitudDetalles = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .Where(sd => sd.IdSolicitud == idSolicitud)
                .ToList();

            receptionViewModel.DetallesSolicitud = solicitudDetalles;

            // Obtener los detalles de compra
            var compraDetalles = _context.CompraDetalles
                .Include(cd => cd.IdCompraNavigation)
                .Include(cd => cd.IdSolicitudDetalleNavigation)
                .Where(cd => cd.IdSolicitudDetalleNavigation.IdSolicitud == idSolicitud)
                .ToList();

            receptionViewModel.Detalles = compraDetalles;


            var recepciondetalle = _context.RecepcionDetalles
                .Include(rd => rd.IdRecepcionNavigation)
                .Include(rd => rd.IdSolicitudDetalleNavigation)
                .Where(rd => rd.IdSolicitudDetalleNavigation.IdSolicitud == idSolicitud)
                .ToList();

            receptionViewModel.DetallesRecepcion = recepciondetalle;


            return View(receptionViewModel);
        }


    }
}
