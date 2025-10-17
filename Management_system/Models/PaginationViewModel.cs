using System;
using System.Collections.Generic;

namespace Management_system.Models;


// Productos
public class ProductoIndexViewModel
{
    public IEnumerable<Producto> Productos { get; set; }
    public PaginationViewModelPagos Pagination { get; set; }
}

public class PaginationViewModelPagos
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

public class ProductoReceptionIndexViewModel
{
    public IEnumerable<Producto> Productos { get; set; }
    public PaginationReceptionViewModel Pagination { get; set; }
}

public class PaginationReceptionViewModel
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


public class ProductoSolicitudIndexViewModel
{
    public IEnumerable<Producto> Productos { get; set; }
    public PaginationSolicitudViewModel Pagination { get; set; }
}

public class PaginationSolicitudViewModel
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}




public class ProveedorIndexViewModel
{
    public IEnumerable<Proveedor> Proveedores { get; set; }
    public PaginationViewModel1 Pagination { get; set; }
}

public class PaginationViewModel1
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


public class ClienteIndexViewModel
{
    public IEnumerable<Cliente> Clientes { get; set; }
    public PaginationViewModel2 Pagination { get; set; }
}

public class PaginationViewModel2
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}



// Solicitudes
public class SolicitudesIndexViewModel
{
    public IEnumerable<Solicitud> Solicitudes { get; set; }
    public PaginationViewModelSolicitudes Pagination { get; set; }
}

public class PaginationViewModelSolicitudes
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Solicitudes seguimiento
public class SeguimientoSolicitudesViewModel
{
    public IEnumerable<SolicitudDetalle> Solicitudes { get; set; }
    public PaginationViewModelSolicitudesSegumiento Pagination { get; set; }
}

public class PaginationViewModelSolicitudesSegumiento
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Solicitudes Review Compras
public class ReviewSolicitudesViewModel
{
    public IEnumerable<Solicitud> Solicitudes { get; set; }
    public PaginationViewModelSolicitudesReview Pagination { get; set; }
    public string SortOrder { get; set; }
    public string SortDirection { get; set; }
}

public class PaginationViewModelSolicitudesReview
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Solicitudes Consulte Review
public class ConsulteReviewSolicitudesViewModel
{
    public IEnumerable<Solicitud> Solicitudes { get; set; }
    public PaginationlSolicitudesConsulteReview Pagination { get; set; }
}

public class PaginationlSolicitudesConsulteReview
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Solicitudes Consulte Details
public class ConsulteDetailsSolicitudesViewModel
{
    public IEnumerable<SolicitudDetalle> Solicitudes { get; set; }
    public PaginationlSolicitudesConsulteDetails Pagination { get; set; }
}

public class PaginationlSolicitudesConsulteDetails
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Compras
public class ComprasIndexViewModel
{
    public IEnumerable<Compra> Compras { get; set; }
    public PaginationViewModelCompras Pagination { get; set; }
}

public class PaginationViewModelCompras
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

public class CSeguimientoSolicitudesViewModel
{
    public IEnumerable<Compra> Compras { get; set; }
    public CPaginationViewModelSolicitudesSegumiento Pagination { get; set; }
}

public class CPaginationViewModelSolicitudesSegumiento
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

// Compras Seguimiento Recepcion
public class ComprasReviewViewModel
{
    public IEnumerable<Compra> Compras { get; set; }
    public PaginationViewModelComprasReview Pagination { get; set; }
}

public class PaginationViewModelComprasReview
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

// Compras Seguimiento Recepcion
public class ComprasReviewPViewModel
{
    public IEnumerable<Compra> Compras { get; set; }
    public PaginationViewModelComprasReviewP Pagination { get; set; }
}

public class PaginationViewModelComprasReviewP
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

// Compras Consulte Review
public class ConsulteReviewComprasViewModel
{
    public IEnumerable<Compra> Compras { get; set; }
    public PaginationComprasConsulteReview Pagination { get; set; }
}

public class PaginationComprasConsulteReview
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}



// Pagos
public class PagosViewModel
{
    public IEnumerable<Pago> Pagos { get; set; }
    public PaginationViewModelPago Pagination { get; set; }
}

public class PaginationViewModelPago
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

public class PagosRCViewModel
{
    public IEnumerable<Pago> Pagos { get; set; }
    public PaginationViewModelPagoRC Pagination { get; set; }
}

public class PaginationViewModelPagoRC
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


public class PagosCViewModel
{
    public IEnumerable<Pago> Pagos { get; set; }
    public PaginationViewModelPagoC Pagination { get; set; }
}

public class PaginationViewModelPagoC
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


public class PagosRAViewModel
{
    public IEnumerable<Pago> Pagos { get; set; }
    public PaginationViewModelPagoRA Pagination { get; set; }
}

public class PaginationViewModelPagoRA
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


public class PagosAViewModel
{
    public IEnumerable<Pago> Pagos { get; set; }
    public PaginationViewModelPagoA Pagination { get; set; }
}

public class PaginationViewModelPagoA
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}




// Recepcion
public class RecepcionViewModel
{
    public IEnumerable<RecepcionMercancium> Recepciones { get; set; }
    public PaginationViewModelRecepcion Pagination { get; set; }
}

public class PaginationViewModelRecepcion
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


public class RecepcionConsulteRViewModel
{
    public IEnumerable<RecepcionMercancium> Recepciones { get; set; }
    public PaginationViewModelRecepcionColsulteR Pagination { get; set; }
}

public class PaginationViewModelRecepcionColsulteR
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}