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
using DocumentFormat.OpenXml.Drawing.Diagrams;
using ClosedXML.Excel;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Management_system.Controllers
{
    public class RutaUsuarioController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public RutaUsuarioController(DbManagementSystemContext context)
        {
            _context = context;
        }


        // GET: RutaUsuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Logistica"))
            {
                return RedirectToAction("Index", "Main");
            }


            if (id == null)
            {
                return NotFound();
            }

            var rutum = await _context.Ruta
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdRuta == id);
            if (rutum == null)
            {
                return NotFound();
            }

            return View(rutum);
        }


        // GET: RutaUsuario/GenerarRuta/5
        public IActionResult GenerarRuta()
        {
            var viewModel = new SearchViewModel
            {
                Conductores = _context.Conductors
                .OrderBy(c => c.Nombre)
                .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SearchResults(SearchViewModel model)
        {
            // Validar que los parámetros de búsqueda no sean nulos
            if (model.FechaSalida == null || model.IdConductor == null)
            {
                TempData["Error"] = "Por favor, seleccione una fecha y un conductor.";
                return RedirectToAction("GenerarRuta");
            }

            // Realizar la consulta filtrando por la fecha y el conductor
            var rutasFiltradas = _context.Ruta
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncp => ncp.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncp => ncp.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdAreaNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(ncm => ncm.IdClienteNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(ncm => ncm.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(ncm => ncm.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdAreaNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                    .ThenInclude(n => n.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                    .ThenInclude(n => n.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdAreaNavigation)
                .Where(r => r.FechaRuta == model.FechaSalida && r.IdConductor == model.IdConductor)
                .ToList();

            if (rutasFiltradas.Count == 0)
            {
                TempData["Message"] = "No se encontraron rutas para la fecha y conductor seleccionados.";
                return RedirectToAction("GenerarRuta");
            }

            // Generar el PDF usando Rotativa.AspNetCore
            return new ViewAsPdf("GenerarRutaPDF", rutasFiltradas)
            {
                FileName = $"Rutas_{DateTime.Now.ToString("yyyyMMdd")}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        // GET: RutaUsuario/Create
        public IActionResult Create()
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Logistica"))
            {
                return RedirectToAction("Index", "Main");
            }


            // Verify if the user has the appropriate roles
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                // If the user does not have admin, developer or support roles, display only their user
                var userName = User.Identity.Name;
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios.Where(u => u.Username == userName), "IdUsuario", "Username");
            }
            else
            {
                // If the user has admin, developer or support roles, display all users
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username");
            }


            ViewData["IdConductor"] = new SelectList(
            _context.Conductors.OrderBy(c => c.Nombre),
            "IdConductor",
            "Nombre"
);
            ViewData["IdEstado"] = new SelectList(_context.EstadoEntregas, "IdEstado", "Nombre");
            ViewData["IdNovedadComercial"] = new SelectList(_context.NovedadComercials, "IdNovedadComercial", "NumeroDocumento");
            ViewData["IdNovedadCompras"] = new SelectList(_context.NovedadCompras, "IdNovedadCompras", "Consecutivo");
            ViewData["IdNovedadGeneral"] = new SelectList(_context.Novedads, "IdNovedad", "Consecutivo");

            return View();
        }

        // POST: RutaUsuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRuta,FechaAsignacion,IdUsuario,IdNovedadCompras,IdNovedadComercial,IdNovedadGeneral,IdConductor,Valor,Recibido,NGuia,IdEstado,Causa,Observaciones,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Campo1,Campo2,Campo3,Campo4,Campo5,Campo6,Estado,EstadoLogistica,ObservacionesLogistica,FechaRuta")] Rutum rutum)
        {
            if (ModelState.IsValid)
            {
                rutum.FechaAsignacion = DateTime.Now;
                rutum.Estado = "Abierta";

                _context.Add(rutum);
                await _context.SaveChangesAsync();

                // Validate that only one of the novelty Ids is selected
                if ((rutum.IdNovedadGeneral != null && (rutum.IdNovedadComercial != null || rutum.IdNovedadCompras != null)) ||
                    (rutum.IdNovedadComercial != null && rutum.IdNovedadCompras != null))
                {
                    ModelState.AddModelError("", "Solo puede seleccionar un tipo de novedad.");
                    return View(rutum);
                }

                // Change the status of the selected novelty
                await CambiarEstadoNovedad(rutum);

                TempData["Success"] = true;
                return RedirectToAction(nameof(Create));
            }

            ViewData["IdConductor"] = new SelectList(_context.Conductors, "IdConductor", "IdConductor", rutum.IdConductor);
            ViewData["IdEstado"] = new SelectList(_context.EstadoEntregas, "IdEstado", "IdEstado", rutum.IdEstado);
            ViewData["IdNovedadComercial"] = new SelectList(_context.NovedadComercials, "IdNovedadComercial", "IdNovedadComercial", rutum.IdNovedadComercial);
            ViewData["IdNovedadCompras"] = new SelectList(_context.NovedadCompras, "IdNovedadCompras", "IdNovedadCompras", rutum.IdNovedadCompras);
            ViewData["IdNovedadGeneral"] = new SelectList(_context.Novedads, "IdNovedad", "IdNovedad", rutum.IdNovedadGeneral);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", rutum.IdUsuario);
            return View(rutum);
        }

        private async Task CambiarEstadoNovedad(Rutum rutum)
        {
            // Si se seleccionó Novedad General
            if (rutum.IdNovedadGeneral != null)
            {
                var novedadGeneral = await _context.Novedads.FindAsync(rutum.IdNovedadGeneral);
                if (novedadGeneral != null)
                {
                    novedadGeneral.Estado = "Programada";
                    novedadGeneral.EstadoLogistica = "Programada";
                    _context.Update(novedadGeneral);
                    await _context.SaveChangesAsync();
                }
            }
            // Si se seleccionó Novedad Comercial
            else if (rutum.IdNovedadComercial != null)
            {
                var novedadComercial = await _context.NovedadComercials.FindAsync(rutum.IdNovedadComercial);
                if (novedadComercial != null)
                {
                    novedadComercial.Estado = "Programada";
                    novedadComercial.EstadoLogistica = "Programada";
                    _context.Update(novedadComercial);
                    await _context.SaveChangesAsync();
                }
            }
            // Si se seleccionó Novedad de Compras
            else if (rutum.IdNovedadCompras != null)
            {
                var novedadCompra = await _context.NovedadCompras.FindAsync(rutum.IdNovedadCompras);
                if (novedadCompra != null)
                {
                    novedadCompra.Estado = "Programada";
                    novedadCompra.EstadoLogistica = "Programada";
                    _context.Update(novedadCompra);
                    await _context.SaveChangesAsync();
                }
            }
        }


        // NovedadGeneral search method
        [HttpGet]
        public JsonResult SearchNovedadGeneral(string term)
        {
            var estadosPermitidos = new[] { "Abierta", "Urgente", "Editada" };

            var novedadGeneral = _context.Novedads
                .Where(n => estadosPermitidos.Contains(n.Estado) && n.Consecutivo.Contains(term))
                .Select(n => new {
                    value = n.IdNovedad,
                    text = n.Consecutivo
                })
                .ToList();

            return Json(novedadGeneral);
        }

        public IActionResult GetNovedadGeneralCliente(int id)
        {
            var novedadG = _context.Novedads
                .Include(n => n.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdEmpresaNavigation)
                .FirstOrDefault(n => n.IdNovedad == id);

            if (novedadG == null)
            {
                return NotFound();
            }

            return Json(new { 
                description = novedadG.Empresa,
                empresa = novedadG.IdUsuarioNavigation.IdEmpresaNavigation.Nombre,
                solicitado = novedadG.IdUsuarioNavigation.Nombre
            });
        }


        // NovedadComercial search method
        [HttpGet]
        public JsonResult SearchNovedadComercial(string term)
        {
            var estadosPermitidos = new[] { "Abierta", "Urgente", "Editada" };

            var novedadComercial = _context.NovedadComercials
                .Where(nc => estadosPermitidos.Contains(nc.Estado) && nc.NumeroDocumento.ToString().Contains(term))
                .Select(nc => new {
                    value = nc.IdNovedadComercial,
                    text = nc.NumeroDocumento
                })
                .ToList();

            return Json(novedadComercial);
        }

        public IActionResult GetNovedadComercialCliente(int id)
        {
            var novedadCM = _context.NovedadComercials
                .Include(n => n.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(n => n.IdClienteNavigation)
                .FirstOrDefault(n => n.IdNovedadComercial == id);

            if (novedadCM == null)
            {
                return NotFound();
            }

            // Definir las variables antes de construir el objeto Json
            string description1;
            if (novedadCM.IdClienteNavigation != null)
            {
                description1 = novedadCM.IdClienteNavigation.Nombre;
            }
            else
            {
                description1 = novedadCM.Empresa;
            }

            // Construir y retornar el Json
            return Json(new
            {
                description1 = description1, // Usamos la variable definida
                description2 = novedadCM.Direccion,
                tipodocumento = novedadCM.TipoDocumento,
                empresa1 = novedadCM.IdUsuarioNavigation.IdEmpresaNavigation.Nombre,
                solicitado1 = novedadCM.IdUsuarioNavigation.Nombre
            });

        }


        // NovedadCompras search method
        [HttpGet]
        public JsonResult SearchNovedadCompras(string term)
        {
            var estadosPermitidos = new[] { "Abierta", "Urgente", "Editada" };

            var novedadCompras = _context.NovedadCompras
                .Where(cp => estadosPermitidos.Contains(cp.Estado) && cp.Consecutivo.Contains(term))
                .Select(cp => new {
                    value = cp.IdNovedadCompras,
                    text = cp.Consecutivo
                })
                .ToList();

            return Json(novedadCompras);
        }

        public IActionResult GetNovedadCompraCliente(int id)
        {
            var novedadCP = _context.NovedadCompras
                .Include(n => n.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdEmpresaNavigation)
                    .Include(n => n.IdCompraNavigation)
                .FirstOrDefault(n => n.IdNovedadCompras == id);

            if (novedadCP == null)
            {
                return NotFound();
            }

            return Json(new
            {
                description = novedadCP.Empresa,
                empresa = novedadCP.IdUsuarioNavigation.IdEmpresaNavigation.Nombre,
                solicitado = novedadCP.IdUsuarioNavigation.Nombre
            });
        }



        // GET: RutaUsuario / Review
        public async Task<IActionResult> ReviewIndex(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Logistica"))
            {
                return RedirectToAction("Logistica", "Main");
            }

            IQueryable<Rutum> query = _context.Ruta
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                    .ThenInclude(n => n.IdUsuarioNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdClienteNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdUsuarioNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdCompraNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdUsuarioNavigation)
                .Include(r => r.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                //query = query.Where(r=> r.IdUsuarioNavigation.Username == userName);
                query = query.Where(r => r.Estado != "Anulado" && r.Estado != "Cerrada");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Comercial":
                        query = query.Where(r => r.IdNovedadComercialNavigation.NumeroDocumento.ToString().Contains(searchTerm));
                        break;
                    case "Compras":
                        query = query.Where(r => r.IdNovedadComprasNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Novedad":
                        query = query.Where(r => r.IdNovedadGeneralNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Estado":
                        query = query.Where(r => r.Estado.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(r => r.FechaAsignacion >= fechaInicio && r.FechaAsignacion < fechaFin);
            }

            query = query.OrderBy(n => n.IdRuta);
            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new RutaReviewIndexViewModel
            {
                Rutas = paginatedItems,
                Pagination = new PaginationViewModelRutaReviewIndex
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);

        }

        // GET: RutaUsuario/Details/5
        public async Task<IActionResult> ReviewDetails(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Logistica"))
            {
                return RedirectToAction("Logistica", "Main");
            }


            if (id == null)
            {
                return NotFound();
            }

            var rutum = await _context.Ruta
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdRuta == id);
            if (rutum == null)
            {
                return NotFound();
            }

            return View(rutum);
        }


        // GET: RutaUsuario/Edit/5
        public async Task<IActionResult> ReviewEdit(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Logistica"))
            {
                return RedirectToAction("Index", "Main");
            }

            if (id == null)
            {
                return NotFound();
            }

            var rutum = await _context.Ruta.FindAsync(id);
            if (rutum == null)
            {
                return NotFound();
            }


            // We always charge the driver and the state
            ViewData["IdConductor"] = new SelectList(_context.Conductors, "IdConductor", "Nombre", rutum.IdConductor);
            ViewData["IdEstado"] = new SelectList(_context.EstadoEntregas, "IdEstado", "Nombre", rutum.IdEstado);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username", rutum.IdUsuario);


            // Now we check which novelty is selected and load only the corresponding list.
            if (rutum.IdNovedadGeneral != null)
            {
                var novedadGeneral = await _context.Novedads
                .Where(ng => ng.IdNovedad == rutum.IdNovedadGeneral)
                .Select(ng => new { ng.IdNovedad, ng.Consecutivo, ng.Empresa, ng.IdUsuarioNavigation.Nombre })
                .FirstOrDefaultAsync();

                ViewData["IdNovedadGeneral"] = new SelectList(new List<object> { new { novedadGeneral.IdNovedad, novedadGeneral.Consecutivo } }, "IdNovedad", "Consecutivo", rutum.IdNovedadGeneral);
                ViewData["NovedadGeneralEmpresa"] = novedadGeneral.Empresa;
                ViewData["NovedadGeneralUsuario"] = novedadGeneral.Nombre;

            }
            else if (rutum.IdNovedadComercial != null)
            {
                var novedadComercial = await _context.NovedadComercials
                .Where(nc => nc.IdNovedadComercial == rutum.IdNovedadComercial)
                .Select(nc => new { nc.IdNovedadComercial, nc.NumeroDocumento, nc.Empresa, nc.IdClienteNavigation.Nombre, nc.IdUsuarioNavigation.Username })
                .FirstOrDefaultAsync();

                ViewData["IdNovedadComercial"] = new SelectList(new List<object> { new { novedadComercial.IdNovedadComercial, novedadComercial.NumeroDocumento } }, "IdNovedadComercial", "NumeroDocumento", rutum.IdNovedadComercial);
                if (novedadComercial.Nombre != null)
                {
                    ViewData["NovedadComercialEmpresa"] = novedadComercial.Nombre;
                }
                else
                {
                    ViewData["NovedadComercialEmpresa"] = novedadComercial.Empresa;
                }

                ViewData["NovedadComercialUsuario"] = novedadComercial.Username;

            }
            else if (rutum.IdNovedadCompras != null)
            {
                var novedadCompra = await _context.NovedadCompras
                .Where(ncm => ncm.IdNovedadCompras == rutum.IdNovedadCompras)
                .Select(ncm => new { ncm.IdNovedadCompras, ncm.Consecutivo, ncm.Empresa, ncm.IdUsuarioNavigation.Nombre })
                .FirstOrDefaultAsync();

                ViewData["IdNovedadCompras"] = new SelectList(new List<object> { new { novedadCompra.IdNovedadCompras, novedadCompra.Consecutivo } }, "IdNovedadCompras", "Consecutivo", rutum.IdNovedadCompras);
                ViewData["NovedadComprasEmpresa"] = novedadCompra.Empresa;
                ViewData["NovedadComprasUsuario"] = novedadCompra.Nombre;
            }

            return View(rutum);
        }

        // POST: RutaUsuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewEdit(int id, [Bind("IdRuta,FechaAsignacion,IdUsuario,IdNovedadCompras,IdNovedadComercial,IdNovedadGeneral,IdConductor,Valor,Recibido,NGuia,IdEstado,Causa,Observaciones,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Campo1,Campo2,Campo3,Campo4,Campo5,Campo6,Estado,EstadoLogistica,ObservacionesLogistica,FechaRuta")] Rutum rutum)
        {
            if (id != rutum.IdRuta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rutum.FechaActualizacion = DateTime.Now;
                    _context.Update(rutum);
                    await _context.SaveChangesAsync();

                    if (rutum.Estado == "Cerrada")
                    {
                        await CambiarEstadoNovedadEdit(rutum);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RutumExists(rutum.IdRuta))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(ReviewIndex));

            }
            ViewData["IdConductor"] = new SelectList(_context.Conductors, "IdConductor", "IdConductor", rutum.IdConductor);
            ViewData["IdEstado"] = new SelectList(_context.EstadoEntregas, "IdEstado", "IdEstado", rutum.IdEstado);
            ViewData["IdNovedadComercial"] = new SelectList(_context.NovedadComercials, "IdNovedadComercial", "IdNovedadComercial", rutum.IdNovedadComercial);
            ViewData["IdNovedadCompras"] = new SelectList(_context.NovedadCompras, "IdNovedadCompras", "IdNovedadCompras", rutum.IdNovedadCompras);
            ViewData["IdNovedadGeneral"] = new SelectList(_context.Novedads, "IdNovedad", "IdNovedad", rutum.IdNovedadGeneral);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", rutum.IdUsuario);
            return View(rutum);
        }


        // GET: RutaUsuario
        public async Task<IActionResult> Index(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Logistica"))
            {
                return RedirectToAction("Logistica", "Main");
            }

            IQueryable<Rutum> query = _context.Ruta
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                    .ThenInclude(n => n.IdUsuarioNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdClienteNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdUsuarioNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdCompraNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdUsuarioNavigation)
                .Include(r => r.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                //query = query.Where(r=> r.IdUsuarioNavigation.Username == userName);
                query = query.Where(r => r.Estado != "Anulado" && r.Estado != "Cerrada");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Comercial":
                        query = query.Where(r => r.IdNovedadComercialNavigation.NumeroDocumento.ToString().Contains(searchTerm));
                        break;
                    case "Compras":
                        query = query.Where(r => r.IdNovedadComprasNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Novedad":
                        query = query.Where(r => r.IdNovedadGeneralNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Estado":
                        query = query.Where(r => r.Estado.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(r => r.FechaAsignacion >= fechaInicio && r.FechaAsignacion < fechaFin);
            }

            query = query.OrderBy(n => n.IdRuta);
            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new RutaIndexViewModel
            {
                Rutas = paginatedItems,
                Pagination = new PaginationViewModelRutaIndex
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);


        }

        // GET: RutaUsuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Logistica"))
            {
                return RedirectToAction("Index", "Main");
            }

            if (id == null)
            {
                return NotFound();
            }

            var rutum = await _context.Ruta.FindAsync(id);
            if (rutum == null)
            {
                return NotFound();
            }

            var conductor = await _context.Conductors
                .Where(c => c.IdConductor == rutum.IdConductor)
                .Select(c => new { c.IdConductor, c.Nombre })
                .FirstOrDefaultAsync();

            // We always charge the driver and the state
            ViewData["IdConductor"] = new SelectList(new List<object> { new { conductor.IdConductor, conductor.Nombre } }, "IdConductor", "Nombre", rutum.IdConductor);
            ViewData["IdEstado"] = new SelectList(_context.EstadoEntregas, "IdEstado", "Nombre", rutum.IdEstado);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username", rutum.IdUsuario);


            // Now we check which novelty is selected and load only the corresponding list.
            if (rutum.IdNovedadGeneral != null)
            {
                var novedadGeneral = await _context.Novedads
                .Where(ng => ng.IdNovedad == rutum.IdNovedadGeneral)
                .Select(ng => new { ng.IdNovedad, ng.Consecutivo, ng.Empresa, ng.IdUsuarioNavigation.Nombre })
                .FirstOrDefaultAsync();

                ViewData["IdNovedadGeneral"] = new SelectList(new List<object> { new { novedadGeneral.IdNovedad, novedadGeneral.Consecutivo } }, "IdNovedad", "Consecutivo", rutum.IdNovedadGeneral);
                ViewData["NovedadGeneralEmpresa"] = novedadGeneral.Empresa;
                ViewData["NovedadGeneralUsuario"] = novedadGeneral.Nombre;

            }
            else if (rutum.IdNovedadComercial != null)
            {
                var novedadComercial = await _context.NovedadComercials
                .Where(nc => nc.IdNovedadComercial == rutum.IdNovedadComercial)
                .Select(nc => new { nc.IdNovedadComercial, nc.NumeroDocumento, nc.Empresa, nc.IdClienteNavigation.Nombre, nc.IdUsuarioNavigation.Username })
                .FirstOrDefaultAsync();

                ViewData["IdNovedadComercial"] = new SelectList(new List<object> { new { novedadComercial.IdNovedadComercial, novedadComercial.NumeroDocumento} }, "IdNovedadComercial", "NumeroDocumento", rutum.IdNovedadComercial);
                if (novedadComercial.Nombre != null)
                {
                    ViewData["NovedadComercialEmpresa"] = novedadComercial.Nombre;
                }
                else 
                {
                    ViewData["NovedadComercialEmpresa"] = novedadComercial.Empresa;
                }

                ViewData["NovedadComercialUsuario"] = novedadComercial.Username;

            }
            else if (rutum.IdNovedadCompras != null)
            {
                var novedadCompra = await _context.NovedadCompras
                .Where(ncm => ncm.IdNovedadCompras == rutum.IdNovedadCompras)
                .Select(ncm => new { ncm.IdNovedadCompras, ncm.Consecutivo, ncm.Empresa, ncm.IdUsuarioNavigation.Nombre })
                .FirstOrDefaultAsync();

                ViewData["IdNovedadCompras"] = new SelectList(new List<object> { new { novedadCompra.IdNovedadCompras, novedadCompra.Consecutivo} }, "IdNovedadCompras", "Consecutivo", rutum.IdNovedadCompras);
                ViewData["NovedadComprasEmpresa"] = novedadCompra.Empresa;
                ViewData["NovedadComprasUsuario"] = novedadCompra.Nombre;
            }

            return View(rutum);
        }

        // POST: RutaUsuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRuta,FechaAsignacion,IdUsuario,IdNovedadCompras,IdNovedadComercial,IdNovedadGeneral,IdConductor,Valor,Recibido,NGuia,IdEstado,Causa,Observaciones,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Campo1,Campo2,Campo3,Campo4,Campo5,Campo6,Estado,EstadoLogistica,ObservacionesLogistica,FechaRuta")] Rutum rutum)
        {
            if (id != rutum.IdRuta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rutum.FechaCierre = DateTime.Now;
                    _context.Update(rutum);
                    await _context.SaveChangesAsync();

                    if (rutum.Estado == "Cerrada")
                    {
                        await CambiarEstadoNovedadEdit(rutum);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RutumExists(rutum.IdRuta))
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
            ViewData["IdConductor"] = new SelectList(_context.Conductors, "IdConductor", "IdConductor", rutum.IdConductor);
            ViewData["IdEstado"] = new SelectList(_context.EstadoEntregas, "IdEstado", "IdEstado", rutum.IdEstado);
            ViewData["IdNovedadComercial"] = new SelectList(_context.NovedadComercials, "IdNovedadComercial", "IdNovedadComercial", rutum.IdNovedadComercial);
            ViewData["IdNovedadCompras"] = new SelectList(_context.NovedadCompras, "IdNovedadCompras", "IdNovedadCompras", rutum.IdNovedadCompras);
            ViewData["IdNovedadGeneral"] = new SelectList(_context.Novedads, "IdNovedad", "IdNovedad", rutum.IdNovedadGeneral);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", rutum.IdUsuario);
            return View(rutum);
        }

        private async Task CambiarEstadoNovedadEdit(Rutum rutum)
        {
            // Si se seleccionó Novedad General
            if (rutum.IdNovedadGeneral != null)
            {
                var novedadGeneral = await _context.Novedads.FindAsync(rutum.IdNovedadGeneral);
                if (novedadGeneral != null)
                {
                    novedadGeneral.Estado = "Cerrada";
                    novedadGeneral.EstadoLogistica = "Cerrada";
                    _context.Update(novedadGeneral);
                    await _context.SaveChangesAsync();
                }
            }
            // Si se seleccionó Novedad Comercial
            else if (rutum.IdNovedadComercial != null)
            {
                var novedadComercial = await _context.NovedadComercials.FindAsync(rutum.IdNovedadComercial);
                if (novedadComercial != null)
                {
                    novedadComercial.Estado = "Cerrada";
                    novedadComercial.EstadoLogistica = "Cerrada";
                    _context.Update(novedadComercial);
                    await _context.SaveChangesAsync();
                }
            }
            // Si se seleccionó Novedad de Compras
            else if (rutum.IdNovedadCompras != null)
            {
                var novedadCompra = await _context.NovedadCompras.FindAsync(rutum.IdNovedadCompras);
                if (novedadCompra != null)
                {
                    novedadCompra.Estado = "Cerrada";
                    novedadCompra.EstadoLogistica = "Cerrada";
                    _context.Update(novedadCompra);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public IActionResult ExportarExcelRemisiones()
        {
            var userRole = User.FindFirst("Rol")?.Value;
            var userName = User.Identity.Name;

            // Consulta las rutas con las condiciones indicadas
            IQueryable<Rutum> query = _context.Ruta
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                    .ThenInclude(n => n.IdUsuarioNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdClienteNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdCompraNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdUsuarioNavigation)
                .Include(r => r.IdUsuarioNavigation);

            
            DateTime fechaInicio = DateTime.Today;
            DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);

            
            query = query.Where(r => r.FechaCierre >= fechaInicio && r.FechaCierre < fechaFin
                                    && (r.IdNovedadComercialNavigation.TipoDocumento == "REMISION"
                                        || r.IdNovedadComercialNavigation.TipoDocumento == "REMISION CON NOVEDAD"))
                        .OrderBy(r => r.IdNovedadComercialNavigation.IdUsuarioNavigation.IdEmpresaNavigation.Nombre);

            var rutas = query.ToList();

            if (!rutas.Any())
            {
                TempData["Message"] = "No se encontraron remisiones para la fecha actual.";
                return RedirectToAction("Index");
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("REMISIONES");

                // Título en la primera fila, que se extiende desde la columna 1 a la 5
                worksheet.Range(1, 1, 1, 5).Merge().Value = $"REMISIONES GEU {DateTime.Now}";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 18;
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Encabezados en la fila 2
                worksheet.Cell(2, 1).Value = "Fecha";
                worksheet.Cell(2, 2).Value = "Empresa";
                worksheet.Cell(2, 3).Value = "Documento";
                worksheet.Cell(2, 4).Value = "Cliente";
                worksheet.Cell(2, 5).Value = "Recibe";

                // Aplicar estilo de negrita y centrar los encabezados
                for (int col = 1; col <= 5; col++)
                {
                    worksheet.Cell(2, col).Style.Font.Bold = true;
                    worksheet.Cell(2, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(2, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                // Filas de datos con bordes
                int row = 3;
                foreach (var ruta in rutas)
                {
                    worksheet.Cell(row, 1).Value = ruta.FechaActualizacion?.ToString("yyyy/MM/dd");
                    worksheet.Cell(row, 2).Value = ruta.IdNovedadComercialNavigation.IdUsuarioNavigation.IdEmpresaNavigation.Nombre;
                    worksheet.Cell(row, 3).Value = ruta.IdNovedadComercialNavigation.NumeroDocumento;

                    if (ruta.IdNovedadComercialNavigation.IdCliente != null)
                    {
                        worksheet.Cell(row, 4).Value = ruta.IdNovedadComercialNavigation.IdClienteNavigation.Nombre;
                    }
                    else
                    {
                        worksheet.Cell(row, 4).Value = ruta.IdNovedadComercialNavigation.Empresa;
                    }

                    worksheet.Cell(row, 5).Value = "";

                    // Aplicar bordes a todas las celdas de esta fila
                    for (int col = 1; col <= 5; col++)
                    {
                        worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    row++;
                }

                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).AdjustToContents();
                worksheet.Column(3).AdjustToContents();
                worksheet.Column(4).Width = 50;
                worksheet.Column(5).Width = 12;

                worksheet.PageSetup.PrintAreas.Add("A1:E" + row.ToString());

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Remisiones_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }

        }


        // GET: RutaUsuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Logistica"))
            {
                return RedirectToAction("Index", "Main");
            }

            if (id == null)
            {
                return NotFound();
            }

            var rutum = await _context.Ruta
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdRuta == id);
            if (rutum == null)
            {
                return NotFound();
            }

            return View(rutum);
        }

        // POST: RutaUsuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rutum = await _context.Ruta.FindAsync(id);
            if (rutum != null)
            {
                _context.Ruta.Remove(rutum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // CONSULTE

        // GET: Consulte Review
        public async Task<IActionResult> ConsulteReview(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Logistica"))
            {
                return RedirectToAction("Index", "Main");
            }

            IQueryable<Rutum> query = _context.Ruta
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                    .ThenInclude(n => n.IdUsuarioNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdClienteNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdUsuarioNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdCompraNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdUsuarioNavigation)
                .Include(r => r.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                //query = query.Where(r=> r.IdUsuarioNavigation.Username == userName);
                query = query.Where(r => r.Estado != "Anulado");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Comercial":
                        query = query.Where(r => r.IdNovedadComercialNavigation.NumeroDocumento.ToString().Contains(searchTerm));
                        break;
                    case "Compras":
                        query = query.Where(r => r.IdNovedadComprasNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Novedad":
                        query = query.Where(r => r.IdNovedadGeneralNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Estado":
                        query = query.Where(r => r.Estado.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(r => r.FechaAsignacion >= fechaInicio && r.FechaAsignacion < fechaFin);
            }

            query = query.OrderBy(n => n.IdRuta);
            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new RutaConsulteViewModel
            {
                Rutas = paginatedItems,
                Pagination = new PaginationViewModelRutaConsulte
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // POST: Method by dowload Excel the data in the list ConsulteReview
        public IActionResult ExportExcelConsulteR()
        {
            var userRole = User.FindFirst("Rol")?.Value;
            var userName = User.Identity.Name;

            IQueryable<Rutum> query = _context.Ruta
                .Include(r => r.IdConductorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Include(r => r.IdNovedadGeneralNavigation)
                    .ThenInclude(n => n.IdUsuarioNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdClienteNavigation)
                .Include(r => r.IdNovedadComercialNavigation)
                    .ThenInclude(nc => nc.IdUsuarioNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdCompraNavigation)
                .Include(r => r.IdNovedadComprasNavigation)
                    .ThenInclude(ncm => ncm.IdUsuarioNavigation)
                .Include(r => r.IdUsuarioNavigation);

            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                query = query.Where(r => r.Estado != "Anulado");
            }

            query = query.OrderBy(r => r.IdRuta);
            var rutas = query.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ruta");

                // Título centrado y combinado
                var titleCell = worksheet.Range("A1:M1").Merge();
                titleCell.Value = "Resumen de Solicitudes";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.Black;

                // Encabezados
                var headers = new string[]
                {
                    "Id Ruta", "Fecha Asignación", "Fecha Cerrado", "Tipo Documento",
                    "Numero Documento", "Tipo de Servicio", "Empresa", "Solicitado por",
                    "Conductor", "Estado", "Recibido", "Causa", "Observaciones"
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

                // Datos
                int row = 3;
                foreach (var ruta in rutas)
                {
                    worksheet.Cell(row, 1).Value = ruta.IdRuta;
                    worksheet.Cell(row, 2).Value = ruta.FechaRuta?.ToString("yyyy/MM/dd");
                    worksheet.Cell(row, 3).Value = ruta.FechaActualizacion?.ToString("yyyy/MM/dd");

                    if (ruta.IdNovedadGeneral != null)
                    {
                        worksheet.Cell(row, 4).Value = "Novedad General";
                        worksheet.Cell(row, 5).Value = ruta.IdNovedadGeneralNavigation.Consecutivo;
                        worksheet.Cell(row, 6).Value = ruta.IdNovedadGeneralNavigation.TipoNovedad;
                        worksheet.Cell(row, 7).Value = ruta.IdNovedadGeneralNavigation.Empresa;
                        worksheet.Cell(row, 8).Value = ruta.IdNovedadGeneralNavigation.IdUsuarioNavigation.Nombre;
                    }
                    else if (ruta.IdNovedadCompras != null)
                    {
                        worksheet.Cell(row, 4).Value = "Novedad Compras";
                        worksheet.Cell(row, 5).Value = ruta.IdNovedadComprasNavigation.Consecutivo;
                        worksheet.Cell(row, 6).Value = ruta.IdNovedadComprasNavigation.TipoNovedad;
                        worksheet.Cell(row, 7).Value = ruta.IdNovedadComprasNavigation.Empresa;
                        worksheet.Cell(row, 8).Value = ruta.IdNovedadComprasNavigation.IdUsuarioNavigation.Nombre;
                    }
                    else if (ruta.IdNovedadComercial != null)
                    {
                        worksheet.Cell(row, 4).Value = "Novedad Comercial";
                        worksheet.Cell(row, 5).Value = ruta.IdNovedadComercialNavigation.NumeroDocumento;
                        worksheet.Cell(row, 6).Value = ruta.IdNovedadComercialNavigation.TipoServicio;
                        worksheet.Cell(row, 7).Value = ruta.IdNovedadComercialNavigation.IdCliente != null
                            ? ruta.IdNovedadComercialNavigation.IdClienteNavigation.Nombre
                            : ruta.IdNovedadComercialNavigation.Empresa;
                        worksheet.Cell(row, 8).Value = ruta.IdNovedadComercialNavigation.IdUsuarioNavigation.Nombre;
                    }

                    worksheet.Cell(row, 9).Value = ruta.IdConductorNavigation?.Nombre ?? "Sin asignar";
                    worksheet.Cell(row, 10).Value = ruta.Estado;
                    worksheet.Cell(row, 11).Value = ruta.Recibido;
                    worksheet.Cell(row, 12).Value = ruta.Causa;
                    worksheet.Cell(row, 13).Value = ruta.Observaciones1;

                    // Bordes a cada celda de la fila
                    for (int col = 1; col <= 13; col++)
                    {
                        worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Rutas_Resumen_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }



        private bool RutumExists(int id)
        {
            return _context.Ruta.Any(e => e.IdRuta == id);
        }
    }
}
