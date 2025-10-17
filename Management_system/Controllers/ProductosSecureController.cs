using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Management_system.Models;

namespace Management_system.Controllers
{
    public class ProductosSecureController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public ProductosSecureController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: ProductosSecure
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 30)
        {
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            {
                return RedirectToAction("Index", "Main");
            }

            var productos = _context.Productos
                .Include(p => p.IdEmpresaNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Descripcion.Contains(searchString) || p.Referencia.Contains(searchString));
            }

            int totalItems = await productos.CountAsync();
            var paginatedItems = await productos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ProductoIndexViewModel
            {
                Productos = paginatedItems,
                Pagination = new PaginationViewModelPagos
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // GET: ProductosSecure/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            {
                // Redirect the user to the main home page if they do not have the appropriate roles
                return RedirectToAction("Index", "Main");
            }
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdEmpresaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: ProductosSecure/Create
        public IActionResult Create()
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            {
                // Redirect the user to the main home page if they do not have the appropriate roles
                return RedirectToAction("Index", "Main");
            }

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");

            return View();
        }

        // POST: ProductosSecure/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,Empresa,Referencia,Descripcion,Unidad,Estado,Link,Proveedor,UltimoIngreso,Precio,IdEmpresa")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");

            return View(producto);
        }

        // GET: ProductosSecure/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            {
                // Redirect the user to the main home page if they do not have the appropriate roles
                return RedirectToAction("Index", "Main");
            }
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");

            return View(producto);
        }

        // POST: ProductosSecure/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,Empresa,Referencia,Descripcion,Unidad,Estado,Link,Proveedor,UltimoIngreso,Precio,IdEmpresa")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
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

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");

            return View(producto);
        }

        // GET: ProductosSecure/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Verify if the user has the appropriate roles
            var userRole = User.FindFirst("Rol")?.Value;
            if (userRole == null ||
                (userRole != "Administrador" && userRole != "Super Usuario" &&
                 userRole != "Gerencia" && userRole != "Desarrollador" &&
                 userRole != "Soporte TI"))
            {
                // Redirect the user to the main home page if they do not have the appropriate roles
                return RedirectToAction("Index", "Main");
            }
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: ProductosSecure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // RECEPTION

        // GET: ProductosSecure
        public async Task<IActionResult> ReceptionIndex(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            //var userRole = User.FindFirst("Rol")?.Value;
            //if (userRole == null ||
            //    (userRole != "Administrador" && userRole != "Super Usuario" &&
            //     userRole != "Gerencia" && userRole != "Desarrollador" &&
            //     userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            //{
            //    return RedirectToAction("Index", "Main");
            //}

            var productos = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Descripcion.Contains(searchString) || p.Referencia.Contains(searchString));
            }

            int totalItems = await productos.CountAsync();
            var paginatedItems = await productos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ProductoReceptionIndexViewModel
            {
                Productos = paginatedItems,
                Pagination = new PaginationReceptionViewModel
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // GET: ProductosSecure/Edit/5
        public async Task<IActionResult> ReceptionEdit(int? id)
        {
            // Verify if the user has the appropriate roles
            //var userRole = User.FindFirst("Rol")?.Value;
            //if (userRole == null ||
            //    (userRole != "Administrador" && userRole != "Super Usuario" &&
            //     userRole != "Gerencia" && userRole != "Desarrollador" &&
            //     userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            //{
            //    // Redirect the user to the main home page if they do not have the appropriate roles
            //    return RedirectToAction("Index", "Main");
            //}

            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: ProductosSecure/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceptionEdit(int id, [Bind("IdProducto,Empresa,Referencia,Descripcion,Unidad,Estado,Link,Proveedor,UltimoIngreso,Precio,IdEmpresa")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ReceptionIndex));
            }
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileEdit(IFormFile file, string referencia)
        {
            if (file != null && file.Length > 0 && !string.IsNullOrEmpty(referencia))
            {
                try
                {
                    // Ruta base
                    var baseDir = @"\\udc_des01\DB PL";
                    // Crear la ruta con la referencia
                    var referenciaDir = Path.Combine(baseDir, referencia);

                    // Verificar si la carpeta de referencia existe, si no, crearla
                    if (!Directory.Exists(referenciaDir))
                    {
                        Directory.CreateDirectory(referenciaDir);
                    }

                    // Ruta completa para guardar el archivo
                    var filePath = Path.Combine(referenciaDir, file.FileName);

                    // Verificar si el archivo ya existe y eliminarlo si es necesario
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // Guardar el archivo en la ruta especificada
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Json(new { fileName = file.FileName, filePath = $"\\\\udc_des01\\DB PL\\{referencia}\\{file.FileName}" });
                }
                catch (Exception ex)
                {
                    return Json(new { error = "Error uploading file", details = ex.Message });
                }
            }
            return BadRequest("No file uploaded or reference missing");
        }


        // RECEPTION

        // GET: ProductosSecure
        public async Task<IActionResult> SolicitudIndex(string searchString, int pageNumber = 1, int pageSize = 30)
        {
            //var userRole = User.FindFirst("Rol")?.Value;
            //if (userRole == null ||
            //    (userRole != "Administrador" && userRole != "Super Usuario" &&
            //     userRole != "Gerencia" && userRole != "Desarrollador" &&
            //     userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            //{
            //    return RedirectToAction("Index", "Main");
            //}

            var productos = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Descripcion.Contains(searchString) || p.Referencia.Contains(searchString));
            }

            int totalItems = await productos.CountAsync();
            var paginatedItems = await productos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ProductoSolicitudIndexViewModel
            {
                Productos = paginatedItems,
                Pagination = new PaginationSolicitudViewModel
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // GET: ProductosSecure/Edit/5
        public async Task<IActionResult> SolicitudEdit(int? id)
        {
            // Verify if the user has the appropriate roles
            //var userRole = User.FindFirst("Rol")?.Value;
            //if (userRole == null ||
            //    (userRole != "Administrador" && userRole != "Super Usuario" &&
            //     userRole != "Gerencia" && userRole != "Desarrollador" &&
            //     userRole != "Soporte TI" && userRole != "Editor de Informacion"))
            //{
            //    // Redirect the user to the main home page if they do not have the appropriate roles
            //    return RedirectToAction("Index", "Main");
            //}


            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: ProductosSecure/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitudEdit(int id, [Bind("IdProducto,Empresa,Referencia,Descripcion,Unidad,Estado,Link,Proveedor,UltimoIngreso,Precio,IdEmpresa")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(SolicitudIndex));
            }
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileEditS(IFormFile file, string referencia)
        {
            if (file != null && file.Length > 0 && !string.IsNullOrEmpty(referencia))
            {
                try
                {
                    // Ruta base
                    var baseDir = @"\\udc_des01\DB PL";
                    // Crear la ruta con la referencia
                    var referenciaDir = Path.Combine(baseDir, referencia);

                    // Verificar si la carpeta de referencia existe, si no, crearla
                    if (!Directory.Exists(referenciaDir))
                    {
                        Directory.CreateDirectory(referenciaDir);
                    }

                    // Ruta completa para guardar el archivo
                    var filePath = Path.Combine(referenciaDir, file.FileName);

                    // Verificar si el archivo ya existe y eliminarlo si es necesario
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // Guardar el archivo en la ruta especificada
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Json(new { fileName = file.FileName, filePath = $"\\\\udc_des01\\DB PL\\{referencia}\\{file.FileName}" });
                }
                catch (Exception ex)
                {
                    return Json(new { error = "Error uploading file", details = ex.Message });
                }
            }
            return BadRequest("No file uploaded or reference missing");
        }



        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }
    }
}
