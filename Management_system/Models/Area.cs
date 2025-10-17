using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Area
{
    public int IdArea { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<MTipoPrestamo> MTipoPrestamos { get; set; } = new List<MTipoPrestamo>();

    public virtual ICollection<MTipoRequerimiento> MTipoRequerimientos { get; set; } = new List<MTipoRequerimiento>();

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Vendedor> Vendedors { get; set; } = new List<Vendedor>();
}
