using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Management_system.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Management_system.Models.Other.ViewModel.Logistica;
using Rotativa.AspNetCore;

namespace Management_system.Controllers
{
    public class NovedadUsuarioController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public NovedadUsuarioController(DbManagementSystemContext context)
        {
            _context = context;
        }


        // GET: NovedadUsuario
        public async Task<IActionResult> Index(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<Novedad> query = _context.Novedads
                .Include(n => n.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(n => n.IdUsuarioNavigation.Username == userName);
                query = query.Where(n => n.Estado != "Programada" && n.Estado != "Anulado" && n.Estado != "Cerrada");
            }

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

            query = query.OrderByDescending(n => n.IdNovedad);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadIndexViewModel
            {
                Novedades = paginatedItems,
                Pagination = new PaginationViewModelNovedad
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }
    

        // GET: NovedadUsuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedad = await _context.Novedads
                .Include(n => n.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedad == id);
            if (novedad == null)
            {
                return NotFound();
            }

            return View(novedad);
        }

        public async Task<IActionResult> GenerarPDF(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedad = await _context.Novedads
                .Include(n => n.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedad == id);
            if (novedad == null)
            {
                return NotFound();
            }

            // Retorna el PDF usando la vista `DetailsPDF`
            return new ViewAsPdf("NovedadPDF", novedad)
            {
                FileName = $"Novedad_Compra_{novedad.Consecutivo}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--disable-smart-shrinking"
            };
        }

        // GET: NovedadUsuario/Create
        public IActionResult Create()
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
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


            return View();
        }

        // POST: NovedadUsuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNovedad,Fecha,TipoNovedad,FechaSalida,Empresa,Direccion,CiudadBarrio,IdUsuario,Contacto,Telefono,Observaciones,Consecutivo,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Estado,EstadoLogistica,ObservacionesLogistica")] Novedad novedad)
        {
            if (ModelState.IsValid)
            {
                novedad.Fecha = DateTime.Now;
                novedad.Estado = "Abierta";
                _context.Add(novedad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedad.IdUsuario);

            return View(novedad);
        }

        // GET: NovedadUsuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedad = await _context.Novedads.FindAsync(id);
            if (novedad == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Where(u => u.IdUsuario == novedad.IdUsuario)
                .Select(u => new { u.IdUsuario, u.Username })
                .FirstOrDefaultAsync();

            ViewData["IdUsuario"] = new SelectList(new List<object> { new { usuario.IdUsuario, usuario.Username } }, "IdUsuario", "Username", novedad.IdUsuario);

            return View(novedad);
        }

        // POST: NovedadUsuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNovedad,Fecha,TipoNovedad,FechaSalida,Empresa,Direccion,CiudadBarrio,IdUsuario,Contacto,Telefono,Observaciones,Consecutivo,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Estado,EstadoLogistica,ObservacionesLogistica")] Novedad novedad)
        {
            if (id != novedad.IdNovedad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    novedad.FechaActualizacion = DateTime.Now;
                    _context.Update(novedad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NovedadExists(novedad.IdNovedad))
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
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedad.IdUsuario);
            return View(novedad);
        }

        // GET: NovedadUsuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedad = await _context.Novedads
                .Include(n => n.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedad == id);
            if (novedad == null)
            {
                return NotFound();
            }

            return View(novedad);
        }

        // POST: NovedadUsuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var novedad = await _context.Novedads.FindAsync(id);
            if (novedad != null)
            {
                _context.Novedads.Remove(novedad);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // TRACKING

        // GET: Tracking Index
        public async Task<IActionResult> TrackingIndex(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<Novedad> query = _context.Novedads
                .Include(n => n.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(n => n.IdUsuarioNavigation.Username == userName);
                //query = query.Where(n => n.Estado != "Anulado");
            }

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

            query = query.OrderByDescending(n => n.IdNovedad);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadTrackingViewModel
            {
                Novedades = paginatedItems,
                Pagination = new PaginationViewModelNovedadTracking
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        //public ActionResult TrackingDetails(int id)
        //{
        //    // Obtener datos de la novedad
        //    var novedad = _context.Novedads.FirstOrDefault(n => n.IdNovedad == id);

        //    // Obtener datos de la ruta (si existe)
        //    var ruta = _context.Ruta.FirstOrDefault(r => r.IdNovedadGeneral == id);

        //    var viewModel = new LogisticaViewModel
        //    {
        //        Novedades = novedad,
        //        Rutas = ruta
        //    };

        //    return View(viewModel);
        //}

        public async Task<IActionResult> TrackingDetails(int? idnovedad)
        {
            if (idnovedad == null)
            {
                return NotFound();
            }

            var novedad = await _context.Novedads
               .Include(n => n.IdUsuarioNavigation)
               .FirstOrDefaultAsync(n => n.IdNovedad == idnovedad);

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
                .FirstOrDefaultAsync(r => r.IdNovedadGeneral == idnovedad);


            var viewModel = new LogisticaNViewModel
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
            IQueryable<Novedad> query = _context.Novedads
                .Include(n => n.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(n => n.IdUsuarioNavigation.Username == userName);
                query = query.Where(n => n.Estado != "Programada" && n.Estado != "Anulado" && n.Estado != "Cerrada");
            }

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

            query = query.OrderBy(n => n.IdNovedad);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadOverrideViewModel
            {
                Novedades = paginatedItems,
                Pagination = new PaginationViewModelNovedadOverride
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // GET: NovedadUsuario/Edit/5
        public async Task<IActionResult> OverrideEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novedad = await _context.Novedads.FindAsync(id);
            if (novedad == null)
            {
                return NotFound();
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username", novedad.IdUsuario);
            return View(novedad);
        }

        // POST: NovedadUsuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OverrideEdit(int id, [Bind("IdNovedad,Fecha,TipoNovedad,FechaSalida,Empresa,Direccion,CiudadBarrio,IdUsuario,Contacto,Telefono,Observaciones,Consecutivo,Link,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3,Estado,EstadoLogistica,ObservacionesLogistica")] Novedad novedad)
        {
            if (id != novedad.IdNovedad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    novedad.FechaActualizacion = DateTime.Now;
                    _context.Update(novedad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NovedadExists(novedad.IdNovedad))
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
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", novedad.IdUsuario);
            return View(novedad);
        }

        // CONSULTE

        // GET: Consulte Review
        public async Task<IActionResult> ConsulteReview(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<Novedad> query = _context.Novedads
                .Include(n => n.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario"
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(n => n.IdUsuarioNavigation.Username == userName);
                query = query.Where(n => n.Estado != "Programada" && n.Estado != "Anulado" && n.Estado != "Cerrada");
            }

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

            query = query.OrderBy(n => n.IdNovedad);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new NovedadConsulteViewModel
            {
                Novedades = paginatedItems,
                Pagination = new PaginationViewModelNovedadConsulte
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

            IQueryable<Novedad> query = _context.Novedads
                .Include(n => n.IdUsuarioNavigation);

            // Aplicar filtro de usuario si el rol y el nombre de usuario cumplen con los requisitos
            if (userRole != "Administrador" && userRole != "Super Usuario" &&
                userRole != "Gerencia" && userRole != "Desarrollador" &&
                userRole != "Soporte TI")
            {
                query = query.Where(n => n.IdUsuarioNavigation.Username == userName);
            }

            query = query.OrderBy(n => n.IdNovedad);
            var novedades = query.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Novedades Generales");

                // Título centrado y combinado en las columnas A-O
                var titleCell = worksheet.Range("A1:O1").Merge();
                titleCell.Value = "Novedades Generales";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.DarkBlue;

                // Encabezados de columna
                var headers = new string[]
                {
                    "Id Novedad", "Consecutivo", "Fecha Creación", "Tipo Novedad",
                    "Fecha Salida", "Empresa", "Dirección", "Ciudad - Barrio",
                    "Usuario", "Contacto", "Teléfono", "Observaciones",
                    "Estado", "Observación Administrador", "Observación Logística"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(2, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                // Rellenar datos desde la fila 3
                int row = 3;
                foreach (var novedad in novedades)
                {
                    worksheet.Cell(row, 1).Value = novedad.IdNovedad;
                    worksheet.Cell(row, 2).Value = novedad.Consecutivo;
                    worksheet.Cell(row, 3).Value = novedad.Fecha?.ToString("yyyy/MM/dd");
                    worksheet.Cell(row, 4).Value = novedad.TipoNovedad;
                    worksheet.Cell(row, 5).Value = novedad.FechaSalida?.ToString("yyyy/MM/dd");
                    worksheet.Cell(row, 6).Value = novedad.Empresa;
                    worksheet.Cell(row, 7).Value = novedad.Direccion;
                    worksheet.Cell(row, 8).Value = novedad.CiudadBarrio;
                    worksheet.Cell(row, 9).Value = novedad.IdUsuarioNavigation.Username;
                    worksheet.Cell(row, 10).Value = novedad.Contacto;
                    worksheet.Cell(row, 11).Value = novedad.Telefono;
                    worksheet.Cell(row, 12).Value = novedad.Observaciones;
                    worksheet.Cell(row, 13).Value = novedad.Estado;
                    worksheet.Cell(row, 14).Value = novedad.Observaciones1;
                    worksheet.Cell(row, 15).Value = novedad.ObservacionesLogistica;

                    // Aplicar bordes a las celdas de la fila actual
                    for (int col = 1; col <= 15; col++)
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

        private bool NovedadExists(int id)
        {
            return _context.Novedads.Any(e => e.IdNovedad == id);
        }
    }
}
