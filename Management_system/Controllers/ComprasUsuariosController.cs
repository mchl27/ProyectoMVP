using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Management_system.Models;
using Microsoft.VisualBasic;
using ClosedXML.Excel;
using Rotativa.AspNetCore;
using Humanizer;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing.Printing;
using Management_system.Models.Others.Analyze;

namespace Management_system.Controllers
{
    public class ComprasUsuariosController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public ComprasUsuariosController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: ComprasUsuarios
        public async Task<IActionResult> Index(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            IQueryable<Compra> query = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
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

            query = query.OrderByDescending(c => c.IdCompra);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ComprasIndexViewModel
            {
                Compras = paginatedItems,
                Pagination = new PaginationViewModelCompras
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        public IActionResult ExportarExcel()
        {
            IQueryable<Compra> query = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .Where(c => c.Estado == "Abierta");

            query = query.OrderBy(c => c.IdCompra);
            var compras = query.ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ordenes de Compra");

                // Título centrado y combinado en las columnas A-N
                var titleCell = worksheet.Range("A1:L1").Merge();
                titleCell.Value = "Ordenes de Compra";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.Black;


                var headers = new string[]
                {
                    "OC", "Fecha O.C","Fecha de Entrega","Estado de la orden","Estado para Pago",
                    "Estado para Bodega", "Estado para Logistica","Observaciones","Solicitud",
                    "Vendedor","Proveedor", "Usuario"
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

                int row = 3;
                foreach (var compra in compras)
                {
                    worksheet.Cell(row, 1).Value = compra.OrdenCompra;
                    worksheet.Cell(row, 2).Value = compra.FechaCompra;
                    worksheet.Cell(row, 3).Value = compra.FechaEntrega;
                    worksheet.Cell(row, 4).Value = compra.Estado;
                    worksheet.Cell(row, 5).Value = compra.Pago;
                    worksheet.Cell(row, 6).Value = compra.Bodega;
                    worksheet.Cell(row, 7).Value = compra.Logistica;
                    worksheet.Cell(row, 8).Value = compra.ObservacionesPago;
                    worksheet.Cell(row, 9).Value = compra.IdSolicitudNavigation.Consecutivo;
                    worksheet.Cell(row, 10).Value = compra.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                    worksheet.Cell(row, 11).Value = compra.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                    worksheet.Cell(row, 12).Value = compra.IdUsuarioNavigation.Username;

                    for (int col = 1; col <= 12; col++)
                    {
                        worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    row++;
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Ordencompras_Abiertas_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
        }

        // POST: METHOD OC ESTADO = ABIERTA DAILY
        public async Task<IActionResult> ExportExcelDay()
        {
            // Consulta base para las compras
            IQueryable<Compra> query = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .Include(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdUsuarioNavigation)
                .Include(c => c.IdUsuarioNavigation);


            // Verify if the user has the appropriate roles
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;

            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras" && userArea != "Bodega"))
            {
                query = query.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            query = query.Where(c => c.Estado == "Abierta");
            query = query.Where(c => c.IdSolicitudNavigation.Estado == "OC Creada");

            // Filtro por la fecha de entrega de hoy
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);

            query = query.Where(c => c.FechaEntrega >= today && c.FechaEntrega < tomorrow);

            var compras = await query.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras");

                // Título centrado y combinado en las columnas A-N
                var titleCell = worksheet.Range("A1:N1").Merge();
                titleCell.Value = "Ordenes de Compra Diarias";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.Black;

                // Encabezados de columna
                var headers = new string[]
                {
                    "Consecutivo", "OC", "Fecha Compra", "Fecha Entrega",
                    "Estado Compra", "Observaciones Compra", "Vendedor", "Proveedor", "Referencia",
                    "Descripción", "Cantidad", "Costo", "Venta", "Rentabilidad"
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

                int row = 3;
                foreach (var compra in compras)
                {
                    var detalles = _context.SolicitudDetalles
                        .Include(d => d.IdProductoNavigation)
                        .Where(d => d.IdSolicitud == compra.IdSolicitud)
                        .ToList();

                    foreach (var detalle in detalles)
                    {
                        worksheet.Cell(row, 1).Value = compra.IdSolicitudNavigation.Consecutivo;
                        worksheet.Cell(row, 2).Value = compra.OrdenCompra;
                        worksheet.Cell(row, 3).Value = compra.FechaCompra;
                        worksheet.Cell(row, 4).Value = compra.FechaEntrega;
                        worksheet.Cell(row, 5).Value = compra.Estado;
                        worksheet.Cell(row, 6).Value = compra.ObservacionesPago;
                        worksheet.Cell(row, 7).Value = compra.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                        worksheet.Cell(row, 8).Value = compra.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                        worksheet.Cell(row, 9).Value = detalle.IdProductoNavigation.Referencia;
                        worksheet.Cell(row, 10).Value = detalle.IdProductoNavigation.Descripcion;
                        worksheet.Cell(row, 11).Value = detalle.Cantidad;
                        worksheet.Cell(row, 12).Value = detalle.PrecioCosto;
                        worksheet.Cell(row, 13).Value = detalle.PrecioVenta;
                        worksheet.Cell(row, 14).Value = detalle.Rentabilidad;
                        row++;
                    }
                }
                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Entregas_hoy{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }

        // POST: METHOD OC ESTADO = ABIERTA DAILY
        public async Task<IActionResult> ExportExcelWeek()
        {
            // Consulta base para las compras
            IQueryable<Compra> query = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .Include(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdUsuarioNavigation)
                .Include(c => c.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras" && userArea != "Bodega"))
            {
                query = query.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            query = query.Where(c => c.Estado == "Abierta");
            query = query.Where(c => c.IdSolicitudNavigation.Estado == "OC Creada");

            // Filtro por la fecha de entrega desde hoy hasta una semana adelante
            DateTime today = DateTime.Today;
            DateTime nextWeek = today.AddDays(7);

            query = query.Where(c => c.FechaEntrega >= today && c.FechaEntrega < nextWeek);

            var compras = await query.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras");

                // Título centrado y combinado en las columnas A-N
                var titleCell = worksheet.Range("A1:N1").Merge();
                titleCell.Value = "Ordenes de Compra Semanales";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.Black;

                // Encabezados de columna
                var headers = new string[]
                {
                    "Consecutivo", "OC", "Fecha Compra", "Fecha Entrega",
                    "Estado Compra", "Observaciones Compra", "Vendedor", "Proveedor", "Referencia",
                    "Descripción", "Cantidad", "Costo", "Venta", "Rentabilidad"
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

                int row = 3;
                foreach (var compra in compras)
                {
                    var detalles = _context.SolicitudDetalles
                        .Include(d => d.IdProductoNavigation)
                        .Where(d => d.IdSolicitud == compra.IdSolicitud)
                        .ToList();

                    foreach (var detalle in detalles)
                    {
                        worksheet.Cell(row, 1).Value = compra.IdSolicitudNavigation.Consecutivo;
                        worksheet.Cell(row, 2).Value = compra.OrdenCompra;
                        worksheet.Cell(row, 3).Value = compra.FechaCompra;
                        worksheet.Cell(row, 4).Value = compra.FechaEntrega;
                        worksheet.Cell(row, 5).Value = compra.Estado;
                        worksheet.Cell(row, 6).Value = compra.ObservacionesPago;
                        worksheet.Cell(row, 7).Value = compra.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                        worksheet.Cell(row, 8).Value = compra.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                        worksheet.Cell(row, 9).Value = detalle.IdProductoNavigation.Referencia;
                        worksheet.Cell(row, 10).Value = detalle.IdProductoNavigation.Descripcion;
                        worksheet.Cell(row, 11).Value = detalle.Cantidad;
                        worksheet.Cell(row, 12).Value = detalle.PrecioCosto;
                        worksheet.Cell(row, 13).Value = detalle.PrecioVenta;
                        worksheet.Cell(row, 14).Value = detalle.Rentabilidad;

                        // Aplicar bordes a las celdas de la fila actual
                        for (int col = 1; col <= 14; col++)
                        {
                            worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }

                        row++;
                    }
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Entregas_semanal{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }

        // GET: ComprasUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: ComprasUsuarios/Create
        public IActionResult Create()
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: ComprasUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,IdSolicitud,IdUsuario,FechaCompra,FechaEntrega,OrdenCompra,Estado,Observaciones,Pago,ObservacionesPago,Logistica,ObservacionesLogistica,Bodega,ObservacionesBodega,LinkOc,OrdenCompra1,LinkOc1,OrdenCompra2,LinkOc2,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", compra.IdSolicitud);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", compra.IdUsuario);
            return View(compra);
        }

        // GET: ComprasUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }

            // Verify if the user has the appropriate roles
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
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username", compra.IdUsuario);
            }

            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "Consecutivo", compra.IdSolicitud);
            return View(compra);
        }

        // POST: ComprasUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompra,IdSolicitud,IdUsuario,FechaCompra,FechaEntrega,OrdenCompra,Estado,Observaciones,Pago,ObservacionesPago,Logistica,ObservacionesLogistica,Bodega,ObservacionesBodega,LinkOc,OrdenCompra1,LinkOc1,OrdenCompra2,LinkOc2,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3")] Compra compra)
        {
            if (id != compra.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    compra.FechaActualizacion = DateTime.Now;
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.IdCompra))
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
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", compra.IdSolicitud);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", compra.IdUsuario);
            return View(compra);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileEdit(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var uploads = Path.Combine(@"\\udc_des01\DB OC");
                    var filePath = Path.Combine(uploads, file.FileName);

                    // Verificar si el archivo ya existe y eliminarlo si es necesario
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Json(new { fileName = file.FileName, filePath });
                }
                catch (Exception ex)
                {
                    return Json(new { error = "Error uploading file", details = ex.Message });
                }
            }
            return BadRequest("No file uploaded");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileEdit2(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var uploads = Path.Combine(@"\\udc_des01\DB OC");
                    var filePath = Path.Combine(uploads, file.FileName);

                    // Verificar si el archivo ya existe y eliminarlo si es necesario
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Json(new { fileName = file.FileName, filePath });
                }
                catch (Exception ex)
                {
                    return Json(new { error = "Error uploading file", details = ex.Message });
                }
            }
            return BadRequest("No file uploaded");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileEdit3(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var uploads = Path.Combine(@"\\udc_des01\DB OC");
                    var filePath = Path.Combine(uploads, file.FileName);

                    // Verificar si el archivo ya existe y eliminarlo si es necesario
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Json(new { fileName = file.FileName, filePath });
                }
                catch (Exception ex)
                {
                    return Json(new { error = "Error uploading file", details = ex.Message });
                }
            }
            return BadRequest("No file uploaded");
        }

        // GET: ComprasUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                return RedirectToAction("Index", "Main");
            }

            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: ComprasUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra != null)
            {
                _context.Compras.Remove(compra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // User's Views

        public async Task<IActionResult> ReviewIndex(string searchTerm, string searchCriteria, DateTime? fecha,
                                                    string sortOrder = "Consecutivo", string sortDirection = "asc",
                                                    int pageNumber = 1, int pageSize = 35)
        {
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            IQueryable<Solicitud> query = _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdUsuarioNavigation.IdEmpresaNavigation)
                .Include(s => s.IdVendedorNavigation);

            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            {
                query = query.Where(s => s.Estado != "OC Creada" && s.Estado != "Anulado" && s.Estado != "En proceso" && s.Estado != "Revisar");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(s => s.Consecutivo.Contains(searchTerm));
                        break;
                    case "Cliente":
                        query = query.Where(s => s.IdClienteNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        query = query.Where(s => s.IdProveedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        query = query.Where(s => s.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Estado":
                        query = query.Where(s => s.Estado.Contains(searchTerm));
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(s => s.Fecha >= fechaInicio && s.Fecha < fechaFin);
            }

            // Ordenamiento dinámico
            query = (sortOrder, sortDirection.ToLower()) switch
            {
                ("Consecutivo", "asc") => query.OrderBy(s => s.Consecutivo),
                ("Consecutivo", "desc") => query.OrderByDescending(s => s.Consecutivo),
                ("Cliente", "asc") => query.OrderBy(s => s.IdClienteNavigation.Nombre),
                ("Cliente", "desc") => query.OrderByDescending(s => s.IdClienteNavigation.Nombre),
                ("Proveedor", "asc") => query.OrderBy(s => s.IdProveedorNavigation.Nombre),
                ("Proveedor", "desc") => query.OrderByDescending(s => s.IdProveedorNavigation.Nombre),
                ("Vendedor", "asc") => query.OrderBy(s => s.IdVendedorNavigation.Nombre),
                ("Vendedor", "desc") => query.OrderByDescending(s => s.IdVendedorNavigation.Nombre),
                ("Estado", "asc") => query.OrderBy(s => s.Estado),
                ("Estado", "desc") => query.OrderByDescending(s => s.Estado),
                ("Fecha", "asc") => query.OrderBy(s => s.Fecha),
                ("Fecha", "desc") => query.OrderByDescending(s => s.Fecha),
                _ => query.OrderBy(s => s.IdSolicitud)
            };

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ReviewSolicitudesViewModel
            {
                Solicitudes = paginatedItems,
                Pagination = new PaginationViewModelSolicitudesReview
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                },
                SortOrder = sortOrder,
                SortDirection = sortDirection
            };

            return View(viewModel);
        }

        // GET: ComprasUsuarios/ReviewDetails/5
        public IActionResult ReviewDetails(int idSolicitud)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;

            var SolicitudQuery = _context.Solicituds
            .Include(s => s.IdClienteNavigation)
            .Include(s => s.IdProveedorNavigation)
            .Include(s => s.IdUsuarioNavigation)
            .Include(s => s.IdVendedorNavigation)
            .Where(s => s.IdSolicitud == idSolicitud);

            
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                SolicitudQuery = SolicitudQuery.Where(c => c.IdUsuarioNavigation.Username == userName);
            }

            var solicitud = SolicitudQuery.FirstOrDefault();

            if (solicitud == null)
            {
                return RedirectToAction("Index", "Main");
            }

            var solicitudDetalles = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .Where(sd => sd.IdSolicitud == idSolicitud)
                .ToList();

            var model = new SolicitudCompraReviewViewModel
            {
                Solicitudes = solicitud,
                SolicitudDetalles = solicitudDetalles
            };

            return View(model);
        }
        
        // GET: ComprasUsuarios/ReviewEdit/5
        public async Task<IActionResult> ReviewEdit(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }


            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicituds.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Where(c => c.IdCliente == solicitud.IdCliente)
                .Select(c => new { c.IdCliente, c.Nombre })
                .FirstOrDefaultAsync();

            var proveedor = await _context.Proveedors
                .Where(p => p.IdProveedor == solicitud.IdProveedor)
                .Select(p => new { p.IdProveedor, p.Nombre })
                .FirstOrDefaultAsync();


            var usuario = await _context.Usuarios
                .Where(u => u.IdUsuario == solicitud.IdUsuario)
                .Select(u => new { u.IdUsuario, u.Username })
                .FirstOrDefaultAsync();

            var vendedor = await _context.Vendedors
                .Where(v => v.IdVendedor == solicitud.IdVendedor)
                .Select(v => new { v.IdVendedor, v.Nombre })
                .FirstOrDefaultAsync();


            var empresa = await _context.Empresas
                .Where(e => e.IdEmpresa == solicitud.IdEmpresa)
                .Select(e => new { e.IdEmpresa, e.Nombre })
                .FirstOrDefaultAsync();

            ViewData["IdCliente"] = new SelectList(new List<object> { new { cliente.IdCliente, cliente.Nombre } }, "IdCliente", "Nombre", solicitud.IdCliente);
            ViewData["IdProveedor"] = new SelectList(new List<object> { new { proveedor.IdProveedor, proveedor.Nombre } }, "IdProveedor", "Nombre", solicitud.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(new List<object> { new { usuario.IdUsuario, usuario.Username } }, "IdUsuario", "Username", solicitud.IdUsuario);
            ViewData["IdVendedor"] = new SelectList(new List<object> { new { vendedor.IdVendedor, vendedor.Nombre } }, "IdVendedor", "Nombre", solicitud.IdVendedor);
            ViewData["IdEmpresa"] = new SelectList(new List<object> { new { empresa.IdEmpresa, empresa.Nombre } }, "IdEmpresa", "Nombre");

            return View(solicitud);
        }

        // POST: ComprasUsuarios/ReviewEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewEdit(int id, [Bind("IdSolicitud,Fecha,IdCliente,IdProveedor,IdUsuario,IdVendedor,Estado,Consecutivo,Observaciones,ProveedorSugerido,FechaEntrega,Negociacion,FechaActualizacion,ObservacionesActualizacion,IdEmpresa,IdUsuarioAsignado,FechaModificacion,FechaOc,FechaRm,FechaCierre,ConsecutivoPorEmpresa,IdUsuarioAprobado")] Solicitud solicitud)
        {
            if (id != solicitud.IdSolicitud)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    solicitud.FechaActualizacion = DateTime.Now;

                    _context.Update(solicitud);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitudExists(solicitud.IdSolicitud))
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
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", solicitud.IdCliente);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", solicitud.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", solicitud.IdUsuario);
            ViewData["IdVendedor"] = new SelectList(_context.Vendedors, "IdVendedor", "IdVendedor", solicitud.IdVendedor);
            return View(solicitud);
        }

        // GET: ComprasUsuarios/Create
        public IActionResult ReviewCreate(int idSolicitud)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idSolicitud);

            // Verify if the user has the appropriate roles
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

            ViewData["IdSolicitud"] = idSolicitud;
            
            return View();
        }

        // POST: ComprasUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewCreate([Bind("IdCompra,IdSolicitud,IdUsuario,FechaCompra,FechaEntrega,OrdenCompra,Estado,Observaciones,Pago,ObservacionesPago,Logistica,ObservacionesLogistica,Bodega,ObservacionesBodega,LinkOc,OrdenCompra1,LinkOc1,OrdenCompra2,LinkOc2,FechaActualizacion,Observaciones1,Observaciones2,Observaciones3")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                var solicitud = await _context.Solicituds.FindAsync(compra.IdSolicitud);
                if (solicitud != null)
                {
                    solicitud.Estado = "OC Creada"; // Change the status as you wish
                    _context.Update(solicitud);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));

            }

            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", compra.IdSolicitud);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", compra.IdUsuario);
            return View(compra);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var uploads = Path.Combine(@"\\udc_des01\DB OC");
                    var filePath = Path.Combine(uploads, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Json(new { fileName = file.FileName, filePath });
                }
                catch (Exception ex)
                {
                    return Json(new { error = "Error uploading file", details = ex.Message });
                }
            }
            return BadRequest("No file uploaded");
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile2(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var uploads = Path.Combine(@"\\udc_des01\DB OC");
                    var filePath = Path.Combine(uploads, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Json(new { fileName = file.FileName, filePath });
                }
                catch (Exception ex)
                {
                    return Json(new { error = "Error uploading file", details = ex.Message });
                }
            }
            return BadRequest("No file uploaded");
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile3(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var uploads = Path.Combine(@"\\udc_des01\DB OC");
                    var filePath = Path.Combine(uploads, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Json(new { fileName = file.FileName, filePath });
                }
                catch (Exception ex)
                {
                    return Json(new { error = "Error uploading file", details = ex.Message });
                }
            }
            return BadRequest("No file uploaded");
        }

        // GET : ComprasUsuarios / Analize
        public IActionResult Analyze(int idSolicitud)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }


            var compraViewModel = new CompraViewModel();

            // Obtener la compra
            var compra = _context.Compras
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefault(c => c.IdSolicitud == idSolicitud);

            if (compra == null)
            {
                return NotFound();
            }

            compraViewModel.NuevaCompra = compra;

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

            compraViewModel.NuevaSolicitud = solicitud;

            // Obtener los detalles de la solicitud
            var solicitudDetalles = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .Where(sd => sd.IdSolicitud == idSolicitud)
                .ToList();

            compraViewModel.DetallesSolicitud = solicitudDetalles;

            // Obtener los detalles de compra
            var compraDetalles = _context.CompraDetalles
                .Include(cd => cd.IdCompraNavigation)
                .Include(cd => cd.IdSolicitudDetalleNavigation)
                .Where(cd => cd.IdSolicitudDetalleNavigation.IdSolicitud == idSolicitud)
                .ToList();

            compraViewModel.Detalles = compraDetalles;

            return View(compraViewModel);
        }

        // GET: ComprasUsuarios/AnalizeCreate
        public IActionResult AnalyzeCreate(int idsolicitud, int idSolicitudDetalle, int idCompra)
        {

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            var solicitud = _context.Solicituds
                .Include(s => s.IdProveedorNavigation)
                .FirstOrDefault(s => s.IdSolicitud == idsolicitud);

            if (solicitud == null)
            {
                return NotFound();
            }

            ViewData["IdSolicitud"] = idsolicitud;
            ViewData["ProveedorSugerido"] = solicitud.IdProveedorNavigation.Nombre;
            ViewData["ProveedorFinal"] = solicitud.ProveedorSugerido;
            ViewData["Consecutivo"] = solicitud.Consecutivo;


            var solicitudDetalle = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .FirstOrDefault(sd => sd.IdSolicitudDetalle == idSolicitudDetalle);

            if (solicitudDetalle == null)
            {
                return NotFound();
            }

            ViewData["SolicitudDetalle"] = idSolicitudDetalle;
            ViewData["Referencia"] = solicitudDetalle.IdProductoNavigation.Referencia;
            ViewData["Descripcion"] = solicitudDetalle.IdProductoNavigation.Descripcion;
            ViewData["Cantidad"] = solicitudDetalle.Cantidad;

            var compra = _context.Compras
                .FirstOrDefault(c => c.IdCompra == idCompra);

            if (compra == null)
            {
                return NotFound();
            }

            ViewData["Compra"] = idCompra;
            ViewData["OrdenCompra"] = compra.OrdenCompra;

            return View();
        }

        // POST: ComprasUsuarios/AnalizeCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnalyzeCreate([Bind("IdCompraDetalle,IdCompra,IdSolicitudDetalle,ProveedorSugerido,PrecioUnitario,Cantidad,ProveedorSugerido1,PrecioUnitario1,Cantidad1")] CompraDetalle compraDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compraDetalle);
                await _context.SaveChangesAsync();

                await _context.Database.ExecuteSqlRawAsync("EXEC ActualizarCompraAhorro");

                var idSolicitudDetalle = compraDetalle.IdSolicitudDetalle;


                var idSolicitud = _context.SolicitudDetalles
                    .Where(sd => sd.IdSolicitudDetalle == idSolicitudDetalle)
                    .Select(sd => sd.IdSolicitud)
                    .FirstOrDefault();

                return RedirectToAction("Analyze", new { idSolicitud });
            }

            return View(compraDetalle);
        }

        // GET: ComprasUsuarios/AnalyzeEdit/5
        public async Task<IActionResult> AnalyzeEdit(int? id, int? idsolicitud)
        {

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            if (id == null)
            {
                return NotFound();
            }

            var comprasDetalle = await _context.CompraDetalles.FindAsync(id);
            if (comprasDetalle == null)
            {
                return NotFound();
            }
            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idsolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            ViewData["IdSolicitud"] = idsolicitud;
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "OrdenCompra", comprasDetalle.IdCompra);
            ViewData["IdSolicitudDetalle"] = new SelectList(_context.SolicitudDetalles, "IdSolicitudDetalle", "IdProducto", comprasDetalle.IdCompra);
            return View(comprasDetalle);
        }

        // POST: ComprasUsuarios/AnalyzeEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnalyzeEdit(int id, [Bind("IdCompraDetalle,IdCompra,IdSolicitudDetalle,ProveedorSugerido,PrecioUnitario,Cantidad,ProveedorSugerido1,PrecioUnitario1,Cantidad1")] CompraDetalle compraDetalle)
        {
            if (id != compraDetalle.IdCompraDetalle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compraDetalle);
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlRawAsync("EXEC ActualizarCompraAhorro");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraDetalleExists(compraDetalle.IdCompraDetalle))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var idSolicitudDetalle = compraDetalle.IdSolicitudDetalle;

                var idSolicitud = _context.SolicitudDetalles
                    .Where(sd => sd.IdSolicitudDetalle == idSolicitudDetalle)
                    .Select(sd => sd.IdSolicitud)
                    .FirstOrDefault();

                return RedirectToAction("Analyze", new { idSolicitud });
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "OrdenCompra", compraDetalle.IdCompra);
            ViewData["IdSolicitudDetalle"] = new SelectList(_context.SolicitudDetalles, "IdSolicitudDetalle", "IdProducto", compraDetalle.IdCompra);
            return View(compraDetalle);
        }


        // GET: ComprasUsuarios/AnalyzeDelete/5
        public async Task<IActionResult> AnalyzeDelete(int? id, int? idsolicitud)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }


            if (id == null)
            {
                return NotFound();
            }

            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idsolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            ViewData["IdSolicitud"] = idsolicitud;

            var compraDetalle = await _context.CompraDetalles
                .Include(cd => cd.IdSolicitudDetalleNavigation)
                .ThenInclude(sd => sd.IdProductoNavigation)
                .FirstOrDefaultAsync(cd => cd.IdCompraDetalle == id);
            if (compraDetalle == null)
            {
                return NotFound();
            }

            return View(compraDetalle);
        }

        // POST: ComprasUsuarios/AnalyzeDelete/5
        [HttpPost, ActionName("AnalyzeDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnalyzeDeleteConfirmed(int id)
        {

            var compraDetalle = await _context.CompraDetalles.FindAsync(id);
            if (compraDetalle != null)
            {
                _context.CompraDetalles.Remove(compraDetalle);
            }
            await _context.SaveChangesAsync();

            var idSolicitudDetalle = compraDetalle.IdSolicitudDetalle;

            var idSolicitud = _context.SolicitudDetalles
                .Where(sd => sd.IdSolicitudDetalle == idSolicitudDetalle)
                .Select(sd => sd.IdSolicitud)
                .FirstOrDefault();

            return RedirectToAction("Analyze", new { idSolicitud });
        }

        // Colsulte

        // GET: ComprasUsuarios
        public async Task<IActionResult> ConsulteReview(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 40)
        {

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            IQueryable<Compra> query = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation);


            // Verify if the user has the appropriate roles
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                var userName = User.Identity.Name;
                query = query.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

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
                query = query.Where(s => s.FechaCompra >= fechaInicio && s.FechaCompra < fechaFin);
            }

            query = query.OrderBy(c => c.IdCompra);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ConsulteReviewComprasViewModel
            {
                Compras = paginatedItems,
                Pagination = new PaginationComprasConsulteReview
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }


        public IActionResult ExportExcelR()
        {
            IQueryable<Compra> query = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation);


            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                var userName = User.Identity.Name;
                query = query.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            var compras = query.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras");

                worksheet.Cell(1, 1).Value = "OC";
                worksheet.Cell(1, 2).Value = "Fecha O.C";
                worksheet.Cell(1, 3).Value = "Fecha de Entrega";
                worksheet.Cell(1, 4).Value = "Estado de la orden";
                worksheet.Cell(1, 5).Value = "Link OC";
                worksheet.Cell(1, 6).Value = "Observaciones";
                worksheet.Cell(1, 7).Value = "Solicitud";
                worksheet.Cell(1, 8).Value = "Vendedor";
                worksheet.Cell(1, 9).Value = "Proveedor";
                worksheet.Cell(1, 10).Value = "Usuario";

                int row = 2;
                foreach (var compra in compras)
                {
                    worksheet.Cell(row, 1).Value = compra.OrdenCompra;
                    worksheet.Cell(row, 2).Value = compra.FechaCompra;
                    worksheet.Cell(row, 3).Value = compra.FechaEntrega;
                    worksheet.Cell(row, 4).Value = compra.Estado;
                    worksheet.Cell(row, 5).Value = compra.Observaciones;
                    worksheet.Cell(row, 6).Value = compra.ObservacionesPago;
                    worksheet.Cell(row, 7).Value = compra.IdSolicitudNavigation.Consecutivo;
                    worksheet.Cell(row, 8).Value = compra.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                    worksheet.Cell(row, 9).Value = compra.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                    worksheet.Cell(row, 10).Value = compra.IdUsuarioNavigation.Username;
                    row++;
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Ordencompras_review{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
        }

        public async Task<IActionResult> ConsulteDetails(int idSolicitud, string searchTerm, string searchCriteria, DateTime? fecha)
        {

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            // Consulta base para las solicitudes
            var solicitudesQuery = _context.Solicituds
                .Include(s => s.Compras)
                .Include(s => s.SolicitudDetalles)
                .ThenInclude(sd => sd.IdProductoNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdVendedorNavigation)
                .AsQueryable();

            // Consulta base para las compras
            var comprasQuery = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .AsQueryable();

            // Verify if the user has the appropriate roles
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras" && userArea != "Bodega"))
            {
                var userName = User.Identity.Name;
                comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            // Filtrar según searchTerm y searchCriteria
            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        comprasQuery = comprasQuery.Where(c => c.OrdenCompra.Contains(searchTerm));
                        break;
                    case "Solicitud":
                        solicitudesQuery = solicitudesQuery.Where(s => s.Consecutivo.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        solicitudesQuery = solicitudesQuery.Where(s => s.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Usuario":
                        solicitudesQuery = solicitudesQuery.Where(s => s.IdUsuarioNavigation.Username.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        solicitudesQuery = solicitudesQuery.Where(s => s.IdProveedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Estado":
                        comprasQuery = comprasQuery.Where(c => c.Estado.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            // Filtrar por fecha
            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                comprasQuery = comprasQuery.Where(c => c.FechaEntrega >= fechaInicio && c.FechaEntrega < fechaFin);
            }

            //comprasQuery = comprasQuery.Where(c => c.Estado == "Abierta");
            //solicitudesQuery = solicitudesQuery.Where(s => s.Estado == "OC Creada");

            // Obtener listas finales
            var solicitudes = await solicitudesQuery.ToListAsync();
            var compras = await comprasQuery.ToListAsync();
            var solicitudDetalles = await _context.SolicitudDetalles.ToListAsync();

            // Create the ViewModel
            var viewModel = new CompraViewDetalle
            {
                Solicitudes = solicitudes,
                Compras = compras,
                SolicitudDetalles = solicitudDetalles
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ExportExcelD()
        {
            // Consulta base para las solicitudes
            var solicitudesQuery = _context.Solicituds
                .Include(s => s.Compras)
                .Include(s => s.SolicitudDetalles)
                .ThenInclude(sd => sd.IdProductoNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdVendedorNavigation)
                .Where(s => s.Estado != "Anulado")
                .AsQueryable();

            // Consulta base para las compras
            var comprasQuery = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .AsQueryable();

            // Verificar roles del usuario
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                var userName = User.Identity.Name;
                comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            var solicitudes = await solicitudesQuery.ToListAsync();
            var solicitudDetalles = await _context.SolicitudDetalles.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras");

                worksheet.Cell(1, 1).Value = "Consecutivo";
                worksheet.Cell(1, 2).Value = "Fecha Solicitud";
                worksheet.Cell(1, 3).Value = "OC";
                worksheet.Cell(1, 4).Value = "Fecha Compra";
                worksheet.Cell(1, 5).Value = "Fecha Entrega";
                worksheet.Cell(1, 6).Value = "Estado Compra";
                worksheet.Cell(1, 7).Value = "Observaciones Compra";
                worksheet.Cell(1, 8).Value = "Vendedor";
                worksheet.Cell(1, 9).Value = "Proveedor";
                worksheet.Cell(1, 10).Value = "Referencia";
                worksheet.Cell(1, 11).Value = "Descripción";
                worksheet.Cell(1, 12).Value = "Cantidad";
                worksheet.Cell(1, 13).Value = "Precio Costo";
                worksheet.Cell(1, 14).Value = "Precio Venta";
                worksheet.Cell(1, 15).Value = "Rentabilidad";
                worksheet.Cell(1, 16).Value = "Estado Solicitud";

                int row = 2;
                foreach (var solicitud in solicitudes)
                {
                    var compras = solicitud.Compras.ToList();

                    if (compras.Any())
                    {
                        foreach (var compra in compras)
                        {
                            var detalles = solicitudDetalles.Where(d => d.IdSolicitud == compra.IdSolicitud).ToList();

                            foreach (var detalle in detalles)
                            {
                                worksheet.Cell(row, 1).Value = solicitud.Consecutivo;
                                worksheet.Cell(row, 2).Value = solicitud.Fecha;
                                worksheet.Cell(row, 3).Value = compra.OrdenCompra;
                                worksheet.Cell(row, 4).Value = compra.FechaCompra;
                                worksheet.Cell(row, 5).Value = compra.FechaEntrega;
                                worksheet.Cell(row, 6).Value = compra.Estado;
                                worksheet.Cell(row, 7).Value = compra.Observaciones;
                                worksheet.Cell(row, 8).Value = solicitud.IdVendedorNavigation.Nombre;
                                worksheet.Cell(row, 9).Value = solicitud.IdProveedorNavigation.Nombre;
                                worksheet.Cell(row, 10).Value = detalle.IdProductoNavigation.Referencia;
                                worksheet.Cell(row, 11).Value = detalle.IdProductoNavigation.Descripcion;
                                worksheet.Cell(row, 12).Value = detalle.Cantidad;
                                worksheet.Cell(row, 13).Value = detalle.PrecioCosto;
                                worksheet.Cell(row, 14).Value = detalle.PrecioVenta;
                                worksheet.Cell(row, 15).Value = detalle.Rentabilidad;
                                worksheet.Cell(row, 16).Value = solicitud.Estado;
                                row++;
                            }
                        }
                    }
                    else
                    {
                        // Si no hay compras asociadas, agregamos solo los detalles de la solicitud
                        foreach (var detalle in solicitud.SolicitudDetalles)
                        {
                            worksheet.Cell(row, 1).Value = solicitud.Consecutivo;
                            worksheet.Cell(row, 2).Value = solicitud.Fecha;
                            worksheet.Cell(row, 3).Value = ""; // Puedes decidir qué poner aquí, por ejemplo, dejarlo vacío
                            worksheet.Cell(row, 4).Value = "";
                            worksheet.Cell(row, 5).Value = "";
                            worksheet.Cell(row, 6).Value = "";
                            worksheet.Cell(row, 7).Value = "";
                            worksheet.Cell(row, 8).Value = solicitud.IdVendedorNavigation.Nombre;
                            worksheet.Cell(row, 9).Value = solicitud.IdProveedorNavigation.Nombre;
                            worksheet.Cell(row, 10).Value = detalle.IdProductoNavigation.Referencia;
                            worksheet.Cell(row, 11).Value = detalle.IdProductoNavigation.Descripcion;
                            worksheet.Cell(row, 12).Value = detalle.Cantidad;
                            worksheet.Cell(row, 13).Value = detalle.PrecioCosto;
                            worksheet.Cell(row, 14).Value = detalle.PrecioVenta;
                            worksheet.Cell(row, 15).Value = detalle.Rentabilidad;
                            worksheet.Cell(row, 16).Value = solicitud.Estado;
                            row++;
                        }
                    }
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Compras_detalle{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }

        // GET: ComprasUsuarios
        public async Task<IActionResult> ConsulteDiscount(string searchTerm, string searchCriteria, DateTime? fecha)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            IQueryable<CompraDetalle> query = _context.CompraDetalles
                .Include(cd => cd.IdCompraNavigation)
                    .ThenInclude(c => c.IdSolicitudNavigation)
                        .ThenInclude(s => s.IdVendedorNavigation)
                .Include(cd => cd.IdCompraNavigation)
                    .ThenInclude(c => c.IdSolicitudNavigation)
                        .ThenInclude(s => s.IdProveedorNavigation)
                .Include(cd => cd.IdCompraNavigation)
                    .ThenInclude(c => c.IdSolicitudNavigation)
                        .ThenInclude(s => s.IdUsuarioNavigation)
                .Include(cd => cd.IdCompraNavigation)
                    .ThenInclude(c => c.IdUsuarioNavigation)
                .Include(cd => cd.IdSolicitudDetalleNavigation)
                    .ThenInclude(sd => sd.IdProductoNavigation);


            // Verify if the user has the appropriate roles
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                var userName = User.Identity.Name;
                query = query.Where(cd => cd.IdCompraNavigation.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(cd => cd.IdCompraNavigation.OrdenCompra.Contains(searchTerm));
                        break;
                    case "Solicitud":
                        query = query.Where(cd => cd.IdCompraNavigation.IdSolicitudNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        query = query.Where(cd => cd.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Usuario":
                        query = query.Where(cd => cd.IdCompraNavigation.IdSolicitudNavigation.IdUsuarioNavigation.Username.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        query = query.Where(cd => cd.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Estado":
                        query = query.Where(cd => cd.IdCompraNavigation.Estado.Contains(searchTerm));
                        break;
                    case "Referencia":
                        query = query.Where(cd => cd.IdSolicitudDetalleNavigation.IdProductoNavigation.Referencia.Contains(searchTerm));
                        break;
                    case "Descripcion":
                        query = query.Where(cd => cd.IdSolicitudDetalleNavigation.IdProductoNavigation.Descripcion.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(cd => cd.IdCompraNavigation.FechaCompra >= fechaInicio && cd.IdCompraNavigation.FechaCompra < fechaFin);
            }

            query = query.OrderBy(c => c.IdCompra);

            var compraDetalles = await query.ToListAsync();

            return View(compraDetalles);
        }


        // POST: Method by dowload Excel the data in the list ConsulteReview
        public IActionResult ExportExcelDs()
        {
            IQueryable<CompraDetalle> query = _context.CompraDetalles
                .Include(cd => cd.IdCompraNavigation)
                    .ThenInclude(c => c.IdSolicitudNavigation)
                        .ThenInclude(s => s.IdVendedorNavigation)
                .Include(cd => cd.IdCompraNavigation)
                    .ThenInclude(c => c.IdSolicitudNavigation)
                        .ThenInclude(s => s.IdProveedorNavigation)
                .Include(cd => cd.IdCompraNavigation)
                    .ThenInclude(c => c.IdSolicitudNavigation)
                        .ThenInclude(s => s.IdUsuarioNavigation)
                .Include(cd => cd.IdCompraNavigation)
                    .ThenInclude(c => c.IdUsuarioNavigation)
                .Include(cd => cd.IdSolicitudDetalleNavigation)
                    .ThenInclude(sd => sd.IdProductoNavigation);


            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                var userName = User.Identity.Name;
                query = query.Where(cd => cd.IdCompraNavigation.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }
            var compraDetalle = query.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras_Discount");

                worksheet.Cell(1, 1).Value = "Solicitud";
                worksheet.Cell(1, 2).Value = "O.C";
                worksheet.Cell(1, 3).Value = "Fecha O.C";
                worksheet.Cell(1, 4).Value = "Fecha Entrega";
                worksheet.Cell(1, 5).Value = "Referencia";
                worksheet.Cell(1, 6).Value = "Descripcion";
                worksheet.Cell(1, 7).Value = "Cantidad";
                worksheet.Cell(1, 8).Value = "Costo";
                worksheet.Cell(1, 9).Value = "Proveedor Sugerido";
                worksheet.Cell(1, 10).Value = "Cantidad";
                worksheet.Cell(1, 11).Value = "Precio Costo";
                worksheet.Cell(1, 12).Value = "Ahorro";
                worksheet.Cell(1, 13).Value = "Proveedor Sugerido 2";
                worksheet.Cell(1, 14).Value = "Cantidad 2";
                worksheet.Cell(1, 15).Value = "Precio Costo 2";
                worksheet.Cell(1, 16).Value = "Ahorro 2";

                int row = 2;
                foreach (var comprasDetalle in compraDetalle)
                {
                    worksheet.Cell(row, 1).Value = comprasDetalle.IdCompraNavigation.IdSolicitudNavigation.Consecutivo;
                    worksheet.Cell(row, 2).Value = comprasDetalle.IdCompraNavigation.OrdenCompra;
                    worksheet.Cell(row, 3).Value = comprasDetalle.IdCompraNavigation.FechaCompra;
                    worksheet.Cell(row, 4).Value = comprasDetalle.IdCompraNavigation.FechaEntrega;
                    worksheet.Cell(row, 5).Value = comprasDetalle.IdSolicitudDetalleNavigation.IdProductoNavigation.Referencia;
                    worksheet.Cell(row, 6).Value = comprasDetalle.IdSolicitudDetalleNavigation.IdProductoNavigation.Descripcion;
                    worksheet.Cell(row, 7).Value = comprasDetalle.IdSolicitudDetalleNavigation.Cantidad;
                    worksheet.Cell(row, 8).Value = comprasDetalle.IdSolicitudDetalleNavigation.PrecioCosto;
                    worksheet.Cell(row, 9).Value = comprasDetalle.ProveedorSugerido;
                    worksheet.Cell(row, 10).Value = comprasDetalle.Cantidad;
                    worksheet.Cell(row, 11).Value = comprasDetalle.PrecioUnitario;
                    worksheet.Cell(row, 12).Value = comprasDetalle.Ahorro;
                    worksheet.Cell(row, 13).Value = comprasDetalle.ProveedorSugerido1;
                    worksheet.Cell(row, 14).Value = comprasDetalle.Cantidad1;
                    worksheet.Cell(row, 15).Value = comprasDetalle.PrecioUnitario1;
                    worksheet.Cell(row, 16).Value = comprasDetalle.Ahorro1;
                    row++;
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Compra_discount_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
        }


        // Method main by consulte specific area "BODEGA"
        public async Task<IActionResult> ConsulteDetailsBodega(int idSolicitud, string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras" && userArea != "Bodega"))
            {
                return RedirectToAction("Index", "Main");
            }
            // Consulta base para las solicitudes
            var solicitudesQuery = _context.Solicituds
                .Include(s => s.Compras)
                .Include(s => s.SolicitudDetalles)
                .ThenInclude(sd => sd.IdProductoNavigation)
                .AsQueryable();

            // Consulta base para las compras
            var comprasQuery = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .AsQueryable();

            // Verify if the user has the appropriate roles
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras" && userArea != "Bodega"))
            {
                var userName = User.Identity.Name;
                comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            // Filtrar según searchTerm y searchCriteria
            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        comprasQuery = comprasQuery.Where(c => c.OrdenCompra.Contains(searchTerm));
                        break;
                    case "Solicitud":
                        comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Usuario":
                        comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdProveedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Estado":
                        comprasQuery = comprasQuery.Where(c => c.Estado.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            // Filtrar por fecha
            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                comprasQuery = comprasQuery.Where(c => c.FechaEntrega >= fechaInicio && c.FechaEntrega < fechaFin);
            }

            comprasQuery = comprasQuery.Where(c => c.Estado == "Abierta");
            solicitudesQuery = solicitudesQuery.Where(s => s.Estado == "OC Creada");

            // Obtener listas finales
            var solicitudes = await solicitudesQuery.ToListAsync();
            var compras = await comprasQuery.ToListAsync();
            var solicitudDetalles = await _context.SolicitudDetalles.ToListAsync();


            int totalItems = await comprasQuery.CountAsync();
            var paginatedItems = await comprasQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Crear el ViewModel
            var viewModel = new CompraViewBodega
            {
                Solicitudes = solicitudes,
                Compras = paginatedItems,
                SolicitudDetalles = solicitudDetalles,
                Pagination = new PaginationViewModelBodega
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ExportExcelDBodega()
        {
            // Consulta base para las compras
            var comprasQuery = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .AsQueryable();


            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras" && userArea != "Bodega"))
            {
                var userName = User.Identity.Name;
                comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            comprasQuery = comprasQuery.Where(c => c.Estado == "Abierta");
            comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.Estado == "OC Creada");

            var compras = await comprasQuery.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras");

                worksheet.Cell(1, 1).Value = "Consecutivo";
                worksheet.Cell(1, 2).Value = "OC";
                worksheet.Cell(1, 3).Value = "Fecha Compra";
                worksheet.Cell(1, 4).Value = "Fecha Entrega";
                worksheet.Cell(1, 5).Value = "Estado Compra";
                worksheet.Cell(1, 6).Value = "Observaciones Compra";
                worksheet.Cell(1, 7).Value = "Vendedor";
                worksheet.Cell(1, 8).Value = "Proveedor";
                worksheet.Cell(1, 9).Value = "Referencia";
                worksheet.Cell(1, 10).Value = "Descripción";
                worksheet.Cell(1, 11).Value = "Cantidad";

                int row = 2;
                foreach (var compra in compras)
                {
                    var detalles = _context.SolicitudDetalles
                        .Include(d => d.IdProductoNavigation)
                        .Where(d => d.IdSolicitud == compra.IdSolicitud)
                        .ToList();

                    foreach (var detalle in detalles)
                    {
                        worksheet.Cell(row, 1).Value = compra.IdSolicitudNavigation.Consecutivo;
                        worksheet.Cell(row, 2).Value = compra.OrdenCompra;
                        worksheet.Cell(row, 3).Value = compra.FechaCompra;
                        worksheet.Cell(row, 4).Value = compra.FechaEntrega;
                        worksheet.Cell(row, 5).Value = compra.Estado;
                        worksheet.Cell(row, 6).Value = compra.ObservacionesBodega;
                        worksheet.Cell(row, 7).Value = compra.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                        worksheet.Cell(row, 8).Value = compra.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                        worksheet.Cell(row, 9).Value = detalle.IdProductoNavigation.Referencia;
                        worksheet.Cell(row, 10).Value = detalle.IdProductoNavigation.Descripcion;
                        worksheet.Cell(row, 11).Value = detalle.Cantidad;
                        row++;
                    }
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Entregas_compras{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }

        public async Task<IActionResult> ConsulteDetailsOC(int idSolicitud, string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {


            // Consulta base para las solicitudes
            var solicitudesQuery = _context.Solicituds
                .Include(s => s.Compras)
                .Include(s => s.SolicitudDetalles)
                .ThenInclude(sd => sd.IdProductoNavigation)
                .AsQueryable();

            // Consulta base para las compras
            var comprasQuery = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .AsQueryable();

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                var userName = User.Identity.Name;
                comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            // Filtrar según searchTerm y searchCriteria
            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        comprasQuery = comprasQuery.Where(c => c.OrdenCompra.Contains(searchTerm));
                        break;
                    case "Solicitud":
                        comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Usuario":
                        comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdProveedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Estado":
                        comprasQuery = comprasQuery.Where(c => c.Estado.Contains(searchTerm));
                        break;
                    default:
                        break;
                }
            }

            // Filtrar por fecha
            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                comprasQuery = comprasQuery.Where(c => c.FechaEntrega >= fechaInicio && c.FechaEntrega < fechaFin);
            }

            solicitudesQuery = solicitudesQuery.Where(s => s.Estado == "OC Creada");
            comprasQuery = comprasQuery.OrderByDescending(c => c.IdSolicitud);

            // Obtener listas finales
            var solicitudes = await solicitudesQuery.ToListAsync();
            var compras = await comprasQuery.ToListAsync();
            var solicitudDetalles = await _context.SolicitudDetalles.ToListAsync();

            int totalItems = await comprasQuery.CountAsync();
            var paginatedItems = await comprasQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Crear el ViewModel
            var viewModel = new CompraViewOC
            {
                Solicitudes = solicitudes,
                Compras = paginatedItems,
                SolicitudDetalles = solicitudDetalles,
                Pagination = new PaginationViewModelOC
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ExportExcelDOC()
        {
            // Consulta base para las compras
            var comprasQuery = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .AsQueryable();


            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                var userName = User.Identity.Name;
                comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            comprasQuery = comprasQuery.Where(c => c.Estado == "Abierta");
            //comprasQuery = comprasQuery.Where(c => c.IdSolicitudNavigation.Estado == "OC Creada");

            comprasQuery = comprasQuery.OrderByDescending(c => c.IdSolicitud);
            var compras = await comprasQuery.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras");

                // Título centrado y combinado en las columnas A-N
                var titleCell = worksheet.Range("A1:K1").Merge();
                titleCell.Value = "Solicitudes - Compras";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.Black;

                // Encabezados de columna
                var headers = new string[]
                {
                    "Consecutivo", "OC", "Fecha Compra", "Fecha Entrega",
                    "Estado Compra", "Link Orden de Compra", "Vendedor", "Proveedor",
                    "Referencia", "Descripción", "Cantidad"
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

                int row = 3;
                foreach (var compra in compras)
                {
                    var detalles = _context.SolicitudDetalles
                        .Include(d => d.IdProductoNavigation)
                        .Where(d => d.IdSolicitud == compra.IdSolicitud)
                        .ToList();

                    foreach (var detalle in detalles)
                    {
                        worksheet.Cell(row, 1).Value = compra.IdSolicitudNavigation.Consecutivo;
                        worksheet.Cell(row, 2).Value = compra.OrdenCompra;
                        worksheet.Cell(row, 3).Value = compra.FechaCompra;
                        worksheet.Cell(row, 4).Value = compra.FechaEntrega;
                        worksheet.Cell(row, 5).Value = compra.Estado;
                        worksheet.Cell(row, 6).Value = compra.LinkOc;
                        worksheet.Cell(row, 7).Value = compra.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                        worksheet.Cell(row, 8).Value = compra.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                        worksheet.Cell(row, 9).Value = detalle.IdProductoNavigation.Referencia;
                        worksheet.Cell(row, 10).Value = detalle.IdProductoNavigation.Descripcion;
                        worksheet.Cell(row, 11).Value = detalle.Cantidad;

                        for (int col = 1; col <= 11; col++)
                        {
                            worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }

                        row++;
                    }
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Solicitud_Compra_Detalle_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }

        // Analyze
        public async Task<IActionResult> AnalyzeCompras(string asesor = null, string proveedor = null, string usuario = null)
        {
            var userName = User.Identity.Name;

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userArea != "Compras"))
            {
                return RedirectToAction("Index", "Main");
            }

            // Obtener todas las solicitudes
            IQueryable<Compra> compras = _context.Compras
                .Where(c => c.Estado != "Anulado");

            // Aplicar filtro de usuario si el rol y el nombre de usuario no cumplen con los requisitos
            if (userRole != "Administrador" && userRole != "Super Usuario" &&
                userRole != "Gerencia" && userRole != "Desarrollador" &&
                userRole != "Soporte TI" && userRole != "Editor de Informacion" && userArea != "Compras" && userArea != "Bodega")
            {
                compras = compras.Where(c => c.IdUsuarioNavigation.Username == userName);
            }

            // Filtrar por asesor, proveedor y usuario si se proporcionan
            if (!string.IsNullOrEmpty(asesor))
            {
                compras = compras.Where(c => c.IdSolicitudNavigation.IdVendedorNavigation.Nombre == asesor);
            }

            if (!string.IsNullOrEmpty(proveedor))
            {
                compras = compras.Where(c => c.IdSolicitudNavigation.IdProveedorNavigation.Nombre == proveedor);
            }


            if (!string.IsNullOrEmpty(usuario))
            {
                compras = compras.Where(c => c.IdUsuarioNavigation.Nombre == usuario);
            }

            // Obtener el total de las compras
            var totalCompras = await compras.CountAsync();

            // Obtener solicitudes por mes
            var comprasPorMes = await compras
                .GroupBy(c => new { c.FechaCompra.Value.Year, c.FechaCompra.Value.Month })
                .Select(g => new CompraPorMes
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();


            var proveedores = await compras
                .Select(c => c.IdSolicitudNavigation.IdProveedorNavigation.Nombre)
                .Distinct()
                .ToListAsync();


            // Contar solicitudes por usuario y asesor
            var comprasPorUsuario = await compras
                .GroupBy(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Nombre)
                .Select(g => new CompraPorUsuario
                {
                    Usuario = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            var comprasPorProveedor = await compras
                .GroupBy(c => c.IdSolicitudNavigation.IdProveedorNavigation.Nombre)
                .Select(g => new CompraPorProveedor
                {
                    Proveedor = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            var comprasPorAsesor = await compras
                .GroupBy(c => c.IdSolicitudNavigation.IdVendedorNavigation.Nombre)
                .Select(g => new CompraPorAsesor
                {
                    Asesor = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();



            // Obtener total de compras por mes
            var comprasValorPorMes = await compras
                .SelectMany(c => c.IdSolicitudNavigation.SolicitudDetalles)
                .GroupBy(sd => new { sd.IdSolicitudNavigation.Fecha.Value.Year, sd.IdSolicitudNavigation.Fecha.Value.Month })
                .Select(g => new CompraValorPorMes
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalValor = g.Sum(sd => (decimal)sd.Cantidad * (decimal)sd.PrecioCosto)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            // Obtener total de compras por proveedor
            var comprasValorPorProveedor = await compras
                .SelectMany(c => c.IdSolicitudNavigation.SolicitudDetalles)
                .GroupBy(sd => sd.IdSolicitudNavigation.IdProveedorNavigation.Nombre)
                .Select(g => new CompraValorPorProveedor
                {
                    Proveedor = g.Key,
                    TotalValor = g.Sum(sd => (decimal)sd.Cantidad * (decimal)sd.PrecioCosto)
                })
                .OrderByDescending(g => g.TotalValor)
                .Take(15)
                .ToListAsync();

            // Obtener total de compras por usuario
            var comprasValorPorUsuario = await compras
                .SelectMany(c => c.IdSolicitudNavigation.SolicitudDetalles)
                .GroupBy(sd => sd.IdSolicitudNavigation.IdVendedorNavigation.Nombre)
                .Select(g => new CompraValorPorUsuario
                {
                    Usuario = g.Key,
                    TotalValor = g.Sum(sd => (decimal)sd.Cantidad * (decimal)sd.PrecioCosto)
                })
                .OrderBy(g => g.TotalValor)
                .ToListAsync();

            var viewModel = new AnalyzeCompraViewModel
            {
                // Pasar los datos a la vista
                TotalCompras = totalCompras,
                ComprasPorMes = comprasPorMes,
                Asesores = await compras.Select(c => c.IdSolicitudNavigation.IdVendedorNavigation.Nombre).Distinct().ToListAsync(),
                Usuarios = await compras.Select(c => c.IdSolicitudNavigation.IdUsuarioNavigation.Nombre).Distinct().ToListAsync(),
                ComprasPorUsuario = comprasPorUsuario,
                ComprasPorProveedor = comprasPorProveedor,
                ComprasPorAsesor = comprasPorAsesor,
                ComprasValorPorMes = comprasValorPorMes,
                ComprasValorPorProveedor = comprasValorPorProveedor,
                ComprasValorPorUsuario = comprasValorPorUsuario
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadPDF(string asesor = null, string usuario = null, string chartImage = null)
        {
            var result = await AnalyzeCompras(asesor, usuario) as ViewResult;
            if (result != null)
            {
                var model = result.Model as AnalyzeCompraViewModel;
                if (model != null)
                {
                    ViewBag.ChartImage = chartImage;
                    return new ViewAsPdf("AnalyzeComprasPDF", model)
                    {
                        FileName = $"Analisis_compras_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf"
                    };
                }
            }
            // Handle the error case appropriately, e.g., return an error view or a message
            return RedirectToAction("AnalyzeCompra", new { asesor, usuario });
        }

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.IdCompra == id);
        }

        private bool CompraDetalleExists(int id)
        {
            return _context.CompraDetalles.Any(e => e.IdCompraDetalle == id);
        }

        private bool SolicitudExists(int id)
        {
            return _context.Solicituds.Any(e => e.IdSolicitud == id);
        }
    }
}
