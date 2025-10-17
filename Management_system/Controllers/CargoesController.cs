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
    public class CargoesController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public CargoesController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Cargoes
        public async Task<IActionResult> Index()
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
            return View(await _context.Cargos.ToListAsync());
        }

        // GET: Cargoes/Details/5
        public async Task<IActionResult> Details(int? id)
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

            var cargo = await _context.Cargos
                .FirstOrDefaultAsync(m => m.IdCargo == id);
            if (cargo == null)
            {
                return NotFound();
            }

            return View(cargo);
        }

        // GET: Cargoes/Create
        public IActionResult Create()
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
            return View();
        }

        // POST: Cargoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCargo,Nombre")] Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cargo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cargo);
        }

        // GET: Cargoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null)
            {
                return NotFound();
            }
            return View(cargo);
        }

        // POST: Cargoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCargo,Nombre")] Cargo cargo)
        {
            if (id != cargo.IdCargo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoExists(cargo.IdCargo))
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
            return View(cargo);
        }

        // GET: Cargoes/Delete/5
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

            var cargo = await _context.Cargos
                .FirstOrDefaultAsync(m => m.IdCargo == id);
            if (cargo == null)
            {
                return NotFound();
            }

            return View(cargo);
        }

        // POST: Cargoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo != null)
            {
                _context.Cargos.Remove(cargo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CargoExists(int id)
        {
            return _context.Cargos.Any(e => e.IdCargo == id);
        }
    }
}
