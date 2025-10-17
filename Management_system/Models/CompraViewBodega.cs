using System;
using System.Collections.Generic;

namespace Management_system.Models;

public class CompraViewBodega
{
    public IEnumerable<Solicitud> Solicitudes { get; set; }
    public IEnumerable<Compra> Compras { get; set; }
    public IEnumerable<SolicitudDetalle> SolicitudDetalles { get; set; }
    public PaginationViewModelBodega Pagination { get; set; }
}

public class PaginationViewModelBodega
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}
