using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Management_system.Models;
using Rotativa.AspNetCore;
using Management_system.Models.Other.ViewModel.Logistica;
using ClosedXML.Excel;

namespace Management_system.Controllers
{
    public class NovedadComercialUsuarioController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public NovedadComercialUsuarioController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: NovedadComercialUsuario
        public async Task<IActionResult> Index(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<NovedadComercial> query = _context.NovedadComercials
                .Include(nc => nc.IdUsuarioNavigation)
                .Include(nc => nc.IdClienteNavigation);

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
                    case "NumeroDoc":
                        query = query.Where(nc => nc.NumeroDocumento.ToString().Contains(searchTerm));
                        break;
                    case "Cliente":
                        query = query.Where(nc => nc.IdClienteNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Usuario":
                        query = query.Where(nc => nc.IdUsuarioNavigation.Username.Contains(searchTerm));
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

            query = query.OrderByDescending(nc => nc.IdNovedadComercial);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadComercialIndexViewModel
            {
                NovedadesC = paginatedItems,
                Pagination = new PaginationViewModelNovedadComercial
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }


        // GET: NovedadComercialUsuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedadComercial = await _context.NovedadComercials
                .Include(n => n.IdClienteNavigation)
                .Include(n => n.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedadComercial == id);
            if (novedadComercial == null)
            {
                return NotFound();
            }

            return View(novedadComercial);
        }

        public async Task<IActionResult> GenerarPDF(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var novedadComercial = await _context.NovedadComercials
                    .Include(n => n.IdClienteNavigation)
                    .Include(n => n.IdUsuarioNavigation)
                    .FirstOrDefaultAsync(m => m.IdNovedadComercial == id);
                if (novedadComercial == null)
                {
                    return NotFound();
                }

                // Retorna el PDF usando la vista `DetailsPDF`
                return new ViewAsPdf("NovedadPDF", novedadComercial)
                {
                    FileName = $"Novedad_{novedadComercial.TipoDocumento}_{novedadComercial.NumeroDocumento}.pdf",
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 10, 10, 10),
                    CustomSwitches = "--disable-smart-shrinking"
                };
            }

        
        // GET: NovedadComercialUsuario/Create
        public IActionResult Create()
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userName = User.Identity.Name;

            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                // If the user does not have admin, developer or support roles, display only their user
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios.Where(u => u.Username == userName), "IdUsuario", "Username");
            }
            else
            {
                // If the user has admin, developer or support roles, display all users
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username");
            }


            var today = DateTime.Today;

            // Load the data for records created today by the current user
            var novedadesDeHoy = _context.NovedadComercials
                .Include(n => n.IdClienteNavigation)
                .Include(n => n.IdUsuarioNavigation)
                .Where(n => n.IdUsuarioNavigation.Username == userName &&
                            EF.Functions.DateDiffDay(n.Fecha, today) == 0)
                .ToList();


            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            ViewData["NovedadesDeHoy"] = novedadesDeHoy;

            return View();
        }

        // POST: NovedadComercialUsuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNovedadComercial,Fecha,TipoDocumento,NumeroDocumento,TipoServicio,FechaSalida,Empresa,Direccion,IdCliente,IdUsuario,TipoPago,Observaciones,Consecutivo,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Estado,EstadoLogistica,ObservacionesLogistica")] NovedadComercial novedadComercial)
        {
            if (ModelState.IsValid)
            {
                novedadComercial.Fecha = DateTime.Now;
                novedadComercial.Estado = "Abierta";
                _context.Add(novedadComercial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", novedadComercial.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedadComercial.IdUsuario);
            return View(novedadComercial);
        }

            // Customer search method
            [HttpGet]
            public JsonResult SearchCliente(string term)
            {
                var cliente = _context.Clientes
                    .Where(p => p.Nombre.Contains(term) || p.Nit.ToString().Contains(term))
                    .Select(p => new {
                        value = p.IdCliente,
                        text = p.Nit + " - " + p.Nombre
                    })
                    .ToList();

                return Json(cliente);
            }

            [HttpPost]
            public IActionResult DownloadNovedadesDeHoyPDF()
            {
                var userName = User.Identity.Name;
                var today = DateTime.Today;

                // Fetch today's NovedadComercial data for the active user
                var novedadesDeHoy = _context.NovedadComercials
                    .Include(n => n.IdClienteNavigation)
                    .Include(n => n.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdEmpresaNavigation)
                    .Include(n => n.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdAreaNavigation)
                    .Where(n => n.IdUsuarioNavigation.Username == userName &&
                                EF.Functions.DateDiffDay(n.Fecha, today) == 0)
                    .ToList();

                if (novedadesDeHoy.Count == 0)
                {
                    // No data for today, handle accordingly (e.g., show a message)
                    TempData["Message"] = "No hay novedades comerciales para hoy.";
                    return RedirectToAction("Create");
                }

                // Pass the data to the PDF view
                return new ViewAsPdf("NovedadesPDF", novedadesDeHoy)
                {
                    FileName = $"NovedadesComerciales_{DateTime.Now.ToString("yyyyMMdd")}.pdf",
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,  // Set to Landscape orientation
                    PageSize = Rotativa.AspNetCore.Options.Size.A4 // Optionally set paper size (A4, Letter, etc.)
                };
            }


        // GET: NovedadComercialUsuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedadComercial = await _context.NovedadComercials.FindAsync(id);
            if (novedadComercial == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Where(c => c.IdCliente == novedadComercial.IdCliente)
                .Select(c => new { c.IdCliente, c.Nombre })
                .FirstOrDefaultAsync();

            // Now we check which novelty is selected and load only the corresponding list.
            if (cliente != null)
            {
                ViewData["IdCliente"] = new SelectList(new List<object> { new { cliente.IdCliente, cliente.Nombre } }, "IdCliente", "Nombre", novedadComercial.IdCliente);
            }

            
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedadComercial.IdUsuario);
            return View(novedadComercial);
        }

        // POST: NovedadComercialUsuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNovedadComercial,Fecha,TipoDocumento,NumeroDocumento,TipoServicio,FechaSalida,Empresa,Direccion,IdCliente,IdUsuario,TipoPago,Observaciones,Consecutivo,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Estado,EstadoLogistica,ObservacionesLogistica")] NovedadComercial novedadComercial)
        {
            if (id != novedadComercial.IdNovedadComercial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    novedadComercial.FechaActualizacion = DateTime.Now;
                    novedadComercial.Estado = "Abierta";
                    _context.Update(novedadComercial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NovedadComercialExists(novedadComercial.IdNovedadComercial))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", novedadComercial.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedadComercial.IdUsuario);
            return View(novedadComercial);
        }

        // Customer search method
        [HttpGet]
        public JsonResult SearchClienteEdit(string term)
        {
            var cliente = _context.Clientes
                .Where(c => c.Nombre.Contains(term))
                .Select(c => new {
                    value = c.IdCliente,
                    text = c.Nombre
                })
                .ToList();

            return Json(cliente);
        }


        // GET: NovedadComercialUsuario/EditReview/5
        public async Task<IActionResult> EditReview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedadComercial = await _context.NovedadComercials.FindAsync(id);
            if (novedadComercial == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Where(c => c.IdCliente == novedadComercial.IdCliente)
                .Select(c => new { c.IdCliente, c.Nombre })
                .FirstOrDefaultAsync();


            ViewData["IdCliente"] = new SelectList(new List<object> { new { cliente.IdCliente, cliente.Nombre } }, "IdCliente", "Nombre", novedadComercial.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedadComercial.IdUsuario);
            return View(novedadComercial);
        }

        // POST: NovedadComercialUsuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(int id, [Bind("IdNovedadComercial,Fecha,TipoDocumento,NumeroDocumento,TipoServicio,FechaSalida,Empresa,Direccion,IdCliente,IdUsuario,TipoPago,Observaciones,Consecutivo,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Estado,EstadoLogistica,ObservacionesLogistica")] NovedadComercial novedadComercial)
        {
            if (id != novedadComercial.IdNovedadComercial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    novedadComercial.FechaActualizacion = DateTime.Now;
                    novedadComercial.Estado = "Editada";
                    _context.Update(novedadComercial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NovedadComercialExists(novedadComercial.IdNovedadComercial))
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
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", novedadComercial.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedadComercial.IdUsuario);
            return View(novedadComercial);
        }


            // Método para la búsqueda de cliente
            [HttpGet]
            public JsonResult SearchClienteEditReview(string term)
            {
                var cliente = _context.Clientes
                    .Where(c => c.Nombre.Contains(term))
                    .Select(c => new {
                        value = c.IdCliente,
                        text = c.Nombre
                    })
                    .ToList();

                return Json(cliente);
            }


        // GET: NovedadComercialUsuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedadComercial = await _context.NovedadComercials
                .Include(n => n.IdClienteNavigation)
                .Include(n => n.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedadComercial == id);
            if (novedadComercial == null)
            {
                return NotFound();
            }

            return View(novedadComercial);
        }

        // POST: NovedadComercialUsuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var novedadComercial = await _context.NovedadComercials.FindAsync(id);
            if (novedadComercial != null)
            {
                _context.NovedadComercials.Remove(novedadComercial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create));
        }


        // TRACKING

        // GET: Tracking Index
        public async Task<IActionResult> TrackingIndex(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<NovedadComercial> query = _context.NovedadComercials
                .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdAreaNavigation)
                .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(nc => nc.IdClienteNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(nc => nc.IdUsuarioNavigation.Username == userName);
                query = query.Where(nc => nc.Estado != "Anulado");
                //query = query.Where(nc => nc.Estado != "Programada" && nc.Estado != "Anulado" && nc.Estado != "Cerrada");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(nc => nc.Consecutivo.Contains(searchTerm));
                        break;
                    case "NumeroDocumento":
                        query = query.Where(nc => nc.NumeroDocumento.ToString().Contains(searchTerm));
                        break;
                    case "Cliente":
                        query = query.Where(nc => nc.IdClienteNavigation.Nombre.Contains(searchTerm));
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
                query = query.Where(nc => nc.Fecha >= fechaInicio && nc.Fecha < fechaFin);
            }

            query = query.OrderByDescending(nc => nc.IdNovedadComercial);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadComercialTrackingViewModel
            {
                NovedadesC = paginatedItems,
                Pagination = new PaginationViewModelNovedadComercialTracking
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

            var novedad = await _context.NovedadComercials
               .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdAreaNavigation)
                .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(nc => nc.IdClienteNavigation)
               .FirstOrDefaultAsync(n => n.IdNovedadComercial == idnovedad);

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
                .FirstOrDefaultAsync(r => r.IdNovedadComercial == idnovedad);


            var viewModel = new LogisticaNCViewModel
            {
                Novedades = novedad,
                Rutas = ruta
            };

            return View(viewModel);
        }


        // OVERRIDE

        // GET: Override Index
        public async Task<IActionResult> OverrideIndex(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<NovedadComercial> query = _context.NovedadComercials
                .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdAreaNavigation)
                .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(nc => nc.IdClienteNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(nc => nc.IdUsuarioNavigation.Username == userName);
                query = query.Where(nc => nc.Estado != "Anulado");
                //query = query.Where(nc => nc.Estado != "Programada" && nc.Estado != "Anulado" && nc.Estado != "Cerrada");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(nc => nc.Consecutivo.Contains(searchTerm));
                        break;
                    case "NumeroDocumento":
                        query = query.Where(nc => nc.NumeroDocumento.ToString().Contains(searchTerm));
                        break;
                    case "Cliente":
                        query = query.Where(nc => nc.IdClienteNavigation.Nombre.Contains(searchTerm));
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
                query = query.Where(nc => nc.Fecha >= fechaInicio && nc.Fecha < fechaFin);
            }

            query = query.OrderBy(nc => nc.IdNovedadComercial);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadComercialOverrideViewModel
            {
                NovedadesC = paginatedItems,
                Pagination = new PaginationViewModelNovedadComercialOverride
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }


        // GET: NovedadComercialUsuario/OverrideEdit/5
        public async Task<IActionResult> OverrideEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedadComercial = await _context.NovedadComercials.FindAsync(id);
            if (novedadComercial == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Where(c => c.IdCliente == novedadComercial.IdCliente)
                .Select(c => new { c.IdCliente, c.Nombre })
                .FirstOrDefaultAsync();


            ViewData["IdCliente"] = new SelectList(new List<object> { new { cliente.IdCliente, cliente.Nombre } }, "IdCliente", "Nombre", novedadComercial.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedadComercial.IdUsuario);
            return View(novedadComercial);
        }

        // POST: NovedadComercialUsuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OverrideEdit(int id, [Bind("IdNovedadComercial,Fecha,TipoDocumento,NumeroDocumento,TipoServicio,FechaSalida,Empresa,Direccion,IdCliente,IdUsuario,TipoPago,Observaciones,Consecutivo,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Estado,EstadoLogistica,ObservacionesLogistica")] NovedadComercial novedadComercial)
        {
            if (id != novedadComercial.IdNovedadComercial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    novedadComercial.Fecha = DateTime.Now;
                    novedadComercial.Estado = "Anulado";
                    _context.Update(novedadComercial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NovedadComercialExists(novedadComercial.IdNovedadComercial))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(OverrideIndex));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", novedadComercial.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedadComercial.IdUsuario);
            return View(novedadComercial);
        }


        // CONSULTE

        // GET: Consulte Review
        public async Task<IActionResult> ConsulteReview(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<NovedadComercial> query = _context.NovedadComercials
                .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdAreaNavigation)
                .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(nc => nc.IdClienteNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(nc => nc.IdUsuarioNavigation.Username == userName);
                query = query.Where(nc => nc.Estado != "Anulado");
                //query = query.Where(nc => nc.Estado != "Programada" && nc.Estado != "Anulado" && nc.Estado != "Cerrada");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(nc => nc.Consecutivo.Contains(searchTerm));
                        break;
                    case "NumeroDocumento":
                        query = query.Where(nc => nc.NumeroDocumento.ToString().Contains(searchTerm));
                        break;
                    case "Cliente":
                        query = query.Where(nc => nc.IdClienteNavigation.Nombre.Contains(searchTerm));
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
                query = query.Where(nc => nc.Fecha >= fechaInicio && nc.Fecha < fechaFin);
            }

            query = query.OrderByDescending(nc => nc.IdNovedadComercial);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadComercialConsulteViewModel
            {
                NovedadesC = paginatedItems,
                Pagination = new PaginationViewModelNovedadComercialConsulte
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // POST: Method by dowload Excel the data in the list ConsulteReview
        // POST: Method by dowload Excel the data in the list ConsulteReview
        public IActionResult ExportExcelConsulteR()
        {
            var userRole = User.FindFirst("Rol")?.Value;
            var userName = User.Identity.Name;

            IQueryable<NovedadComercial> query = _context.NovedadComercials
                .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdAreaNavigation)
                .Include(nc => nc.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(nc => nc.IdClienteNavigation);

            // Filtrar según el rol del usuario
            if (userRole != "Administrador" && userRole != "Super Usuario" &&
                userRole != "Gerencia" && userRole != "Desarrollador" &&
                userRole != "Soporte TI")
            {
                query = query.Where(nc => nc.IdUsuarioNavigation.Username == userName);
            }

            query = query.OrderBy(nc => nc.IdNovedadComercial);
            var novedades = query.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Novedad Comercial");

                // Título centrado y combinado en las columnas A-N
                var titleCell = worksheet.Range("A1:N1").Merge();
                titleCell.Value = "Novedades Comerciales";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.Black;

                // Encabezados de columna
                var headers = new string[]
                {
                    "Id Novedad", "Fecha Creación", "Tipo Documento", "Numero Documento",
                    "Tipo Servicio", "Fecha Salida", "Empresa", "Cliente", "Dirección",
                    "Solicitado Por", "Área", "Pago", "Estado", "Observaciones"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(2, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.Black;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                // Rellenar datos desde la fila 3
                int row = 3;
                foreach (var novedad in novedades)
                {
                    worksheet.Cell(row, 1).Value = novedad.IdNovedadComercial;
                    worksheet.Cell(row, 2).Value = novedad.Fecha?.ToString("yyyy/MM/dd");
                    worksheet.Cell(row, 3).Value = novedad.TipoDocumento;
                    worksheet.Cell(row, 4).Value = novedad.NumeroDocumento;
                    worksheet.Cell(row, 5).Value = novedad.TipoServicio;
                    worksheet.Cell(row, 6).Value = novedad.FechaSalida?.ToString("yyyy/MM/dd");
                    worksheet.Cell(row, 7).Value = novedad.IdUsuarioNavigation.IdEmpresaNavigation.Nombre;
                    worksheet.Cell(row, 8).Value = novedad.IdClienteNavigation != null ? novedad.IdClienteNavigation.Nombre : novedad.Empresa;
                    worksheet.Cell(row, 9).Value = novedad.Direccion;
                    worksheet.Cell(row, 10).Value = novedad.IdUsuarioNavigation.Nombre;
                    worksheet.Cell(row, 11).Value = novedad.IdUsuarioNavigation.IdAreaNavigation.Nombre;
                    worksheet.Cell(row, 12).Value = novedad.TipoPago;
                    worksheet.Cell(row, 13).Value = novedad.Estado;
                    worksheet.Cell(row, 14).Value = novedad.Observaciones1;

                    // Aplicar bordes a las celdas de la fila actual
                    for (int col = 1; col <= 14; col++)
                    {
                        worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    row++;
                }

                // Ajustar automáticamente el ancho de las columnas
                worksheet.Columns().AdjustToContents();

                // Guardar y devolver el archivo
                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Novedades_Resumen_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }

        // GET: NovedadComercialUsuae
        public IActionResult ConsulteNovedad()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchResults(SearchCViewModel model)
        {
            // Validar que los parámetros de búsqueda no sean nulos
            if (model.Fecha == null)
            {
                TempData["Error"] = "Por favor, seleccione una fecha";
                return RedirectToAction("ConsulteNovedad");
            }

            var fechaInicio = model.Fecha.Date;
            var fechaFin = model.Fecha.Date.AddDays(1).AddTicks(-1);

            var NovedadesFiltradas = _context.NovedadComercials
                .Include(ncm => ncm.IdClienteNavigation)
                .Include(ncm => ncm.IdUsuarioNavigation)
                    .ThenInclude(ncm => ncm.IdEmpresaNavigation)
                .Include(ncm => ncm.IdUsuarioNavigation)
                    .ThenInclude(ncm => ncm.IdAreaNavigation)
                .Where(ncm => ncm.Fecha >= fechaInicio && ncm.Fecha <= fechaFin)
                .ToList();

            if (NovedadesFiltradas.Count == 0)
            {
                TempData["Message"] = "No se encontraron rutas para la fecha y conductor seleccionados.";
                return RedirectToAction("GenerarRuta");
            }

            // Generar el PDF usando Rotativa.AspNetCore
            return new ViewAsPdf("ConsulteNovedadPDF", NovedadesFiltradas)
            {
                FileName = $"Novedades_{DateTime.Now.ToString("yyyyMMdd")}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }


        private bool NovedadComercialExists(int id)
        {
            return _context.NovedadComercials.Any(e => e.IdNovedadComercial == id);
        }
    }
}
