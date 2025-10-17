using System;
using System.Collections.Generic;

namespace Management_system.Models;

public class CompraViewOC
{
    public IEnumerable<Solicitud> Solicitudes { get; set; }
    public IEnumerable<Compra> Compras { get; set; }
    public IEnumerable<SolicitudDetalle> SolicitudDetalles { get; set; }
    public PaginationViewModelOC Pagination { get; set; }
}

public class PaginationViewModelOC
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

