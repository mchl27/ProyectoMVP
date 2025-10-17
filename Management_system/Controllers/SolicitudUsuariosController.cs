using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Management_system.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using DocumentFormat.OpenXml.Wordprocessing;
using Management_system.Models.Others.Analyze;


namespace Management_system.Controllers
{
    public class SolicitudUsuariosController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public SolicitudUsuariosController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: SolicitudUsuarios
        public async Task<IActionResult> Index(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {
            IQueryable<Solicitud> query = _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                    .ThenInclude(u => u.IdEmpresaNavigation)
                .Include(s => s.IdVendedorNavigation)
                .Include(s => s.IdEmpresaNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" 
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(s => s.IdUsuarioNavigation.Username == userName);
                query = query.Where(s => s.Estado != "OC Creada" && s.Estado != "Anulado");
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
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(s => s.Fecha >= fechaInicio && s.Fecha < fechaFin);
            }

            query = query.OrderByDescending(s => s.IdSolicitud);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new SolicitudesIndexViewModel
            {
                Solicitudes = paginatedItems,
                Pagination = new PaginationViewModelSolicitudes
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }


        // POST: Method by dowload Excel the data in the list Index
        public IActionResult ExportarExcel()
        {
            var userRole = User.FindFirst("Rol")?.Value;
            var userName = User.Identity.Name;

            IQueryable<Solicitud> query = _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdUsuarioNavigation.IdEmpresaNavigation)
                .Include(s => s.IdVendedorNavigation);

            // Aplicar filtro de usuario si el rol y el nombre de usuario cumplen con los requisitos
            if (userRole != "Administrador" && userRole != "Super Usuario" && userRole != "Desarrollador" &&
                userRole != "Soporte TI")
            {
                query = query.Where(s => s.IdUsuarioNavigation.Username == userName);
            }

            var estadosPermitidos = new List<string> { "Abierta", "En proceso", "Revisar", "Revisada" };
            query = query.Where(s => estadosPermitidos.Contains(s.Estado));
            query = query.OrderBy(s => s.IdSolicitud);
            var solicitudes = query.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Solicitudes");

                // Título centrado y combinado en las columnas A-I
                var titleCell = worksheet.Range("A1:I1").Merge();
                titleCell.Value = "Solicitudes Activas";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.Black;

                // Encabezados de columna
                var headers = new string[]
                {
                    "Consecutivo", "Fecha", "Cliente", "Proveedor",
                    "Usuario", "Empresa", "Vendedor", "Estado", "Observaciones Compras"
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
                foreach (var solicitud in solicitudes)
                {
                    worksheet.Cell(row, 1).Value = solicitud.Consecutivo;
                    worksheet.Cell(row, 2).Value = solicitud.Fecha;
                    worksheet.Cell(row, 3).Value = solicitud.IdClienteNavigation.Nombre;
                    worksheet.Cell(row, 4).Value = solicitud.IdProveedorNavigation.Nombre;
                    worksheet.Cell(row, 5).Value = solicitud.IdUsuarioNavigation.Username;
                    worksheet.Cell(row, 6).Value = solicitud.IdUsuarioNavigation.IdEmpresaNavigation.Nombre;
                    worksheet.Cell(row, 7).Value = solicitud.IdVendedorNavigation.Nombre;
                    worksheet.Cell(row, 8).Value = solicitud.Estado;
                    worksheet.Cell(row, 9).Value = solicitud.Observaciones;

                    for (int col = 1; col <= 9; col++)
                    {
                        worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    row++;
                }

                // Ajustar automáticamente el ancho de las columnas
                worksheet.Columns().AdjustToContents();

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Solicitudes_Abiertas_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
        }


        // GET: SolicitudUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;

            var SolicitudQuery = _context.Solicituds
            .Include(s => s.IdClienteNavigation)
            .Include(s => s.IdProveedorNavigation)
            .Include(s => s.IdUsuarioNavigation)
            .Include(s => s.IdVendedorNavigation)
            .Include(s => s.IdEmpresaNavigation)
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

            var model = new SolicitudCompraDetailsViewModel
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
            .Include(s => s.IdEmpresaNavigation)
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

            var model = new SolicitudCompraPDFViewModel
            {
                Solicitudes = solicitud,
                SolicitudDetalles = solicitudDetalles
            };

            return new ViewAsPdf("DetailsPDF", model)
            {
                FileName = $"{solicitud.Consecutivo}_{solicitud.IdClienteNavigation.Nombre}.pdf"
            };
        }

        public IActionResult GenerarPDF1(int id)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;

            var SolicitudQuery = _context.Solicituds
            .Include(s => s.IdClienteNavigation)
            .Include(s => s.IdProveedorNavigation)
            .Include(s => s.IdUsuarioNavigation)
            .Include(s => s.IdVendedorNavigation)
            .Include(s => s.IdEmpresaNavigation)
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

            var model = new SolicitudCompraPDF1ViewModel
            {
                Solicitudes = solicitud,
                SolicitudDetalles = solicitudDetalles
            };

            return new ViewAsPdf("DetailsPDF1", model)
            {
                FileName = $"{solicitud.Consecutivo}_{solicitud.IdClienteNavigation.Nombre}.pdf"
            };
        }

        // GET: SolicitudUsuarios/Create
        public IActionResult Create()
        {
            // Verify if the user has the appropriate roles
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;
            var userEmpresa = User.FindFirst("Empresa")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Desarrollador" && userRole != "Soporte TI" &&
                 userRole != "Jefe de Proceso"))
            {
                // If the user does not have admin, developer or support roles, display only their user
                
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios.Where(u => u.Username == userName), "IdUsuario", "Username");

                ViewData["IdEmpresa"] = new SelectList(_context.Empresas.Where(e => e.Nombre == userEmpresa), "IdEmpresa", "Nombre");
            }
            else
            {
                // If the user has admin, developer or support roles, display all users
                //ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username");
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios.Where(u => u.Username == userName), "IdUsuario", "Username");
                ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");
            }


            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "Nombre");

            var vendedores = _context.Vendedors
                .Where(v => v.Estado == "Activo")
                .OrderBy(v => v.Nombre).ToList();
            ViewData["IdVendedor"] = new SelectList(vendedores, "IdVendedor", "Nombre");


            //ViewData["IdVendedor"] = new SelectList(_context.Vendedors, "IdVendedor", "Nombre");

            DateTime fechaActual = DateTime.Now;
            DateTime fechaMinima;

            if (fechaActual.Hour >= 17)
            {
                // Si son las 5 PM o después, establecer fecha mínima para el siguiente día a las 00:00 horas
                fechaMinima = fechaActual.Date.AddDays(1).AddHours(8);
            }
            else
            {
                // Si es antes de las 5 PM, agregar 5 horas
                fechaMinima = fechaActual.AddHours(5);
            }

            // Pasar la fecha mínima a la vista
            ViewBag.FechaMinima = fechaMinima.ToString("yyyy-MM-ddTHH:mm");

            return View();
        }

        // POST: SolicitudUsuarios/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdSolicitud,Fecha,IdCliente,IdProveedor,IdUsuario,IdVendedor,Estado,Consecutivo,Observaciones,ProveedorSugerido,FechaEntrega,Negociacion,FechaActualizacion,ObservacionesActualizacion,IdEmpresa,IdUsuarioAsignado,FechaModificacion,FechaOc,FechaRm,FechaCierre,IdUsuarioAprobado")] Solicitud solicitud)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        solicitud.Estado = "En proceso";
        //        solicitud.Fecha = DateTime.Now;

        //        // Contar cuántas solicitudes tiene esa empresa
        //        int cantidadSolicitudes = await _context.Solicituds
        //            .Where(s => s.IdEmpresa == solicitud.IdEmpresa)
        //            .CountAsync();

        //        // El siguiente consecutivo es cantidad + 1
        //        int siguienteConsecutivo = cantidadSolicitudes + 1;

        //        // Formatear el consecutivo con ceros a la izquierda
        //        solicitud.ConsecutivoPorEmpresa = $"SGC-COM-SOL-{siguienteConsecutivo.ToString("D5")}";


        //        _context.Add(solicitud);
        //        await _context.SaveChangesAsync();
        //        // Redirect to the CreateDetail view with the ID of the newly created request
        //        return RedirectToAction("CreateDetalle", new { idSolicitud = solicitud.IdSolicitud });
        //    }
        //    ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", solicitud.IdCliente);
        //    ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", solicitud.IdProveedor);
        //    ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", solicitud.IdUsuario);
        //    ViewData["IdVendedor"] = new SelectList(_context.Vendedors, "IdVendedor", "IdVendedor", solicitud.IdVendedor);
        //    ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");
        //    return View(solicitud);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSolicitud,Fecha,IdCliente,IdProveedor,IdUsuario,IdVendedor,Estado,Consecutivo,Observaciones,ProveedorSugerido,FechaEntrega,Negociacion,FechaActualizacion,ObservacionesActualizacion,IdEmpresa,IdUsuarioAsignado,FechaModificacion,FechaOc,FechaRm,FechaCierre,IdUsuarioAprobado")] Solicitud solicitud)
        {
            if (ModelState.IsValid)
            {
                // Validar que la empresa esté seleccionada
                if (solicitud.IdEmpresa == 0)
                {
                    ModelState.AddModelError("IdEmpresa", "Debe seleccionar una empresa válida.");
                }
                else
                {
                    solicitud.Estado = "En proceso";
                    solicitud.Fecha = DateTime.Now;

                    // Obtener cuántas solicitudes tiene esta empresa
                    int cantidadSolicitudes = await _context.Solicituds
                        .Where(s => s.IdEmpresa == solicitud.IdEmpresa)
                        .CountAsync();

                    // Generar el consecutivo formateado
                    int siguienteConsecutivo = cantidadSolicitudes + 1;
                    solicitud.ConsecutivoPorEmpresa = $"SGC-COM-SOL-{siguienteConsecutivo.ToString("D5")}";

                    // Verificar que no esté vacío (por seguridad adicional)
                    if (string.IsNullOrEmpty(solicitud.ConsecutivoPorEmpresa))
                    {
                        ModelState.AddModelError("", "No se pudo generar el consecutivo. Intente nuevamente.");
                    }
                    else
                    {
                        // Guardar en la base de datos
                        _context.Add(solicitud);
                        await _context.SaveChangesAsync();

                        // Redirigir al detalle
                        return RedirectToAction("CreateDetalle", new { idSolicitud = solicitud.IdSolicitud });
                    }
                }
            }

            // Recargar SelectLists en caso de error
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", solicitud.IdCliente);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", solicitud.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", solicitud.IdUsuario);
            ViewData["IdVendedor"] = new SelectList(_context.Vendedors, "IdVendedor", "IdVendedor", solicitud.IdVendedor);
            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre", solicitud.IdEmpresa);

            return View(solicitud);
        }

        // Método para la búsqueda de proveedor
        [HttpGet]
        public JsonResult SearchProveedor(string term)
        {
            var proveedor = _context.Proveedors
                .Where(p => p.Nombre.Contains(term) || p.Nit.ToString().Contains(term))
                .Select(p => new {
                    value = p.IdProveedor,
                    text = p.Nit + " - " + p.Nombre
                })
                .ToList();

            return Json(proveedor);
        }

        // Método para la búsqueda de cliente
        [HttpGet]
        public JsonResult SearchCliente(string term)
        {
            var cliente = _context.Clientes
                .Where(c => c.Nombre.Contains(term) || c.Nit.ToString().Contains(term))
                .Select(c => new {
                    value = c.IdCliente,
                    text = c.Nit + " - " + c.Nombre
                })
                .ToList();

            return Json(cliente);
        }


        // GET: SolicitudUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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

            var empresa = await _context.Empresas
                .Where(e => e.IdEmpresa == solicitud.IdEmpresa)
                .Select(e => new { e.IdEmpresa, e.Nombre })
                .FirstOrDefaultAsync();

            ViewData["IdCliente"] = new SelectList(new List<object> { new { cliente.IdCliente, cliente.Nombre } }, "IdCliente", "Nombre", solicitud.IdCliente);
            ViewData["IdProveedor"] = new SelectList(new List<object> { new { proveedor.IdProveedor, proveedor.Nombre } }, "IdProveedor", "Nombre", solicitud.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(new List<object> { new { usuario.IdUsuario, usuario.Username } }, "IdUsuario", "Username", solicitud.IdUsuario);
            ViewData["IdVendedor"] = new SelectList(_context.Vendedors, "IdVendedor", "Nombre", solicitud.IdVendedor);
            ViewData["IdEmpresa"] = new SelectList(new List<object> { new { empresa.IdEmpresa, empresa.Nombre } }, "IdEmpresa", "Nombre");


            return View(solicitud);
        }

        // POST: SolicitudUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSolicitud,Fecha,IdCliente,IdProveedor,IdUsuario,IdVendedor,Estado,Consecutivo,Observaciones,ProveedorSugerido,FechaEntrega,Negociacion,FechaActualizacion,ObservacionesActualizacion,IdEmpresa,IdUsuarioAsignado,FechaModificacion,FechaOc,FechaRm,FechaCierre,ConsecutivoPorEmpresa,IdUsuarioAprobado")] Solicitud solicitud)
        {
            if (id != solicitud.IdSolicitud)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    solicitud.Estado = "Revisar";
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
                return RedirectToAction("EditDetalle", new { idSolicitud = solicitud.IdSolicitud });
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", solicitud.IdCliente);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", solicitud.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", solicitud.IdUsuario);
            ViewData["IdVendedor"] = new SelectList(_context.Vendedors, "IdVendedor", "IdVendedor", solicitud.IdVendedor);
            return View(solicitud);
        }

        // Método para la búsqueda de proveedor
        [HttpGet]
        public JsonResult SearchProveedorEdit(string term)
        {
            var proveedor = _context.Proveedors
                .Where(p => p.Nombre.Contains(term) || p.Nit.ToString().Contains(term))
                .Select(p => new {
                    value = p.IdProveedor,
                    text = p.Nit + " - " + p.Nombre
                })
                .ToList();

            return Json(proveedor);
        }

        // Método para la búsqueda de cliente
        [HttpGet]
        public JsonResult SearchClienteEdit(string term)
        {
            var cliente = _context.Clientes
                .Where(c => c.Nombre.Contains(term) || c.Nit.ToString().Contains(term))
                .Select(c => new {
                    value = c.IdCliente,
                    text = c.Nit + " - " + c.Nombre
                })
                .ToList();

            return Json(cliente);
        }


        // GET: SolicitudUsuarios/Delete/5 Remember that this code is alone to use administrator
        public async Task<IActionResult> Delete(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                // Redirect the user to the main home page if they do not have the appropriate roles
                return RedirectToAction("Index", "Main");
            }
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdVendedorNavigation)
                .FirstOrDefaultAsync(m => m.IdSolicitud == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // POST: SolicitudUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitud = await _context.Solicituds.FindAsync(id);
            if (solicitud != null)
            {
                _context.Solicituds.Remove(solicitud);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // User's View 

        //GET: CreateDetalle / Details - This View is after of the method POST: Create
        public IActionResult CreateDetalle(int idSolicitud)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;

            var SolicitudQuery = _context.Solicituds
            .Include(s => s.IdClienteNavigation)
            .Include(s => s.IdProveedorNavigation)
            .Include(s => s.IdUsuarioNavigation)
            .Include(s => s.IdVendedorNavigation)
            .Include(s => s.IdEmpresaNavigation)
            .Where(s => s.IdSolicitud == idSolicitud);

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
                .Where(sd => sd.IdSolicitud == idSolicitud)
                .ToList();

            var model = new SolicitudCompraIndexViewModel
            {
                Solicitudes = solicitud,
                SolicitudDetalles = solicitudDetalles
            };

            return View(model);
        }

        // This method is essential to set the final status of the application
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int idSolicitud, string nuevoEstado)
        {
            var solicitud = await _context.Solicituds.FindAsync(idSolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            solicitud.Estado = nuevoEstado;
            solicitud.Fecha = DateTime.Now;
            _context.Update(solicitud);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = solicitud.IdSolicitud });
        }


        // GET: CreateProducto/create - This view is for viewing the product of the company its caracteristics and other specific aspects 
        public IActionResult CreateProducto(int idSolicitud)
        {
            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idSolicitud);

            if (solicitud == null)
            {
                return NotFound();
            }

            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Referencia");
            ViewData["IdSolicitud"] = idSolicitud;

            return View();
        }

        // POST: CreateProducto/create - This view is for agree product to the "Specific Solicitud"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProducto([Bind("IdSolicitudDetalle,IdSolicitud,IdProducto,Observaciones,Cantidad,PrecioCosto,PrecioVenta,Rentabilidad,Negociacion,ObservacionCompras")] SolicitudDetalle solicitudDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solicitudDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction("CreateDetalle", new { idSolicitud = solicitudDetalle.IdSolicitud });
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Referencia", solicitudDetalle.IdProducto);
            ViewData["IdSolicitud"] = solicitudDetalle.IdSolicitud;
            return View(solicitudDetalle);
        }

        // Método para la búsqueda de productos
        [HttpGet]
        public JsonResult SearchProducto(string term)
        {
            var productos = _context.Productos
                .Where(p => p.Referencia.Contains(term) || p.Descripcion.Contains(term))
                .Select(p => new {
                    value = p.IdProducto,
                    text = p.Referencia + " - " + p.Descripcion
                })
                .ToList();

            return Json(productos);
        }

        public IActionResult GetProductDescription(int id)
        {
            var product = _context.Productos.FirstOrDefault(p => p.IdProducto == id);

            if (product == null)
            {
                return NotFound();
            }

            return Json(new { description = product.Descripcion,
                              unidad = product.Unidad
            });
        }

        // GET: SolicitudUsuarios / CreateProductoEdit/5 - This view allows you to edit product already added
        public async Task<IActionResult> CreateProductoEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitudDetalle = await _context.SolicitudDetalles.FindAsync(id);
            if (solicitudDetalle == null)
            {
                return NotFound();
            }

            // Solo carga la referencia del producto asociado inicialmente
            var producto = await _context.Productos
                .Where(p => p.IdProducto == solicitudDetalle.IdProducto)
                .Select(p => new { p.IdProducto, p.Referencia })
                .FirstOrDefaultAsync();

            ViewData["IdProducto"] = new SelectList(new List<object> { new { producto.IdProducto, producto.Referencia } }, "IdProducto", "Referencia", solicitudDetalle.IdProducto);
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", solicitudDetalle.IdSolicitud);

            return View(solicitudDetalle);
        }

        // POST: SolicitudUsuarios / CreateProductoEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductoEdit(int id, [Bind("IdSolicitudDetalle,IdSolicitud,IdProducto,Observaciones,Cantidad,PrecioCosto,PrecioVenta,Rentabilidad,Negociacion,ObservacionCompras")] SolicitudDetalle solicitudDetalle)
        {
            if (id != solicitudDetalle.IdSolicitudDetalle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitudDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitudDetalleExists(solicitudDetalle.IdSolicitudDetalle))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("CreateDetalle", new { idSolicitud = solicitudDetalle.IdSolicitud });
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", solicitudDetalle.IdProducto);
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", solicitudDetalle.IdSolicitud);
            return View(solicitudDetalle);
        }

        // Método para la búsqueda de productos
        [HttpGet]
        public JsonResult SearchProductoEdit(string term)
        {
            var productos = _context.Productos
                .Where(p => p.Referencia.Contains(term) || p.Descripcion.Contains(term))
                .Select(p => new {
                    value = p.IdProducto,
                    text = p.Referencia + " - " + p.Descripcion
                })
                .ToList();

            return Json(productos);
        }

        public IActionResult GetProductDescriptionE(int id)
        {
            var product = _context.Productos.FirstOrDefault(p => p.IdProducto == id);

            if (product == null)
            {
                return NotFound();
            }

            return Json(new { description = product.Descripcion,
                              unidad = product.Unidad
            });
        }

        // GET: SolicitudUsuarios / CreateProductoDelete/5
        public async Task<IActionResult> CreateProductoDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitudDetalle = await _context.SolicitudDetalles
                .Include(s => s.IdProductoNavigation)
                .Include(s => s.IdSolicitudNavigation)
                .FirstOrDefaultAsync(m => m.IdSolicitudDetalle == id);
            if (solicitudDetalle == null)
            {
                return NotFound();
            }

            return View(solicitudDetalle);
        }

        // POST: SolicitudUsuarios / CreateProductoDelete/5
        [HttpPost, ActionName("CreateProductoDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductoDeleteDeleteConfirmed(int id)
        {

            var solicitudDetalle = await _context.SolicitudDetalles.FindAsync(id);
            if (solicitudDetalle != null)
            {
                _context.SolicitudDetalles.Remove(solicitudDetalle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("CreateDetalle", new { idSolicitud = solicitudDetalle.IdSolicitud });
        }

        //GET: SolicitudUsuarios / EditDetalle/Details
        public IActionResult EditDetalle(int idSolicitud)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;

            var SolicitudQuery = _context.Solicituds
            .Include(s => s.IdClienteNavigation)
            .Include(s => s.IdProveedorNavigation)
            .Include(s => s.IdUsuarioNavigation)
            .Include(s => s.IdVendedorNavigation)
            .Include(s => s.IdEmpresaNavigation)
            .Where(s => s.IdSolicitud == idSolicitud);

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
                .Where(sd => sd.IdSolicitud == idSolicitud)
                .ToList();

            var model = new SolicitudCompraEditViewModel
            {
                Solicitudes = solicitud,
                SolicitudDetalles = solicitudDetalles
            };

            return View(model);
        }

        // This method is essential to set the final status of the application
        [HttpPost]
        public async Task<IActionResult> UpdateStatusEdit(int idSolicitud, string nuevoEstado)
        {
            var solicitud = await _context.Solicituds.FindAsync(idSolicitud);
            if (solicitud == null)
            {
                return NotFound();
            }

            solicitud.Estado = nuevoEstado;
            solicitud.FechaActualizacion = DateTime.Now;
            _context.Update(solicitud);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = solicitud.IdSolicitud });
        }


        // GET: SolicitudUsuarios / EditProducto/create
        public IActionResult EditProducto(int idSolicitud)
        {
            var solicitud = _context.Solicituds.FirstOrDefault(s => s.IdSolicitud == idSolicitud);

            if (solicitud == null)
            {
                return NotFound();
            }

            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Referencia");
            ViewData["IdSolicitud"] = idSolicitud;

            return View();
        }

        // Método para la búsqueda de productos
        [HttpGet]
        public JsonResult SearchEProducto(string term)
        {
            var productos = _context.Productos
                .Where(p => p.Referencia.Contains(term) || p.Descripcion.Contains(term))
                .Select(p => new {
                    value = p.IdProducto,
                    text = p.Referencia + " - " + p.Descripcion
                })
                .ToList();

            return Json(productos);
        }

        public IActionResult EGetProductDescription(int id)
        {
            var product = _context.Productos.FirstOrDefault(p => p.IdProducto == id);

            if (product == null)
            {
                return NotFound();
            }

            return Json(new { description = product.Descripcion,
                              unidad = product.Unidad
            });
        }

        // POST: SolicitudUsuarios / EditProducto/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProducto([Bind("IdSolicitudDetalle,IdSolicitud,IdProducto,Observaciones,Cantidad,PrecioCosto,PrecioVenta,Rentabilidad,Negociacion,ObservacionCompras")] SolicitudDetalle solicitudDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solicitudDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction("EditDetalle", new { idSolicitud = solicitudDetalle.IdSolicitud });
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Referencia", solicitudDetalle.IdProducto);
            ViewData["IdSolicitud"] = solicitudDetalle.IdSolicitud;
            return View(solicitudDetalle);
        }

        // GET: SolicitudUsuarios / EditProductoEdit/Edit/5
        public async Task<IActionResult> EditProductoEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitudDetalle = await _context.SolicitudDetalles.FindAsync(id);
            if (solicitudDetalle == null)
            {
                return NotFound();
            }

            // Only loads the associated product reference initially
            var producto = await _context.Productos
                .Where(p => p.IdProducto == solicitudDetalle.IdProducto)
                .Select(p => new { p.IdProducto, p.Referencia })
                .FirstOrDefaultAsync();

            ViewData["IdProducto"] = new SelectList(new List<object> { new { producto.IdProducto, producto.Referencia } }, "IdProducto", "Referencia", solicitudDetalle.IdProducto);
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", solicitudDetalle.IdSolicitud);
            return View(solicitudDetalle);
        }

        // Método para la búsqueda de productos
        [HttpGet]
        public JsonResult SearchEProductoEdit(string term)
        {
            var productos = _context.Productos
                .Where(p => p.Referencia.Contains(term) || p.Descripcion.Contains(term))
                .Select(p => new {
                    value = p.IdProducto,
                    text = p.Referencia + " - " + p.Descripcion
                })
                .ToList();

            return Json(productos);
        }

        public IActionResult EGetProductDescriptionE(int id)
        {
            var product = _context.Productos.FirstOrDefault(p => p.IdProducto == id);

            if (product == null)
            {
                return NotFound();
            }

            return Json(new { description = product.Descripcion,
                              unidad = product.Unidad
            });
        }

        // POST: SolicitudUsuarios / EditProductoEdit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductoEdit(int id, [Bind("IdSolicitudDetalle,IdSolicitud,IdProducto,Observaciones,Cantidad,PrecioCosto,PrecioVenta,Rentabilidad,Negociacion,ObservacionCompras")] SolicitudDetalle solicitudDetalle)
        {
            if (id != solicitudDetalle.IdSolicitudDetalle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitudDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitudDetalleExists(solicitudDetalle.IdSolicitudDetalle))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("EditDetalle", new { idSolicitud = solicitudDetalle.IdSolicitud });
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", solicitudDetalle.IdProducto);
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", solicitudDetalle.IdSolicitud);
            return View(solicitudDetalle);
        }

        // GET: SolicitudUsuarios / EditProductoDelete/Delete/5
        public async Task<IActionResult> EditProductoDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitudDetalle = await _context.SolicitudDetalles
                .Include(s => s.IdProductoNavigation)
                .Include(s => s.IdSolicitudNavigation)
                .FirstOrDefaultAsync(m => m.IdSolicitudDetalle == id);
            if (solicitudDetalle == null)
            {
                return NotFound();
            }

            return View(solicitudDetalle);
        }

        // POST: SolicitudUsuarios / EditProductoDelete/Delete/5
        [HttpPost, ActionName("EditProductoDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductoDeleteConfirmed(int id)
        {

            var solicitudDetalle = await _context.SolicitudDetalles.FindAsync(id);
            if (solicitudDetalle != null)
            {
                _context.SolicitudDetalles.Remove(solicitudDetalle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("EditDetalle", new { idSolicitud = solicitudDetalle.IdSolicitud });
        }

        // GET: SolicitudUsuarios/Overridemain
        public async Task<IActionResult> Overridemain(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 25)
        {

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" 
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                // Redirect the user to the main home page if they do not have the appropriate roles
                return RedirectToAction("Index", "Main");
            }

            IQueryable<Solicitud> query = _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdUsuarioNavigation.IdEmpresaNavigation)
                .Include(s => s.IdVendedorNavigation);

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
                    default:
                        break;
                }
            }
            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(s => s.Fecha >= fechaInicio && s.Fecha < fechaFin);
            }

            query = query.OrderByDescending(s => s.IdSolicitud);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new SolicitudesIndexViewModel
            {
                Solicitudes = paginatedItems,
                Pagination = new PaginationViewModelSolicitudes
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // GET: SolicitudUsuarios / Override/5
        public async Task<IActionResult> Override(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" 
                && userRole != "Desarrollador" && userRole != "Soporte TI"))
            {
                // Redirect the user to the main home page if they do not have the appropriate roles
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
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Nombre", solicitud.IdCliente);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "Nombre", solicitud.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Username", solicitud.IdUsuario);
            ViewData["IdVendedor"] = new SelectList(_context.Vendedors, "IdVendedor", "Nombre", solicitud.IdVendedor);
            return View(solicitud);
        }

        // POST: SolicitudUsuarios / Override/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Override(int id, [Bind("IdSolicitud,Fecha,IdCliente,IdProveedor,IdUsuario,IdVendedor,Estado,Consecutivo,Observaciones,ProveedorSugerido,FechaEntrega,Negociacion,FechaActualizacion,ObservacionesActualizacion,IdEmpresa,IdUsuarioAsignado,FechaModificacion,FechaOc,FechaRm,FechaCierre,IdUsuarioAprobado")] Solicitud solicitud)
        {
            if (id != solicitud.IdSolicitud)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Overridemain));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", solicitud.IdCliente);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", solicitud.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", solicitud.IdUsuario);
            ViewData["IdVendedor"] = new SelectList(_context.Vendedors, "IdVendedor", "IdVendedor", solicitud.IdVendedor);
            return View(solicitud);
        }


        // Consulte Review and details
        // GET: SolicitudUsuarios
        public async Task<IActionResult> ConsulteReview(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 40)
        {
            IQueryable<Solicitud> query = _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdUsuarioNavigation.IdEmpresaNavigation)
                .Include(s => s.IdVendedorNavigation);

            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                var userName = User.Identity.Name;
                query = query.Where(s => s.IdUsuarioNavigation.Username == userName);
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
                    default:
                        break;
                }
            }

            if (fecha != null)
            {
                DateTime fechaInicio = fecha.Value.Date;
                DateTime fechaFin = fechaInicio.AddDays(1).AddTicks(-1);
                query = query.Where(s => s.Fecha >= fechaInicio && s.Fecha < fechaFin);
            }

            query = query.OrderBy(s => s.IdSolicitud);
            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ConsulteReviewSolicitudesViewModel
            {
                Solicitudes = paginatedItems,
                Pagination = new PaginationlSolicitudesConsulteReview
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

            IQueryable<Solicitud> query = _context.Solicituds
                .Include(s => s.IdClienteNavigation)
                .Include(s => s.IdProveedorNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .Include(s => s.IdUsuarioNavigation.IdEmpresaNavigation)
                .Include(s => s.IdVendedorNavigation);

            // Aplicar filtro de usuario si el rol y el nombre de usuario cumplen con los requisitos
            if (userRole != "Administrador" && userRole != "Super Usuario" &&
                userRole != "Gerencia" && userRole != "Desarrollador" &&
                userRole != "Soporte TI")
            {
                query = query.Where(s => s.IdUsuarioNavigation.Username == userName);
            }

            var estadosPermitidos = new List<string> { "Abierta", "Legalizar", "Urgente", "OC Creada", "En proceso", "Revisar", "Revisada"};
            query = query.Where(s => estadosPermitidos.Contains(s.Estado));
            var solicitudes = query.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Solicitudes");

                // Título centrado y combinado en las columnas A-N
                var titleCell = worksheet.Range("A1:M1").Merge();
                titleCell.Value = "Solicitudes Resumen";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.Black;

                // Encabezados de columna
                var headers = new string[]
                {
                    "Consecutivo", "Fecha", "Fecha Entrega", "Cliente",
                    "Proveedor", "Usuario", "Vendedor", "Estado", "Negociación",
                    "Observaciones", "Proveedor sugerido", "Fecha Actualización", "Observacion Compras"
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
                foreach (var solicitud in solicitudes)
                {
                    worksheet.Cell(row, 1).Value = solicitud.Consecutivo;
                    worksheet.Cell(row, 2).Value = solicitud.Fecha;
                    worksheet.Cell(row, 3).Value = solicitud.FechaEntrega;
                    worksheet.Cell(row, 4).Value = solicitud.IdClienteNavigation.Nombre;
                    worksheet.Cell(row, 5).Value = solicitud.IdProveedorNavigation.Nombre;
                    worksheet.Cell(row, 6).Value = solicitud.IdUsuarioNavigation.Username;
                    worksheet.Cell(row, 7).Value = solicitud.IdVendedorNavigation.Nombre;
                    worksheet.Cell(row, 8).Value = solicitud.Estado;
                    worksheet.Cell(row, 9).Value = solicitud.Negociacion;
                    worksheet.Cell(row, 10).Value = solicitud.Observaciones;
                    worksheet.Cell(row, 11).Value = solicitud.ProveedorSugerido;
                    worksheet.Cell(row, 12).Value = solicitud.FechaActualizacion;
                    worksheet.Cell(row, 13).Value = solicitud.ObservacionesActualizacion;

                    for (int col = 1; col <= 13; col++)
                    {
                        worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    row++;
                }

                // Ajustar automáticamente el ancho de las columnas
                //worksheet.Columns().AdjustToContents();

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Solicitudes_Resumen_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
        }

        // GET: SolicitudUsuarios
        public async Task<IActionResult> ConsulteDetails(string searchTerm, string searchCriteria, DateTime? fecha, int pageNumber = 1, int pageSize = 40)
        {
            IQueryable<SolicitudDetalle> query = _context.SolicitudDetalles
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdClienteNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdUsuarioNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdVendedorNavigation)
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
                    case "Estado":
                        query = query.Where(sd => sd.IdSolicitudNavigation.Estado.Contains(searchTerm));
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

            query = query.OrderBy(s => s.IdSolicitud);

            int totalItems = await query.CountAsync();
            var paginatedItems = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ConsulteDetailsSolicitudesViewModel
            {
                Solicitudes = paginatedItems,
                Pagination = new PaginationlSolicitudesConsulteDetails
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // POST: Method by dowload Excel the data in the list ConsulteReview
        public IActionResult ExportExcelConsulteD()
        {
            var userRole = User.FindFirst("Rol")?.Value;
            var userName = User.Identity.Name;

            IQueryable<SolicitudDetalle> query = _context.SolicitudDetalles
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdClienteNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdProveedorNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdUsuarioNavigation)
                .Include(sd => sd.IdSolicitudNavigation)
                    .ThenInclude(s => s.IdVendedorNavigation)
                .Include(sd => sd.IdProductoNavigation);

            // Aplicar filtro de usuario si el rol y el nombre de usuario cumplen con los requisitos
            if (userRole != "Administrador" && userRole != "Super Usuario" &&
                userRole != "Gerencia" && userRole != "Desarrollador" &&
                userRole != "Soporte TI")
            {
                query = query.Where(sd => sd.IdSolicitudNavigation.IdUsuarioNavigation.Username == userName);
            }

            var estadosPermitidos = new List<string> { "Abierta", "Legalizar", "Urgente", "OC Creada", "En proceso", "Revisar", "Revisada" };
            query = query.Where(s => estadosPermitidos.Contains(s.IdSolicitudNavigation.Estado));
            var solicitudDetalle = query.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Solicitudes Detalle");


                // Título centrado y combinado en las columnas A-N
                var titleCell = worksheet.Range("A1:P1").Merge();
                titleCell.Value = "Solicitudes Detalle";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.White;
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Fill.BackgroundColor = XLColor.Black;


                // Encabezados de columna
                var headers = new string[]
                {
                    "Consecutivo", "Fecha", "Cliente", "Proveedor",
                    "Usuario", "Vendedor", "Estado", "Observaciones Compras", "Referencia",
                    "Descripcion", "Observaciones", "Cantidad", "Precio Costo", "Precio Venta",
                    "Rentabilidad","Negociación"
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
                foreach (var solicituddetalle in solicitudDetalle)
                {
                    worksheet.Cell(row, 1).Value = solicituddetalle.IdSolicitudNavigation.Consecutivo;
                    worksheet.Cell(row, 2).Value = solicituddetalle.IdSolicitudNavigation.Fecha;
                    worksheet.Cell(row, 3).Value = solicituddetalle.IdSolicitudNavigation.IdClienteNavigation.Nombre;
                    worksheet.Cell(row, 4).Value = solicituddetalle.IdSolicitudNavigation.IdProveedorNavigation.Nombre;
                    worksheet.Cell(row, 5).Value = solicituddetalle.IdSolicitudNavigation.IdUsuarioNavigation.Username;
                    worksheet.Cell(row, 6).Value = solicituddetalle.IdSolicitudNavigation.IdVendedorNavigation.Nombre;
                    worksheet.Cell(row, 7).Value = solicituddetalle.IdSolicitudNavigation.Estado;
                    worksheet.Cell(row, 8).Value = solicituddetalle.IdSolicitudNavigation.Observaciones;
                    worksheet.Cell(row, 9).Value = solicituddetalle.IdProductoNavigation.Referencia;
                    worksheet.Cell(row, 10).Value = solicituddetalle.IdProductoNavigation.Descripcion;
                    worksheet.Cell(row, 11).Value = solicituddetalle.Observaciones;
                    worksheet.Cell(row, 12).Value = solicituddetalle.Cantidad;
                    worksheet.Cell(row, 13).Value = solicituddetalle.PrecioCosto;
                    worksheet.Cell(row, 14).Value = solicituddetalle.PrecioVenta;
                    worksheet.Cell(row, 15).Value = solicituddetalle.Rentabilidad;
                    worksheet.Cell(row, 16).Value = solicituddetalle.Negociacion;

                    for (int col = 1; col <= 16; col++)
                    {
                        worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }

                    row++;
                }

                // Ajustar automáticamente el ancho de las columnas
                //worksheet.Columns().AdjustToContents();

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Solicitudes_detalle_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            }
        }

        // GET: AnalyzeSolicitud - View User
        public async Task<IActionResult> AnalyzeSolicitud123(string asesor = null, string usuario = null)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;

            IQueryable<Solicitud> solicitudes = _context.Solicituds;

            if (userRole != "Administrador" && userRole != "Super Usuario" &&
                userRole != "Gerencia" && userRole != "Desarrollador" &&
                userRole != "Soporte TI")
            {
                solicitudes = solicitudes.Where(s => s.IdUsuarioNavigation.Username == userName);
            }

            if (!string.IsNullOrEmpty(asesor))
            {
                solicitudes = solicitudes.Where(s => s.IdVendedorNavigation.Nombre == asesor);
            }

            if (!string.IsNullOrEmpty(usuario))
            {
                solicitudes = solicitudes.Where(s => s.IdUsuarioNavigation.Nombre == usuario);
            }

            var totalSolicitudes = await solicitudes.CountAsync();

            var solicitudesPorMes = await solicitudes
                .GroupBy(s => new { s.Fecha.Value.Year, s.Fecha.Value.Month })
                .Select(g => new SolicitudPorMes
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            var asesores = await solicitudes
                .Select(s => s.IdVendedorNavigation.Nombre)
                .Distinct()
                .ToListAsync();

            var usuarios = await solicitudes
                .Select(s => s.IdUsuarioNavigation.Nombre)
                .Distinct()
                .ToListAsync();

            var solicitudesPorUsuario = await solicitudes
                .GroupBy(s => s.IdUsuarioNavigation.Nombre)
                .Select(g => new SolicitudPorUsuario
                {
                    Usuario = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            var solicitudesPorAsesor = await solicitudes
                .GroupBy(s => s.IdVendedorNavigation.Nombre)
                .Select(g => new SolicitudPorAsesor
                {
                    Asesor = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();


            var viewModel = new AnalyzeSolicitudViewModel
            {
                TotalSolicitudes = totalSolicitudes,
                SolicitudesPorMes = solicitudesPorMes,
                Asesores = asesores,
                Usuarios = usuarios,
                SolicitudesPorUsuario = solicitudesPorUsuario,
                SolicitudesPorAsesor = solicitudesPorAsesor
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AnalyzeSolicitud(string asesor = null, string usuario = null)
        {
            var userName = User.Identity.Name;
            var userRole = User.FindFirst("Rol")?.Value;

            IQueryable<Solicitud> solicitudes = _context.Solicituds
                .Where(s => s.Estado != "Anulado");

            if (userRole != "Administrador" && userRole != "Super Usuario" &&
                userRole != "Gerencia" && userRole != "Desarrollador" &&
                userRole != "Soporte TI")
            {
                solicitudes = solicitudes.Where(s => s.IdUsuarioNavigation.Username == userName);
            }

            if (!string.IsNullOrEmpty(asesor))
            {
                solicitudes = solicitudes.Where(s => s.IdVendedorNavigation.Nombre == asesor)
                    .OrderBy(s => s.IdVendedorNavigation.Nombre);
            }

            if (!string.IsNullOrEmpty(usuario))
            {
                solicitudes = solicitudes.Where(s => s.IdUsuarioNavigation.Nombre == usuario)
                    .OrderBy(s => s.IdUsuarioNavigation.Nombre);
            }

            var totalSolicitudes = await solicitudes.CountAsync();

            var solicitudesPorMes = await solicitudes
                .GroupBy(s => new { s.Fecha.Value.Year, s.Fecha.Value.Month })
                .Select(g => new SolicitudPorMes
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            // Calcular ventas por mes desde SolicitudDetalle
            var ventasPorMes = await _context.SolicitudDetalles
                .Where(d => d.IdSolicitud.HasValue && solicitudes.Select(s => s.IdSolicitud).Contains(d.IdSolicitud.Value))
                .GroupBy(d => new { d.IdSolicitudNavigation.Fecha.Value.Year, d.IdSolicitudNavigation.Fecha.Value.Month })
                .Select(g => new VentasPorMes
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalVentas = g.Sum(d => (decimal)d.PrecioCosto * (decimal)d.Cantidad)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            var solicitudesPorUsuario = await solicitudes
                .GroupBy(s => s.IdUsuarioNavigation.Nombre)
                .Select(g => new SolicitudPorUsuario
                {
                    Usuario = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            var solicitudesPorAsesor = await solicitudes
                .GroupBy(s => s.IdVendedorNavigation.Nombre)
                .Select(g => new SolicitudPorAsesor
                {
                    Asesor = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            var productosMasSolicitados = await _context.SolicitudDetalles
                .Where(d => d.IdSolicitud.HasValue && solicitudes.Select(s => s.IdSolicitud).Contains(d.IdSolicitud.Value))
                .GroupBy(d => new { d.IdProductoNavigation.Referencia, d.IdProductoNavigation.Descripcion })
                .Select(g => new ProductoMasSolicitado
                {
                    Referencia = g.Key.Referencia,
                    Descripcion = g.Key.Descripcion,
                    TotalCantidad = g.Sum(d => (decimal)d.Cantidad * (decimal)d.PrecioCosto)
                })
                .OrderByDescending(g => g.TotalCantidad)
                .Take(20)
                .ToListAsync();

            var comprasPorProveedor = await _context.SolicitudDetalles
                .Where(d => d.IdSolicitud.HasValue && solicitudes.Select(s => s.IdSolicitud).Contains(d.IdSolicitud.Value))
                .GroupBy(d => d.IdSolicitudNavigation.IdProveedorNavigation.Nombre)
                .Select(g => new ComprasPorProveedor
                {
                    Proveedor = g.Key,
                    TotalComprado = g.Sum(d => (decimal)d.Cantidad * (decimal)d.PrecioCosto)
                })
                .OrderByDescending(g => g.TotalComprado)
                .Take(10)
                .ToListAsync();

            var comprasPorCliente = await _context.SolicitudDetalles
                .Where(d => d.IdSolicitud.HasValue && solicitudes.Select(s => s.IdSolicitud).Contains(d.IdSolicitud.Value))
                .GroupBy(d => d.IdSolicitudNavigation.IdClienteNavigation.Nombre)
                .Select(g => new ComprasPorCliente
                {
                    Cliente = g.Key,
                    TotalComprado = g.Sum(d => (decimal)d.Cantidad * (decimal)d.PrecioVenta)
                })
                .OrderByDescending(g => g.TotalComprado)
                .Take(10)
                .ToListAsync();

            var viewModel = new AnalyzeSolicitudViewModel
            {
                TotalSolicitudes = totalSolicitudes,
                SolicitudesPorMes = solicitudesPorMes,
                VentasPorMes = ventasPorMes,
                Asesores = await solicitudes.Select(s => s.IdVendedorNavigation.Nombre).Distinct().ToListAsync(),
                Usuarios = await solicitudes.Select(s => s.IdUsuarioNavigation.Nombre).Distinct().ToListAsync(),
                SolicitudesPorUsuario = solicitudesPorUsuario,
                SolicitudesPorAsesor = solicitudesPorAsesor,
                ProductosMasSolicitados = productosMasSolicitados,
                ComprasPorProveedor = comprasPorProveedor,
                ComprasPorCliente = comprasPorCliente
            };

            return View(viewModel);

        }


        [HttpPost]
        public async Task<IActionResult> DownloadPDF(string asesor = null, string usuario = null, string chartImage = null)
        {
            var result = await AnalyzeSolicitud(asesor, usuario) as ViewResult;
            if (result != null)
            {
                var model = result.Model as AnalyzeSolicitudViewModel;
                if (model != null)
                {
                    ViewBag.ChartImage = chartImage;
                    return new ViewAsPdf("AnalyzeSolicitudPDF", model)
                    {
                        FileName = $"Analisis_Solicitudes_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf"
                    };
                }
            }
            // Handle the error case appropriately, e.g., return an error view or a message
            return RedirectToAction("AnalyzeSolicitud", new { asesor, usuario });
        }

        private bool SolicitudExists(int id)
        {
            return _context.Solicituds.Any(e => e.IdSolicitud == id);
        }

        private bool SolicitudDetalleExists(int id)
        {
            return _context.SolicitudDetalles.Any(e => e.IdSolicitudDetalle == id);
        }
    }
}
