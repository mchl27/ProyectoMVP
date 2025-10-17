using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Management_system.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing.Printing;
using Management_system.Models.Other.ViewModel.Logistica;
using Management_system.Models.Others.Analyze;

namespace Management_system.Controllers
{
    public class RecepcionUsuariosController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public RecepcionUsuariosController(DbManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 50)
        {
            IQueryable<RecepcionMercancium> query = _context.RecepcionMercancia
                .Include(r => r.IdCompraNavigation)
                .ThenInclude(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .Include(r => r.IdCompraNavigation)
                .ThenInclude(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdVendedorNavigation)
                .Include(r => r.IdUsuarioNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(c => c.Estado != "Abierta" && c.Estado != "Anulado");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Ordendecompra":
                        query = query.Where(r => r.IdCompraNavigation.OrdenCompra.Contains(searchTerm) || r.IdCompraNavigation.OrdenCompra1.Contains(searchTerm) || r.IdCompraNavigation.OrdenCompra2.Contains(searchTerm));
                        break;
                    case "Solicitud":
                        query = query.Where(r => r.IdCompraNavigation.IdSolicitudNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        query = query.Where(r => r.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Usuario":
                        query = query.Where(r => r.IdUsuarioNavigation.Username.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        query = query.Where(r => r.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation.Nombre.Contains(searchTerm));
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
                query = query.Where(s => s.FechaRecepcion >= fechaInicio && s.FechaRecepcion < fechaFin);
            }

            query = query.OrderByDescending(r => r.FechaRecepcion);


            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new RecepcionViewModel
            {
                Recepciones = paginatedItems,
                Pagination = new PaginationViewModelRecepcion
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
            var recepcions = _context.RecepcionMercancia
                .Include(r => r.IdCompraNavigation)
                .Include(r => r.IdCompraNavigation.IdSolicitudNavigation)
                .Include(r => r.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(r => r.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(r => r.IdCompraNavigation.IdSolicitudNavigation)
                .Include(r => r.IdUsuarioNavigation);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras");

                worksheet.Cell(1, 1).Value = "OC";
                worksheet.Cell(1, 2).Value = "Solicitud";
                worksheet.Cell(1, 3).Value = "Proveedor";
                worksheet.Cell(1, 4).Value = "vendedor";
                worksheet.Cell(1, 5).Value = "Usuario";
                worksheet.Cell(1, 6).Value = "Fecha Recpeción";
                worksheet.Cell(1, 7).Value = "Estado";
                worksheet.Cell(1, 8).Value = "Observaciones";

                int row = 2;
                foreach (var recepcion in recepcions)
                {
                    worksheet.Cell(row, 1).Value = recepcion.IdCompraNavigation.OrdenCompra;
                    worksheet.Cell(row, 2).Value = recepcion.IdCompraNavigation.IdSolicitudNavigation.Consecutivo;
                    worksheet.Cell(row, 3).Value = recepcion.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                    worksheet.Cell(row, 4).Value = recepcion.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                    worksheet.Cell(row, 5).Value = recepcion.IdUsuarioNavigation.Username;
                    worksheet.Cell(row, 6).Value = recepcion.FechaRecepcion;
                    worksheet.Cell(row, 7).Value = recepcion.Estado;
                    worksheet.Cell(row, 8).Value = recepcion.Observaciones;
                    row++;
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Recepcion_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
        }

        // GET:ReceptionDetalle / Details
        public IActionResult Details(int idSolicitud)
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

        // GET: RecepcionUsuarios/Create
        public IActionResult Create()
        {
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: RecepcionUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRecepcion,IdCompra,IdUsuario,FechaRecepcion,Estado,Observaciones")] RecepcionMercancium recepcionMercancium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recepcionMercancium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", recepcionMercancium.IdCompra);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", recepcionMercancium.IdUsuario);
            return View(recepcionMercancium);
        }

        // GET: RecepcionUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionMercancium = await _context.RecepcionMercancia.FindAsync(id);
            if (recepcionMercancium == null)
            {
                return NotFound();
            }

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                // If the user has admin, developer or support roles, display all users
                var userName = User.Identity.Name;
                ViewData["IdUsuarioAct"] = new SelectList(_context.Usuarios.Where(u => u.Username == userName), "Nombre", "Username");
            }
            else
            {
                // If the user has admin, developer or support roles, display all users
                ViewData["IdUsuarioAct"] = new SelectList(_context.Usuarios, "Nombre", "Username");
            }

            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", recepcionMercancium.IdCompra);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", recepcionMercancium.IdUsuario);
            return View(recepcionMercancium);
        }

        // POST: RecepcionUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRecepcion, IdCompra, IdUsuario, FechaRecepcion, Estado, Observaciones, DocumentoRecibido, NumeroDocumento, FechaActualizacion, Observaciones1, Observaciones2, Observaciones3")] RecepcionMercancium recepcionMercancium)
        {
            if (id != recepcionMercancium.IdRecepcion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    recepcionMercancium.FechaActualizacion = DateTime.Now;
                    _context.Update(recepcionMercancium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionMercanciumExists(recepcionMercancium.IdRecepcion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var idSolicitud = _context.Compras
                .Where(c => c.IdCompra == recepcionMercancium.IdCompra)
                .Select(c => c.IdSolicitud)
                .FirstOrDefault();

                //  Redirect to ReceptionDetail with the request id
                return RedirectToAction("EditDetalle", new { idSolicitud });
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", recepcionMercancium.IdCompra);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username", recepcionMercancium.IdUsuario);
            return View(recepcionMercancium);
        }

        // This method is essential to set the final status of the application
        [HttpPost]
        public async Task<IActionResult> UpdateStatusEdit(int idRecepcion, string nuevoEstado)
        {
            var recepcion = await _context.RecepcionMercancia.FindAsync(idRecepcion);
            if (recepcion == null)
            {
                return NotFound();
            }

            // Validar estado
            if (!new[] { "Recibida", "Parcial", "En revision", "Rechazada" }.Contains(nuevoEstado))
            {
                return BadRequest("Estado no válido.");
            }

            recepcion.Estado = nuevoEstado;
            recepcion.FechaActualizacion = DateTime.Now;
            _context.Update(recepcion);

            var compra = await _context.Compras.FindAsync(recepcion.IdCompra);
            if (compra != null)
            {
                //Update the state of the purchase based on the received state
                if (recepcion.Estado == "Recibida")
                {
                    compra.Estado = "Recibido";
                }
                else if (recepcion.Estado == "Parcial")
                {
                    compra.Estado = "Parcial";
                }
                else if (recepcion.Estado == "En revision")
                {
                    compra.Estado = "En revision";
                }
                else if (recepcion.Estado == "Rechazada")
                {
                    compra.Estado = "Mercancia Rechazada";
                }

                _context.Update(compra);

            }

            await _context.SaveChangesAsync();

            // Redirigir al índice después de actualizar el estado
            return RedirectToAction("Index");
        }

        // GET:RecepcionUsuarios / EditDetalle
        public IActionResult EditDetalle(int idSolicitud)
        {
            var receptionViewModel = new ReceptionViewModel();

            // Obtain the purchase
            var compra = _context.Compras
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefault(c => c.IdSolicitud == idSolicitud);

            if (compra == null)
            {
                return NotFound();
            }

            receptionViewModel.NuevaCompra = compra;

            // Obtain the application
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

            // Obtain application details
            var solicitudDetalles = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .Where(sd => sd.IdSolicitud == idSolicitud)
                .ToList();

            receptionViewModel.DetallesSolicitud = solicitudDetalles;

            // Obtain purchase details
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

        // GET: RecepcionUsuarios/ EditProducto
        public IActionResult EditProducto(int idSolicitud, int idRecepcion, int idSolicitudDetalle)
        {
            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idSolicitud);

            if (solicitud == null)
            {
                return NotFound();
            }

            ViewData["IdSolicitud"] = idSolicitud;

            var recepcion = _context.RecepcionMercancia
                .Include(r => r.IdCompraNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefault(r => r.IdRecepcion == idRecepcion);

            if (recepcion == null)
            {
                return NotFound();
            }

            ViewData["Recepcion"] = idRecepcion;

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
            return View();
        }

        // POST: RecepcionUsuarios/ EditProducto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProducto([Bind("IdRecepcionDetalle,IdRecepcion,IdSolicitudDetalle,CantidadRecibida,EstadoProducto,Observaciones")] RecepcionDetalle recepcionDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recepcionDetalle);
                await _context.SaveChangesAsync();

                var idSolicitudDetalle = recepcionDetalle.IdSolicitudDetalle;


                var idSolicitud = _context.SolicitudDetalles
                    .Where(sd => sd.IdSolicitudDetalle == idSolicitudDetalle)
                    .Select(sd => sd.IdSolicitud)
                    .FirstOrDefault();

                return RedirectToAction("EditDetalle", new { idSolicitud });
            }

            return View(recepcionDetalle);
        }

        // GET: RecepcionUsuarios/ EditProductoEdit/5
        public async Task<IActionResult> EditProductoEdit(int? id, int? idsolicitud)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionDetalle = await _context.RecepcionDetalles.FindAsync(id);
            if (recepcionDetalle == null)
            {
                return NotFound();
            }
            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idsolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            var solicitudDetalle = await _context.SolicitudDetalles
                .Include(sd => sd.IdSolicitudNavigation)
                .Include(sd => sd.IdProductoNavigation)
                .FirstOrDefaultAsync(sd => sd.IdSolicitudDetalle == recepcionDetalle.IdSolicitudDetalle);
            if (solicitudDetalle == null)
            {
                return NotFound();
            }

            ViewData["Referencia"] = solicitudDetalle.IdProductoNavigation.Referencia;
            ViewData["Descripcion"] = solicitudDetalle.IdProductoNavigation.Descripcion;
            ViewData["Cantidad"] = solicitudDetalle.Cantidad;

            ViewData["IdSolicitud"] = idsolicitud;
            ViewData["IdRecepcion"] = new SelectList(_context.RecepcionMercancia, "IdRecepcion", "IdRecepcion", recepcionDetalle.IdRecepcion);
            ViewData["IdSolicitudDetalle"] = new SelectList(_context.SolicitudDetalles, "IdSolicitudDetalle", "IdProducto", recepcionDetalle.IdRecepcion);
            return View(recepcionDetalle);
        }


        // POST: RecepcionUsuarios/ EditProductoEditt/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductoEdit(int id, [Bind("IdRecepcionDetalle,IdRecepcion,IdSolicitudDetalle,CantidadRecibida,EstadoProducto,Observaciones")] RecepcionDetalle recepcionDetalle)
        {
            if (id != recepcionDetalle.IdRecepcionDetalle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recepcionDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionDetalleExists(recepcionDetalle.IdRecepcionDetalle))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var idSolicitudDetalle = recepcionDetalle.IdSolicitudDetalle;

                var idSolicitud = _context.SolicitudDetalles
                    .Where(sd => sd.IdSolicitudDetalle == idSolicitudDetalle)
                    .Select(sd => sd.IdSolicitud)
                    .FirstOrDefault();

                return RedirectToAction("EditDetalle", new { idSolicitud });
            }
            ViewData["IdRecepcion"] = new SelectList(_context.RecepcionMercancia, "IdRecepcion", "IdRecepcion", recepcionDetalle.IdRecepcion);
            ViewData["IdSolicitudDetalle"] = new SelectList(_context.SolicitudDetalles, "IdSolicitudDetalle", "IdProducto", recepcionDetalle.IdRecepcion);
            return View(recepcionDetalle);
        }

        // GET: RecepcionUsuarios/ EditProductoDelete/5
        public async Task<IActionResult> EditProductoDelete(int? id, int? idsolicitud)
        {
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

            var recepcionDetalle = await _context.RecepcionDetalles
                .Include(rd => rd.IdSolicitudDetalleNavigation)
                .Include(rd => rd.IdRecepcionNavigation)
                .FirstOrDefaultAsync(rd => rd.IdRecepcionDetalle == id);
            if (recepcionDetalle == null)
            {
                return NotFound();
            }

            return View(recepcionDetalle);
        }

        // POST: RecepcionUsuarios/ EditProductoDelete/5
        [HttpPost, ActionName("EditProductoDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductoDeleteConfirmed(int id)
        {

            var recepcionDetalle = await _context.RecepcionDetalles.FindAsync(id);
            if (recepcionDetalle != null)
            {
                _context.RecepcionDetalles.Remove(recepcionDetalle);
            }
            await _context.SaveChangesAsync();

            var idSolicitudDetalle = recepcionDetalle.IdSolicitudDetalle;

            var idSolicitud = _context.SolicitudDetalles
                .Where(sd => sd.IdSolicitudDetalle == idSolicitudDetalle)
                .Select(sd => sd.IdSolicitud)
                .FirstOrDefault();

            return RedirectToAction("EditDetalle", new { idSolicitud });
        }

        // GET: RecepcionUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionMercancium = await _context.RecepcionMercancia
                .Include(r => r.IdCompraNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdRecepcion == id);
            if (recepcionMercancium == null)
            {
                return NotFound();
            }

            return View(recepcionMercancium);
        }

        // POST: RecepcionUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recepcionMercancium = await _context.RecepcionMercancia.FindAsync(id);
            if (recepcionMercancium != null)
            {
                _context.RecepcionMercancia.Remove(recepcionMercancium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // User's Views

        // GET: VistaSolicitudController
        public async Task<IActionResult> ReviewIndex(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 90)
        {
            IQueryable<Compra> query = _context.Compras
                .Include(c => c.IdSolicitudNavigation)
                .Include(c => c.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(c => c.IdSolicitudNavigation.IdUsuarioNavigation)
                .Include(c => c.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation);


            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            {
                var userName = User.Identity.Name;
                query = query.Where(c => c.Estado!= "Recibido" && c.Estado != "Anulado" && c.Estado != "En revision" && c.Estado != "Parcial");
            }

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Ordendecompra":
                        query = query.Where(c => c.OrdenCompra.Contains(searchTerm) || c.OrdenCompra1.Contains(searchTerm) || c.OrdenCompra2.Contains(searchTerm));
                        break;
                    case "Solicitud":
                        query = query.Where(c => c.IdSolicitudNavigation.Consecutivo.Contains(searchTerm));
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
            
            query = query.OrderBy(c => c.IdCompra);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ComprasReviewViewModel
            {
                Compras = paginatedItems,
                Pagination = new PaginationViewModelComprasReview
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }


        // GET: RecepcionUsuario/ReviewDetails
        public async Task<IActionResult> ReviewDetails(int? id)
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

            ViewData["IdSolicitud"] = solicitud.IdSolicitud;
            ViewData["Fecha"] = solicitud.Fecha;
            ViewData["IdCliente"] = solicitud.IdClienteNavigation.Nombre;
            ViewData["IdProveedor"] = solicitud.IdProveedorNavigation.Nombre;
            ViewData["IdUsuario"] = solicitud.IdUsuarioNavigation.Username;
            ViewData["IdVendedor"] = solicitud.IdVendedorNavigation.Nombre;
            ViewData["Estado"] = solicitud.Estado;
            ViewData["Consecutivo"] = solicitud.Consecutivo;
            ViewData["Observacion"] = solicitud.Observaciones;

            var solicitudDetalles = _context.SolicitudDetalles
                .Include(sd => sd.IdProductoNavigation)
                .Where(sd => sd.IdSolicitud == id)
                .ToList();

            ViewData["SolicitudDetalles"] = solicitudDetalles;

            var compra = _context.Compras
                .Include(s => s.IdSolicitudNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .FirstOrDefault(s => s.IdSolicitudNavigation.IdSolicitud == id);

            if (compra == null)
            {
                return NotFound();
            }

            ViewData["IdCompra"] = compra.IdCompra;
            ViewData["OrdenCompra"] = compra.OrdenCompra;
            ViewData["IdUsuarioC"] = compra.IdUsuarioNavigation.Username;
            ViewData["FechaCompra"] = compra.FechaCompra;
            ViewData["FechaEntrega"] = compra.FechaEntrega;
            ViewData["EstadoC"] = compra.Estado;
            ViewData["ObservacionesC"] = compra.Observaciones;

            return View();
        }


        // GET: RecepcionUsuarios/Reception
        public IActionResult Reception(int? idcompra, int? idsolicitud)
        {
            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idsolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            ViewData["IdSolicitud"] = idsolicitud;

            var compra = _context.Compras.FirstOrDefault(c => c.IdCompra == idcompra);
            if (compra == null)
            {
                return NotFound();
            }

            ViewData["IdCompra"] = idcompra;

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
            return View();
        }

        // POST: RecepcionUsuarios/Reception
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reception([Bind("IdRecepcion,IdCompra,IdUsuario,FechaRecepcion,Estado,Observaciones")] RecepcionMercancium recepcionMercancium)
        {
            if (ModelState.IsValid)
            {
                recepcionMercancium.FechaRecepcion = DateTime.Now;
                recepcionMercancium.Estado = "En Revision";
                _context.Add(recepcionMercancium);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                //var compra = await _context.Compras.FindAsync(recepcionMercancium.IdCompra);
                //if (compra != null)
                //{
                //    Update the state of the purchase based on the received state
                //    if (recepcionMercancium.Estado == "Recibida")
                //    {
                //        compra.Estado = "Recibido";
                //    }
                //    else if (recepcionMercancium.Estado == "Parcial")
                //    {
                //        compra.Estado = "Parcial";
                //    }

                //    _context.Update(compra);
                //    await _context.SaveChangesAsync();
                //}

                var idSolicitud = _context.Compras
                .Where(c => c.IdCompra == recepcionMercancium.IdCompra)
                .Select(c => c.IdSolicitud)
                .FirstOrDefault();

                // Redirigir a ReceptionDetalle con el id de la solicitud
                return RedirectToAction("ReceptionDetalle", new { idSolicitud });
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "IdCompra", recepcionMercancium.IdCompra);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", recepcionMercancium.IdUsuario);
            return View(recepcionMercancium);
        }

        // GET:ReceptionDetalle / Create
        public IActionResult ReceptionDetalle(int idSolicitud)
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

        // This method is essential to set the final status of the application
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int idRecepcion, string nuevoEstado)
        {
            var recepcion = await _context.RecepcionMercancia.FindAsync(idRecepcion);
            if (recepcion == null)
            {
                return NotFound();
            }

            // Validar estado
            if (!new[] { "Recibida", "Parcial", "En revision", "Rechazada" }.Contains(nuevoEstado))
            {
                return BadRequest("Estado no válido.");
            }

            recepcion.Estado = nuevoEstado;
            recepcion.FechaRecepcion = DateTime.Now;
            _context.Update(recepcion);

            var compra = await _context.Compras.FindAsync(recepcion.IdCompra);
            if (compra != null)
            {
                //Update the state of the purchase based on the received state
                if (recepcion.Estado == "Recibida")
                {
                    compra.Estado = "Recibido";
                }
                else if (recepcion.Estado == "Parcial")
                {
                    compra.Estado = "Parcial";
                }
                else if (recepcion.Estado == "En revision")
                {
                    compra.Estado = "En revision";
                }
                else if (recepcion.Estado == "Rechazada")
                {
                    compra.Estado = "Mercancia Rechazada";
                }

                _context.Update(compra);

            }

            await _context.SaveChangesAsync();

            // Redirigir al índice después de actualizar el estado
            return RedirectToAction("ReviewIndex");
        }

        // GET: RecepcionUsuarios / ReceptionProducto
        public IActionResult ReceptionProducto(int idSolicitud, int idSolicitudDetalle, int idRecepcion)
        {
            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idSolicitud);

            if (solicitud == null)
            {
                return NotFound();
            }

            ViewData["IdSolicitud"] = idSolicitud;

            var recepcion = _context.RecepcionMercancia
                .Include(r => r.IdCompraNavigation)
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefault(r => r.IdRecepcion == idRecepcion);

            if (recepcion == null)
            {
                return NotFound();
            }

            ViewData["Recepcion"] = idRecepcion;


            var solicitudDetalle = _context.SolicitudDetalles
                .Include(sd => sd.IdSolicitudNavigation)
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
            return View();
        }

        // POST: RecepcionUsuario / ReceptionProducto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceptionProducto([Bind("IdRecepcionDetalle,IdRecepcion,IdSolicitudDetalle,CantidadRecibida,EstadoProducto,Observaciones")] RecepcionDetalle recepcionDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recepcionDetalle);
                await _context.SaveChangesAsync();

                var idSolicitudDetalle = recepcionDetalle.IdSolicitudDetalle;


                var idSolicitud = _context.SolicitudDetalles
                    .Where(sd => sd.IdSolicitudDetalle == idSolicitudDetalle)
                    .Select(sd => sd.IdSolicitud)
                    .FirstOrDefault();

                return RedirectToAction("ReceptionDetalle", new { idSolicitud });
            }

            return View(recepcionDetalle);
        }

        // GET: RecepcionUsuarios/ ReceptionEdit/5
        public async Task<IActionResult> ReceptionEdit(int? id, int? idsolicitud)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recepcionDetalle = await _context.RecepcionDetalles.FindAsync(id);
            if (recepcionDetalle == null)
            {
                return NotFound();
            }
            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idsolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            var solicitudDetalle = await _context.SolicitudDetalles
                .Include(sd => sd.IdSolicitudNavigation)
                .Include(sd => sd.IdProductoNavigation)
                .FirstOrDefaultAsync(sd => sd.IdSolicitudDetalle == recepcionDetalle.IdSolicitudDetalle);
            if (solicitudDetalle == null)
            {
                return NotFound();
            }

            ViewData["Referencia"] = solicitudDetalle.IdProductoNavigation.Referencia;
            ViewData["Descripcion"] = solicitudDetalle.IdProductoNavigation.Descripcion;
            ViewData["Cantidad"] = solicitudDetalle.Cantidad;

            ViewData["IdSolicitud"] = idsolicitud;
            ViewData["IdRecepcion"] = new SelectList(_context.RecepcionMercancia, "IdRecepcion", "IdRecepcion", recepcionDetalle.IdRecepcion);
            ViewData["IdSolicitudDetalle"] = new SelectList(_context.SolicitudDetalles, "IdSolicitudDetalle", "IdProducto", recepcionDetalle.IdRecepcion);
            return View(recepcionDetalle);
        }

        // POST: RecepcionUsuarios/ ReceptionEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceptionEdit(int id, [Bind("IdRecepcionDetalle,IdRecepcion,IdSolicitudDetalle,CantidadRecibida,EstadoProducto,Observaciones")] RecepcionDetalle recepcionDetalle)
        {
            if (id != recepcionDetalle.IdRecepcionDetalle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recepcionDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecepcionDetalleExists(recepcionDetalle.IdRecepcionDetalle))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var idSolicitudDetalle = recepcionDetalle.IdSolicitudDetalle;

                var idSolicitud = _context.SolicitudDetalles
                    .Where(sd => sd.IdSolicitudDetalle == idSolicitudDetalle)
                    .Select(sd => sd.IdSolicitud)
                    .FirstOrDefault();

                return RedirectToAction("ReceptionDetalle", new { idSolicitud });
            }
            ViewData["IdRecepcion"] = new SelectList(_context.RecepcionMercancia, "IdRecepcion", "IdRecepcion", recepcionDetalle.IdRecepcion);
            ViewData["IdSolicitudDetalle"] = new SelectList(_context.SolicitudDetalles, "IdSolicitudDetalle", "IdProducto", recepcionDetalle.IdRecepcion);
            return View(recepcionDetalle);
        }

        // GET: RecepcionUsuarios/ ReceptionDelete/5
        public async Task<IActionResult> ReceptionDelete(int? id, int? idsolicitud)
        {
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

            var recepcionDetalle = await _context.RecepcionDetalles
                .Include(rd => rd.IdSolicitudDetalleNavigation)
                .Include(rd => rd.IdRecepcionNavigation)
                .FirstOrDefaultAsync(rd => rd.IdRecepcionDetalle == id);
            if (recepcionDetalle == null)
            {
                return NotFound();
            }

            return View(recepcionDetalle);
        }

        // POST: RecepcionUsuarios/ ReceptionDelete/5
        [HttpPost, ActionName("ReceptionDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceptionDeleteConfirmed(int id)
        {

            var recepcionDetalle = await _context.RecepcionDetalles.FindAsync(id);
            if (recepcionDetalle != null)
            {
                _context.RecepcionDetalles.Remove(recepcionDetalle);
            }
            await _context.SaveChangesAsync();

            var idSolicitudDetalle = recepcionDetalle.IdSolicitudDetalle;

            var idSolicitud = _context.SolicitudDetalles
                .Where(sd => sd.IdSolicitudDetalle == idSolicitudDetalle)
                .Select(sd => sd.IdSolicitud)
                .FirstOrDefault();

            return RedirectToAction("ReceptionDetalle", new { idSolicitud });
        }

        // Consulte
        // GET: RecepcionUsuarios
        public async Task<IActionResult> ConsulteReview(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 50)
        {
            IQueryable<RecepcionMercancium> query = _context.RecepcionMercancia
                .Include(r => r.IdCompraNavigation)
                .ThenInclude(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .Include(r => r.IdCompraNavigation)
                .ThenInclude(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdVendedorNavigation)
                .Include(r => r.IdUsuarioNavigation);

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchCriteria))
            {
                switch (searchCriteria)
                {
                    case "Consecutivo":
                        query = query.Where(r => r.IdCompraNavigation.OrdenCompra.Contains(searchTerm));
                        break;
                    case "Solicitud":
                        query = query.Where(r => r.IdCompraNavigation.IdSolicitudNavigation.Consecutivo.Contains(searchTerm));
                        break;
                    case "Vendedor":
                        query = query.Where(r => r.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre.Contains(searchTerm));
                        break;
                    case "Usuario":
                        query = query.Where(r => r.IdUsuarioNavigation.Username.Contains(searchTerm));
                        break;
                    case "Proveedor":
                        query = query.Where(r => r.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation.Nombre.Contains(searchTerm));
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
                query = query.Where(s => s.FechaRecepcion >= fechaInicio && s.FechaRecepcion < fechaFin);
            }

            var solicitudes = await query.ToListAsync();

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new RecepcionConsulteRViewModel
            {
                Recepciones = paginatedItems,
                Pagination = new PaginationViewModelRecepcionColsulteR
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        public IActionResult ExportExcelConsulteR()
        {
            var recepcions = _context.RecepcionMercancia
                .Include(r => r.IdCompraNavigation)
                .Include(r => r.IdCompraNavigation.IdSolicitudNavigation)
                .Include(r => r.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation)
                .Include(r => r.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation)
                .Include(r => r.IdCompraNavigation.IdSolicitudNavigation)
                .Include(r => r.IdUsuarioNavigation);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras");

                worksheet.Cell(1, 1).Value = "OC";
                worksheet.Cell(1, 2).Value = "Solicitud";
                worksheet.Cell(1, 3).Value = "Proveedor";
                worksheet.Cell(1, 4).Value = "vendedor";
                worksheet.Cell(1, 5).Value = "Usuario";
                worksheet.Cell(1, 6).Value = "Fecha Recpeción";
                worksheet.Cell(1, 7).Value = "Estado";
                worksheet.Cell(1, 8).Value = "Observaciones";

                int row = 2;
                foreach (var recepcion in recepcions)
                {
                    worksheet.Cell(row, 1).Value = recepcion.IdCompraNavigation.OrdenCompra;
                    worksheet.Cell(row, 2).Value = recepcion.IdCompraNavigation.IdSolicitudNavigation.Consecutivo;
                    worksheet.Cell(row, 3).Value = recepcion.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                    worksheet.Cell(row, 4).Value = recepcion.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                    worksheet.Cell(row, 5).Value = recepcion.IdUsuarioNavigation.Username;
                    worksheet.Cell(row, 6).Value = recepcion.FechaRecepcion;
                    worksheet.Cell(row, 7).Value = recepcion.Estado;
                    worksheet.Cell(row, 8).Value = recepcion.Observaciones;
                    row++;
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Recepcion_review_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
        }

        // PENDIENTE

        // POST: ExportExcelConsulteD
        public IActionResult ExportExcelConsulteD()
        {
            var recepciondetalles = _context.RecepcionDetalles
                .Include(rd => rd.IdRecepcionNavigation)
                    .ThenInclude(r => r.IdCompraNavigation)
                    .ThenInclude(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .Include(rd => rd.IdRecepcionNavigation)
                    .ThenInclude(r => r.IdCompraNavigation)
                    .ThenInclude(c => c.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdVendedorNavigation)
                .Include(rd => rd.IdRecepcionNavigation)
                    .ThenInclude(r => r.IdUsuarioNavigation)
                .Include(rd => rd.IdSolicitudDetalleNavigation)
                    .ThenInclude(sd => sd.IdProductoNavigation)
                .Include(rd => rd.IdSolicitudDetalleNavigation)
                    .ThenInclude(sd => sd.IdSolicitudNavigation);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Compras");

                worksheet.Cell(1, 1).Value = "OC";
                worksheet.Cell(1, 2).Value = "Solicitud";
                worksheet.Cell(1, 3).Value = "Proveedor";
                worksheet.Cell(1, 4).Value = "vendedor";
                worksheet.Cell(1, 5).Value = "Usuario";
                worksheet.Cell(1, 6).Value = "Fecha Recpeción";
                worksheet.Cell(1, 7).Value = "Estado";
                worksheet.Cell(1, 8).Value = "Observaciones";
                worksheet.Cell(1, 9).Value = "Referencia";
                worksheet.Cell(1, 10).Value = "Descripcion";
                worksheet.Cell(1, 11).Value = "Cantidad";
                worksheet.Cell(1, 12).Value = "Cantidad Recibida";
                worksheet.Cell(1, 13).Value = "Estado del producto";
                worksheet.Cell(1, 14).Value = "Observaciones del producto";

                int row = 2;
                foreach (var recepciodetalle in recepciondetalles)
                {
                    worksheet.Cell(row, 1).Value = recepciodetalle.IdRecepcionNavigation.IdCompraNavigation.OrdenCompra;
                    worksheet.Cell(row, 2).Value = recepciodetalle.IdRecepcionNavigation.IdCompraNavigation.IdSolicitudNavigation.Consecutivo;
                    worksheet.Cell(row, 3).Value = recepciodetalle.IdRecepcionNavigation.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                    worksheet.Cell(row, 4).Value = recepciodetalle.IdRecepcionNavigation.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                    worksheet.Cell(row, 5).Value = recepciodetalle.IdRecepcionNavigation.IdUsuarioNavigation.Username;
                    worksheet.Cell(row, 6).Value = recepciodetalle.IdRecepcionNavigation.FechaRecepcion;
                    worksheet.Cell(row, 7).Value = recepciodetalle.IdRecepcionNavigation.Estado;
                    worksheet.Cell(row, 8).Value = recepciodetalle.IdRecepcionNavigation.Observaciones;
                    worksheet.Cell(row, 9).Value = recepciodetalle.IdSolicitudDetalleNavigation.IdProductoNavigation.Referencia;
                    worksheet.Cell(row, 10).Value = recepciodetalle.IdSolicitudDetalleNavigation.IdProductoNavigation.Descripcion;
                    worksheet.Cell(row, 11).Value = recepciodetalle.IdSolicitudDetalleNavigation.Cantidad;
                    worksheet.Cell(row, 12).Value = recepciodetalle.CantidadRecibida;
                    worksheet.Cell(row, 13).Value = recepciodetalle.EstadoProducto;
                    worksheet.Cell(row, 14).Value = recepciodetalle.Observaciones;
                    row++;
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Recepcion_details_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
        }


        public async Task<IActionResult> AnalyzeRecepcion(string asesor = null, string usuario = null)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            var userArea = User.FindFirst("Area")?.Value;

            IQueryable<RecepcionMercancium> recepciones = _context.RecepcionMercancia
                .Where(s => s.Estado != "Anulado");

            if (userRole != "Administrador" && userRole != "Super Usuario" &&
                userRole != "Gerencia" && userRole != "Desarrollador" &&
                userRole != "Soporte TI" && userArea != "Compras" && userArea != "Recepción de Mercancia")
            {
                recepciones = recepciones.Where(s => s.IdUsuarioNavigation.Username == userName);
            }

            if (!string.IsNullOrEmpty(asesor))
            {
                recepciones = recepciones.Where(r => r.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre == asesor)
                    .OrderBy(r => r.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre);
            }

            if (!string.IsNullOrEmpty(usuario))
            {
                recepciones = recepciones.Where(r => r.IdCompraNavigation.IdSolicitudNavigation.IdUsuarioNavigation.Nombre == usuario)
                    .OrderBy(r => r.IdCompraNavigation.IdSolicitudNavigation.IdUsuarioNavigation.Nombre);
            }

            var totalrecepciones = await recepciones.CountAsync();

            var recepcionPorUsuario = await recepciones
                .GroupBy(r => r.IdUsuarioNavigation.Nombre)
                .Select(g => new RecepcionPorUsuario
                {
                    Usuario = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            var recepcionPorAsesor = await recepciones
                .GroupBy(r => r.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre)
                .Select(g => new RecepcionPorAsesor
                {
                    Asesor = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            var recepcionesPorMes = await recepciones
                .GroupBy(s => new { s.FechaRecepcion.Value.Year, s.FechaRecepcion.Value.Month })
                .Select(g => new RecepcionesPorMes
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            var productosMasRecepcionados = await _context.RecepcionDetalles
                .Where(d => d.IdRecepcion.HasValue && recepciones.Select(r => r.IdRecepcion).Contains(d.IdRecepcion.Value))
                .GroupBy(d => new { d.IdSolicitudDetalleNavigation.IdProductoNavigation.Referencia, d.IdSolicitudDetalleNavigation.IdProductoNavigation.Descripcion })
                .Select(g => new ProductoMasRecepcionados
                {
                    Referencia = g.Key.Referencia,
                    Descripcion = g.Key.Descripcion,
                    TotalCantidad = g.Sum(d => (decimal)d.IdSolicitudDetalleNavigation.Cantidad * (decimal)d.IdSolicitudDetalleNavigation.PrecioCosto)
                })
                .OrderByDescending(g => g.TotalCantidad)
                .Take(20)
                .ToListAsync();

            var recepcionPorProveedor = await _context.RecepcionDetalles
                .Where(d => d.IdRecepcion.HasValue && recepciones.Select(r => r.IdRecepcion).Contains(d.IdRecepcion.Value))
                .GroupBy(d => d.IdRecepcionNavigation.IdCompraNavigation.IdSolicitudNavigation.IdProveedorNavigation.Nombre)
                .Select(g => new RecepcionPorProveedor
                {
                    Proveedor = g.Key,
                    TotalComprado = g.Sum(d => (decimal)d.IdSolicitudDetalleNavigation.Cantidad * (decimal)d.IdSolicitudDetalleNavigation.PrecioCosto)
                })
                .OrderByDescending(g => g.TotalComprado)
                .Take(10)
                .ToListAsync();


            var viewModel = new AnalyzeRecepcionViewModel
            {
                TotalRecepciones = totalrecepciones,
                RecepcionesPorMes = recepcionesPorMes,
                Asesores = await recepciones.Select(r => r.IdCompraNavigation.IdSolicitudNavigation.IdVendedorNavigation.Nombre).Distinct().ToListAsync(),
                Usuarios = await recepciones.Select(r => r.IdCompraNavigation.IdSolicitudNavigation.IdUsuarioNavigation.Nombre).Distinct().ToListAsync(),
                RecepcionPorAsesores = recepcionPorAsesor,
                RecepcionPorUsuarios = recepcionPorUsuario,
                RecepcionPorProveedores = recepcionPorProveedor,
                ProductoMasRecepcionados = productosMasRecepcionados,
            };

            return View(viewModel);

        }

        private bool RecepcionMercanciumExists(int id)
        {
            return _context.RecepcionMercancia.Any(r => r.IdRecepcion == id);
        }

        private bool RecepcionDetalleExists(int id)
        {
            return _context.RecepcionDetalles.Any(rd => rd.IdRecepcionDetalle == id);
        }
    }
}
