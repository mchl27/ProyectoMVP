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
    public class VendedoresController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public VendedoresController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Vendedores
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 25)
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
            var vendedor = _context.Vendedors
                .Include(v => v.CargoNavigation)
                .Include(v => v.IdAreaNavigation)
                .AsQueryable();


            if (!string.IsNullOrEmpty(searchString))
            {
                vendedor = vendedor.Where(u => u.Identificacion.ToString().Contains(searchString) || u.Nombre.Contains(searchString));
            }

            int totalItems = await vendedor.CountAsync();
            var paginatedItems = await vendedor.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new VendedorIndexViewModel
            {
                Vendedores = paginatedItems,
                Pagination = new PaginationViewModelVendedor
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // GET: Vendedores/Details/5
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

            var vendedor = await _context.Vendedors
                .Include(v => v.CargoNavigation)
                .Include(v => v.IdAreaNavigation)
                .FirstOrDefaultAsync(m => m.IdVendedor == id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return View(vendedor);
        }

        // GET: Vendedores/Create
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
            ViewData["Cargo"] = new SelectList(_context.Cargos, "IdCargo", "Nombre");
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "Nombre");
            return View();
        }

        // POST: Vendedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVendedor,Identificacion,Nombre,IdArea,Cargo,Estado")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cargo"] = new SelectList(_context.Cargos, "IdCargo", "IdCargo", vendedor.Cargo);
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "IdArea", vendedor.IdArea);
            return View(vendedor);
        }

        // GET: Vendedores/Edit/5
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

            var vendedor = await _context.Vendedors.FindAsync(id);
            if (vendedor == null)
            {
                return NotFound();
            }
            ViewData["Cargo"] = new SelectList(_context.Cargos, "IdCargo", "Nombre", vendedor.Cargo);
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "Nombre", vendedor.IdArea);
            return View(vendedor);
        }

        // POST: Vendedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVendedor,Identificacion,Nombre,IdArea,Cargo,Estado")] Vendedor vendedor)
        {
            if (id != vendedor.IdVendedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendedorExists(vendedor.IdVendedor))
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
            ViewData["Cargo"] = new SelectList(_context.Cargos, "IdCargo", "IdCargo", vendedor.Cargo);
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "IdArea", vendedor.IdArea);
            return View(vendedor);
        }

        // GET: Vendedores/Delete/5
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

            var vendedor = await _context.Vendedors
                .Include(v => v.CargoNavigation)
                .Include(v => v.IdAreaNavigation)
                .FirstOrDefaultAsync(m => m.IdVendedor == id);
            if (vendedor == null)
            {
                return NotFound();
            }

            return View(vendedor);
        }

        // POST: Vendedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendedor = await _context.Vendedors.FindAsync(id);
            if (vendedor != null)
            {
                _context.Vendedors.Remove(vendedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendedorExists(int id)
        {
            return _context.Vendedors.Any(e => e.IdVendedor == id);
        }
    }
}
