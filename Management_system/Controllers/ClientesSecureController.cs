using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Management_system.Models;
using ClosedXML.Excel;
using Management_system.Models.Others.ViewModel.Cliente;

namespace Management_system.Controllers
{
    public class ClientesSecureController : Controller
    {
        private readonly DbManagementSystemContext _context;

        public ClientesSecureController(DbManagementSystemContext context)
        {
            _context = context;
        }

        public IActionResult ImportarDatos()
        {
            return View();
        }

        // POST: Cientes Plantilla
        [HttpGet]
        public IActionResult DescargarPlantilla()
        {
            using (var workbook = new XLWorkbook())
            {
                var hoja = workbook.Worksheets.Add("Clientes");
                hoja.Cell(1, 1).Value = "Nit";
                hoja.Cell(1, 2).Value = "Nombre";
                hoja.Cell(1, 3).Value = "Email";
                hoja.Cell(1, 4).Value = "Ciudad";
                hoja.Cell(1, 5).Value = "Dirección";

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Plantilla_Clientes.xlsx");
                }
            }
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CargarClientes(IFormFile archivo, [FromServices] DbManagementSystemContext _context)
        {
            var resultado = new ClientesImportarViewModel();

            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo no válido.");

            using (var stream = new MemoryStream())
            {
                await archivo.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var hoja = workbook.Worksheet(1);
                    var filaInicio = 2;
                    var filaFin = hoja.LastRowUsed().RowNumber();

                    for (int fila = filaInicio; fila <= filaFin; fila++)
                    {
                        string nombre = hoja.Cell(fila, 2).GetValue<string>();
                        string email = hoja.Cell(fila, 3).GetValue<string>();
                        int? nit;

                        // Validar NIT
                        if (!int.TryParse(hoja.Cell(fila, 1).GetValue<string>(), out int nitParsed))
                        {
                            resultado.ClientesErroneos.Add(new ClienteResumen { Nombre = nombre, Motivo = "NIT inválido o vacío" });
                            continue;
                        }

                        nit = nitParsed;

                        // Validar email
                        if (!string.IsNullOrEmpty(email) && !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                        {
                            resultado.ClientesErroneos.Add(new ClienteResumen { Nit = nit, Nombre = nombre, Motivo = "Email inválido" });
                            continue;
                        }

                        // Validar duplicados
                        bool existe = await _context.Clientes.AnyAsync(c => c.Nit == nit);
                        if (existe)
                        {
                            resultado.ClientesDuplicados.Add(new ClienteResumen { Nit = nit, Nombre = nombre, Motivo = "Duplicado" });
                            continue;
                        }

                        // Cliente válido
                        var cliente = new Cliente
                        {
                            Nit = nit,
                            Nombre = nombre,
                            Email = email,
                            Ciudad = hoja.Cell(fila, 4).GetValue<string>(),
                            Direccion = hoja.Cell(fila, 5).GetValue<string>()
                        };

                        _context.Clientes.Add(cliente);
                        resultado.ClientesAgregados.Add(new ClienteResumen { Nit = nit, Nombre = nombre });
                    }

                    await _context.SaveChangesAsync();
                }
            }

            TempData["ResumenCarga"] = System.Text.Json.JsonSerializer.Serialize(resultado); // Para exportar si se desea
            return View("ResultadoCargaClientes", resultado);
        }

        public IActionResult ResultadoCargaClientes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExportarResumenExcel()
        {
            var resumenJson = TempData["ResumenCarga"] as string;
            if (resumenJson == null)
                return RedirectToAction("Index");

            var resumen = System.Text.Json.JsonSerializer.Deserialize<ClientesImportarViewModel>(resumenJson);

            using var workbook = new XLWorkbook();
            var hoja = workbook.AddWorksheet("Resumen");

            hoja.Cell(1, 1).Value = "Tipo";
            hoja.Cell(1, 2).Value = "NIT";
            hoja.Cell(1, 3).Value = "Nombre";
            hoja.Cell(1, 4).Value = "Motivo";

            int row = 2;

            void AgregarFila(string tipo, ClienteResumen c)
            {
                hoja.Cell(row, 1).Value = tipo;
                hoja.Cell(row, 2).Value = c.Nit?.ToString() ?? "-";
                hoja.Cell(row, 3).Value = c.Nombre;
                hoja.Cell(row, 4).Value = c.Motivo;
                row++;
            }

            resumen.ClientesAgregados.ForEach(c => AgregarFila("Agregado", c));
            resumen.ClientesDuplicados.ForEach(c => AgregarFila("Duplicado", c));
            resumen.ClientesErroneos.ForEach(c => AgregarFila("Error", c));

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Resumen_Carga_Clientes.xlsx");
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
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

            var clientes = _context.Clientes
                .Include(c => c.IdEmpresaNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                clientes = clientes.Where(c => c.Nombre.Contains(searchString));
            }

            int totalItems = await clientes.CountAsync();
            var paginatedItems = await clientes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new ClienteIndexViewModel
            {
                Clientes = paginatedItems,
                Pagination = new PaginationViewModel2
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(viewModel);
        }

        // GET: Clientes/Details/5
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

            var cliente = await _context.Clientes
                .Include(c => c.IdEmpresaNavigation)
                .FirstOrDefaultAsync(m => m.IdCliente == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
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

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente,Nit,Nombre,Email,Ciudad,Telefono,Direccion,IdEmpresa,Categoria")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");

            return View(cliente);
        }

        // GET: Clientes/Edit/5
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

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente,Nit,Nombre,Email,Ciudad,Telefono,Direccion,IdEmpresa,Categoria")] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.IdCliente))
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

            return View(cliente);
        }

        // GET: Clientes/Delete/5
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

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id);
        }
    }
}
