using Management_system.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Management_system.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly DbManagementSystemContext _context;

        public HomeController(DbManagementSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        public IActionResult Phases()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nombreUsuario, string contrasena)
        {
            var usuario = _context.Usuarios
                .Include(u => u.IdAreaNavigation)
                .Include(u => u.IdRolNavigation)
                .Include(u => u.IdEmpresaNavigation) 
                .FirstOrDefault(u => u.Username == nombreUsuario && u.Contraseña == contrasena);

            if (usuario != null)
            {

                if (usuario.Estado == "Inactivo")
                {
                    ModelState.AddModelError("", "Usuario inhabilitado.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Username),
                    new Claim("Area", usuario.IdAreaNavigation.Nombre),
                    new Claim("Rol", usuario.IdRolNavigation.Nombre),
                    new Claim("Empresa", usuario.IdEmpresaNavigation.Nombre)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Main");
            }
            else
            {
                // Nombre de usuario o contraseña incorrectos.
                ModelState.AddModelError("", "Nombre de usuario o contraseña incorrectos.");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
