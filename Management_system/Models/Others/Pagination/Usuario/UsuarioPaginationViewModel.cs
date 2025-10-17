using System;
using System.Collections.Generic;

namespace Management_system.Models;


// Solicitud
public class UsuarioIndexViewModel
{
    public IEnumerable<Usuario> Usuarios { get; set; }
    public PaginationViewModelUsuario Pagination { get; set; }
}

public class PaginationViewModelUsuario
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

