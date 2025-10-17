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
    public class ConductoresSecureController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public ConductoresSecureController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: ConductoresSecure
        public async Task<IActionResult> Index()
        {
            return View(await _context.Conductors.ToListAsync());
        }

        // GET: ConductoresSecure/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conductor = await _context.Conductors
                .FirstOrDefaultAsync(m => m.IdConductor == id);
            if (conductor == null)
            {
                return NotFound();
            }

            return View(conductor);
        }

        // GET: ConductoresSecure/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConductoresSecure/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConductor,Identificacion,Nombre,PlacaVehiculo,Telefono")] Conductor conductor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conductor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conductor);
        }

        // GET: ConductoresSecure/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conductor = await _context.Conductors.FindAsync(id);
            if (conductor == null)
            {
                return NotFound();
            }
            return View(conductor);
        }

        // POST: ConductoresSecure/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdConductor,Identificacion,Nombre,PlacaVehiculo,Telefono")] Conductor conductor)
        {
            if (id != conductor.IdConductor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conductor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConductorExists(conductor.IdConductor))
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
            return View(conductor);
        }

        // GET: ConductoresSecure/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conductor = await _context.Conductors
                .FirstOrDefaultAsync(m => m.IdConductor == id);
            if (conductor == null)
            {
                return NotFound();
            }

            return View(conductor);
        }

        // POST: ConductoresSecure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conductor = await _context.Conductors.FindAsync(id);
            if (conductor != null)
            {
                _context.Conductors.Remove(conductor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConductorExists(int id)
        {
            return _context.Conductors.Any(e => e.IdConductor == id);
        }
    }
}
