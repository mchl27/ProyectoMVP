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
    public class UserSecureController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public UserSecureController(DbManagementSystemContext context)
        {
            _context = context;
        }

        // GET: UserSecure
        //public async Task<IActionResult> Index()
        //{
        //    // Verify if the user has the appropriate roles
        //    var userRole = User.FindFirst("Rol")?.Value;
        //    if (userRole == null ||
        //        (userRole != "Administrador" && userRole != "Super Usuario" &&
        //         userRole != "Gerencia" && userRole != "Desarrollador" &&
        //         userRole != "Soporte TI"))
        //    {
        //        // Redirect the user to the main home page if they do not have the appropriate roles
        //        return RedirectToAction("Index", "Main");
        //    }
        //    var dbManagementSystemContext = _context.Usuarios.Include(u => u.IdAreaNavigation).Include(u => u.IdEmpresaNavigation).Include(u => u.IdRolNavigation);
        //    return View(await dbManagementSystemContext.ToListAsync());
        //}



        // GET: Clientes
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

            var usuario = _context.Usuarios
                .Include(u => u.IdAreaNavigation)
                .Include(u => u.IdEmpresaNavigation)
                .Include(u => u.IdRolNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                usuario = usuario.Where(u => u.Identificacion.ToString().Contains(searchString) || u.Nombre.Contains(searchString));
            }

            int totalItems = await usuario.CountAsync();
            var paginatedItems = await usuario.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new UsuarioIndexViewModel
            {
                Usuarios = paginatedItems,
                Pagination = new PaginationViewModelUsuario
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }



        // GET: UserSecure/Details/5
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

            var usuario = await _context.Usuarios
                .Include(u => u.IdAreaNavigation)
                .Include(u => u.IdEmpresaNavigation)
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: UserSecure/Create
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

            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "Nombre");
            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");
            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "Nombre");
            return View();
        }

        // POST: UserSecure/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Creacion,Identificacion,Nombre,Email,Username,Contraseña,Estado,IdRol,IdEmpresa,IdArea")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "IdArea", usuario.IdArea);
            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "IdEmpresa", usuario.IdEmpresa);
            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: UserSecure/Edit/5
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

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "Nombre", usuario.IdArea);
            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre", usuario.IdEmpresa);
            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "Nombre", usuario.IdRol);
            return View(usuario);
        }

        // POST: UserSecure/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Creacion,Identificacion,Nombre,Email,Username,Contraseña,Estado,IdRol,IdEmpresa,IdArea")] Usuario usuario)
        {

            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
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
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdArea", "IdArea", usuario.IdArea);
            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "IdEmpresa", usuario.IdEmpresa);
            ViewData["IdRol"] = new SelectList(_context.Rols, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: UserSecure/Delete/5
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

            var usuario = await _context.Usuarios
                .Include(u => u.IdAreaNavigation)
                .Include(u => u.IdEmpresaNavigation)
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: UserSecure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
