using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Management_system.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Rotativa.AspNetCore;
using Management_system.Models.Other.ViewModel.Logistica;

namespace Management_system.Controllers
{
    public class NovedadCompraUsuarioController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public NovedadCompraUsuarioController(DbManagementSystemContext context)
        {
            _context = context;
        }


        // VIEWS COMPRAS

        // GET: Pagos Index
        public async Task<IActionResult> ReviewIndex(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<Compra> query = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation);

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(c => c.OrdenCompra.Contains(searchTerm));
                        break;
                    case "Solicitud":
                        query = query.Where(c => c.IdSolicitudNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        query = query.Where(c => c.IdSolicitudNavigation.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Usuario":
                        query = query.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        query = query.Where(c => c.IdSolicitudNavigation.IdProveedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Estado":
                        query = query.Where(c => c.Estado.Contains(searchTerm));
                        break;
                    case "Pagos":
                        query = query.Where(c => c.Observaciones.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(s => s.FechaEntrega >= fechaInicio && s.FechaEntrega < fechaFin);
            }

            query = query.Where(c => c.Estado == "Abierta");
            query = query.Where(c => c.Observaciones1 != "Programado" && c.Observaciones1 != "Cerrada");
            query = query.OrderByDescending(c => c.IdCompra);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ComprasReviewNCViewModel
            {
                ComprasNC = paginatedItems,
                Pagination = new PaginationViewModelNovedadCompraNC
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }


        // VIEWS NOVEDAD

        // GET: PagosUsuarios/ReviewCreate
        public IActionResult Create(int idCompra)
        {

            var compra = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .FirstOrDefault(c => c.IdCompra == idCompra);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                // If the user has admin, developer or support roles, display all users
                var userName = User.Identity.Name;
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios.Where(u => u.Username == userName), "IdUsuario", "Username");
            }
            else
            {
                // If the user has admin, developer or support roles, display all users
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username");
            }

            ViewData["IdCompra"] = idCompra;
            ViewData["OrdenCompra"] = compra.OrdenCompra;
            ViewData["Empresa"] = compra.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
            return View();
        }

        // POST: PagosUsuarios/ReviewCreate
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNovedadCompras,IdCompra,Fecha,TipoNovedad,FechaSalida,Empresa,Direccion,CiudadBarrio,IdUsuario,Contacto,Telefono,Observaciones,Consecutivo,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Estado,EstadoLogistica,ObservacionesLogistica")] NovedadCompra novedadCompra)
        {
            if (ModelState.IsValid)
            {
                novedadCompra.Fecha = DateTime.Now;
                novedadCompra.Estado = "Abierta";
                _context.Add(novedadCompra);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                var compra = await _context.Compras.FindAsync(novedadCompra.IdCompra);
                if (compra != null)
                {
                    compra.Observaciones1 = "Programado";
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));

            }

            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", novedadCompra.IdCompra);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedadCompra.IdUsuario);
            return View(novedadCompra);
        }


        // GET: NovedadCompraUsuario
        public async Task<IActionResult> Index(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<NovedadCompra> query = _context.NovedadCompras
                .Include(nc => nc.IdUsuarioNavigation)
                .Include(nc => nc.IdCompraNavigation)
                    .ThenInclude(c => c.IdSolicitudNavigation)
                        .ThenInclude(s => s.IdProveedorNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(nc => nc.IdUsuarioNavigation.Username == userName);
                query = query.Where(nc => nc.Estado != "Programada" && nc.Estado != "Anulado" && nc.Estado != "Cerrada");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(nc => nc.Consecutivo.Contains(searchTerm));
                        break;
                    case "OrdenCompra":
                        query = query.Where(nc => nc.IdCompraNavigation.OrdenCompra.ToString().Contains(searchTerm));
                        break;
                    case "Empresa":
                        query = query.Where(nc => nc.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Estado":
                        query = query.Where(nc => nc.Estado.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(nc => nc.Fecha >= fechaInicio && nc.Fecha < fechaFin);
            }

            query = query.OrderByDescending(nc => nc.IdNovedadCompras);
            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadCompraIndexViewModel
            {
                NovedadesCM = paginatedItems,
                Pagination = new PaginationViewModelNovedadCompra
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // GET: NovedadCompraUsuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedadCompra = await _context.NovedadCompras
                .Include(n => n.IdCompraNavigation)
                .Include(n => n.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedadCompras == id);
            if (novedadCompra == null)
            {
                return NotFound();
            }

            return View(novedadCompra);
        }

        // POST: NovedadComprasUsuario / GenerarPDF /
        public async Task<IActionResult> GenerarPDF(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedad = await _context.NovedadCompras
                .Include(n => n.IdCompraNavigation)
                .Include(n => n.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedadCompras == id);
            if (novedad == null)
            {
                return NotFound();
            }

            // Retorna el PDF usando la vista `DetailsPDF`
            return new ViewAsPdf("NovedadPDF", novedad)
            {
                FileName = $"Novedad_{novedad.Consecutivo}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--disable-smart-shrinking"
            };
        }

        // GET: NovedadCompraUsuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedadCompra = await _context.NovedadCompras.FindAsync(id);
            if (novedadCompra == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
               .Where(u => u.IdUsuario == novedadCompra.IdUsuario)
               .Select(u => new { u.IdUsuario, u.Username })
               .FirstOrDefaultAsync();

            var compra = await _context.Compras
               .Where(c => c.IdCompra == novedadCompra.IdCompra)
               .Select(c => new { c.IdCompra, c.OrdenCompra })
               .FirstOrDefaultAsync();

            ViewData["IdCompra"] = new SelectList( new List<object> { new { compra.IdCompra, compra.OrdenCompra } }, "IdCompra", "OrdenCompra", novedadCompra.IdCompra);
            ViewData["IdUsuario"] = new SelectList(new List<object> { new { usuario.IdUsuario, usuario.Username } }, "IdUsuario", "Username", novedadCompra.IdUsuario);
            return View(novedadCompra);
        }

        // POST: NovedadCompraUsuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNovedadCompras,IdCompra,Fecha,TipoNovedad,FechaSalida,Empresa,Direccion,CiudadBarrio,IdUsuario,Contacto,Telefono,Observaciones,Consecutivo,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Estado,EstadoLogistica,ObservacionesLogistica")] NovedadCompra novedadCompra)
        {
            if (id != novedadCompra.IdNovedadCompras)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    novedadCompra.FechaActualizacion = DateTime.Now;
                    _context.Update(novedadCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NovedadCompraExists(novedadCompra.IdNovedadCompras))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", novedadCompra.IdCompra);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedadCompra.IdUsuario);
            return View(novedadCompra);
        }


        // GET: NovedadCompraUsuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedadCompra = await _context.NovedadCompras
                .Include(n => n.IdCompraNavigation)
                .Include(n => n.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedadCompras == id);
            if (novedadCompra == null)
            {
                return NotFound();
            }

            return View(novedadCompra);
        }

        // POST: NovedadCompraUsuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var novedadCompra = await _context.NovedadCompras.FindAsync(id);
            if (novedadCompra != null)
            {
                _context.NovedadCompras.Remove(novedadCompra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // TRACKING

        // GET: Tracking Index
        public async Task<IActionResult> TrackingIndex(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<NovedadCompra> query = _context.NovedadCompras
                .Include(n => n.IdCompraNavigation)
                .Include(n => n.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            //if (userRole == null ||
            //    (userRole != "Administrador" && userRole != "Super Usuario"
            //    && userRole != "Desarrollador" && userRole != "Soporte TI"))
            //{
            //    var userName = User.Identity.Name;
            //    query = query.Where(n => n.IdUsuarioNavigation.Username == userName);
            //    query = query.Where(n => n.Estado != "Anulado");
            //}

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(n => n.Consecutivo.Contains(searchTerm));
                        break;
                    case "Empresa":
                        query = query.Where(n => n.Empresa.Contains(searchTerm));
                        break;
                    case "Contacto":
                        query = query.Where(n => n.Contacto.Contains(searchTerm));
                        break;
                    case "Estado":
                        query = query.Where(n => n.Estado.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(n => n.Fecha >= fechaInicio && n.Fecha < fechaFin);
            }

            query = query.OrderByDescending(n => n.IdNovedadCompras);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadCompraTrackingViewModel
            {
                NovedadesCM = paginatedItems,
                Pagination = new PaginationViewModelNovedadCompraTracking
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        public async Task<IActionResult> TrackingDetails(int? idnovedad)
        {
            if (idnovedad == null)
            {
                return NotFound();
            }

            var novedad = await _context.NovedadCompras
               .Include(n => n.IdCompraNavigation)
               .Include(n => n.IdUsuarioNavigation)
               .FirstOrDefaultAsync(n => n.IdNovedadCompras == idnovedad);

            if (novedad == null)
            {
                return NotFound();
            }

            var ruta = await _context.Ruta
                .Include(r => r.IdUsuarioNavigation)
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                .FirstOrDefaultAsync(r => r.IdNovedadCompras == idnovedad);


            var viewModel = new LogisticaNCMViewModel
            {
                Novedades = novedad,
                Rutas = ruta
            };

            return View(viewModel);
        }


        // CONSULTE


        private bool NovedadCompraExists(int id)
        {
            return _context.NovedadCompras.Any(e => e.IdNovedadCompras == id);
        }
    }
}
