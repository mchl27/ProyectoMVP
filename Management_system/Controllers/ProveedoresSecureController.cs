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
    public class ProveedoresSecureController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public ProveedoresSecureController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: ProveedoresSecure
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
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

            var proveedor = _context.Proveedors
                .Include(p => p.IdEmpresaNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                proveedor = proveedor.Where(p => p.Nombre.Contains(searchString));
            }

            int totalItems = await proveedor.CountAsync();
            var paginatedItems = await proveedor.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ProveedorIndexViewModel
            {
                Proveedores = paginatedItems,
                Pagination = new PaginationViewModel1
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // GET: ProveedoresSecure/Details/5
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

            var proveedor = await _context.Proveedors
                .Include(p => p.IdEmpresaNavigation)
                .FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: ProveedoresSecure/Create
        public IActionResult Create()
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

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");

            return View();
        }

        // POST: ProveedoresSecure/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProveedor,Nit,Nombre,Email,Ciudad,Telefono,Descripcion,Direccion,IdEmpresa,Categoria")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");

            return View(proveedor);
        }

        // GET: ProveedoresSecure/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");

            return View(proveedor);
        }

        // POST: ProveedoresSecure/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProveedor,Nit,Nombre,Email,Ciudad,Telefono,Descripcion,Direccion,IdEmpresa,Categoria")] Proveedor proveedor)
        {
            if (id != proveedor.IdProveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.IdProveedor))
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

            return View(proveedor);
        }

        // GET: ProveedoresSecure/Delete/5
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

            var proveedor = await _context.Proveedors
                .FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: ProveedoresSecure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedors.Remove(proveedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedors.Any(e => e.IdProveedor == id);
        }
    }
}
