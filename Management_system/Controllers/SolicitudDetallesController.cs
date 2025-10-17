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
    public class SolicitudDetallesController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public SolicitudDetallesController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: SolicitudDetalles
        public async Task<IActionResult> Index()
        {
            var dbManagementSystemContext = _context.SolicitudDetalles.Include(s => s.IdProductoNavigation).Include(s => s.IdSolicitudNavigation);
            return View(await dbManagementSystemContext.ToListAsync());
        }

        // GET: SolicitudDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: SolicitudDetalles/Create
        public IActionResult Create()
        {
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto");
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud");
            return View();
        }

        // POST: SolicitudDetalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSolicitudDetalle,IdSolicitud,IdProducto,Observaciones,Cantidad,PrecioCosto,PrecioVenta,Rentabilidad,Negociacion,ObservacionCompras")] SolicitudDetalle solicitudDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solicitudDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", solicitudDetalle.IdProducto);
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", solicitudDetalle.IdSolicitud);
            return View(solicitudDetalle);
        }

        // GET: SolicitudDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", solicitudDetalle.IdProducto);
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", solicitudDetalle.IdSolicitud);
            return View(solicitudDetalle);
        }

        // POST: SolicitudDetalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSolicitudDetalle,IdSolicitud,IdProducto,Observaciones,Cantidad,PrecioCosto,PrecioVenta,Rentabilidad,Negociacion,ObservacionCompras")] SolicitudDetalle solicitudDetalle)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", solicitudDetalle.IdProducto);
            ViewData["IdSolicitud"] = new SelectList(_context.Solicituds, "IdSolicitud", "IdSolicitud", solicitudDetalle.IdSolicitud);
            return View(solicitudDetalle);
        }

        // GET: SolicitudDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: SolicitudDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitudDetalle = await _context.SolicitudDetalles.FindAsync(id);
            if (solicitudDetalle != null)
            {
                _context.SolicitudDetalles.Remove(solicitudDetalle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitudDetalleExists(int id)
        {
            return _context.SolicitudDetalles.Any(e => e.IdSolicitudDetalle == id);
        }
    }
}
