using System;
using System.Collections.Generic;

namespace Management_system.Models;


// Novedad
public class NovedadIndexViewModel
{
    public IEnumerable<Novedad> Novedades { get; set; }
    public PaginationViewModelNovedad Pagination { get; set; }
}

public class PaginationViewModelNovedad
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Tracking
public class NovedadTrackingViewModel
{
    public IEnumerable<Novedad> Novedades { get; set; }
    public PaginationViewModelNovedadTracking Pagination { get; set; }
}

public class PaginationViewModelNovedadTracking
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Override
public class NovedadOverrideViewModel
{
    public IEnumerable<Novedad> Novedades { get; set; }
    public PaginationViewModelNovedadOverride Pagination { get; set; }
}

public class PaginationViewModelNovedadOverride
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Consulte
public class NovedadConsulteViewModel
{
    public IEnumerable<Novedad> Novedades { get; set; }
    public PaginationViewModelNovedadConsulte Pagination { get; set; }
}

public class PaginationViewModelNovedadConsulte
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}




// NOVEDAD COMERCIAL


// Novedad Index
public class NovedadComercialIndexViewModel
{
    public IEnumerable<NovedadComercial> NovedadesC { get; set; }
    public PaginationViewModelNovedadComercial Pagination { get; set; }
}

public class PaginationViewModelNovedadComercial
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Tracking
public class NovedadComercialTrackingViewModel
{
    public IEnumerable<NovedadComercial> NovedadesC { get; set; }
    public PaginationViewModelNovedadComercialTracking Pagination { get; set; }
}

public class PaginationViewModelNovedadComercialTracking
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Override
public class NovedadComercialOverrideViewModel
{
    public IEnumerable<NovedadComercial> NovedadesC { get; set; }
    public PaginationViewModelNovedadComercialOverride Pagination { get; set; }
}

public class PaginationViewModelNovedadComercialOverride
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Consulte
public class NovedadComercialConsulteViewModel
{
    public IEnumerable<NovedadComercial> NovedadesC { get; set; }
    public PaginationViewModelNovedadComercialConsulte Pagination { get; set; }
}

public class PaginationViewModelNovedadComercialConsulte
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}




// NOVEDAD COMPRAS


// Review Compras
public class ComprasReviewNCViewModel
{
    public IEnumerable<Compra> ComprasNC { get; set; }
    public PaginationViewModelNovedadCompraNC Pagination { get; set; }
}

public class PaginationViewModelNovedadCompraNC
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Index
public class NovedadCompraIndexViewModel
{
    public IEnumerable<NovedadCompra> NovedadesCM { get; set; }
    public PaginationViewModelNovedadCompra Pagination { get; set; }
}

public class PaginationViewModelNovedadCompra
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Tracking
public class NovedadCompraTrackingViewModel
{
    public IEnumerable<NovedadCompra> NovedadesCM { get; set; }
    public PaginationViewModelNovedadCompraTracking Pagination { get; set; }
}

public class PaginationViewModelNovedadCompraTracking
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Override
public class NovedadCompraOverrideViewModel
{
    public IEnumerable<NovedadCompra> NovedadesCM { get; set; }
    public PaginationViewModelNovedadCompraOverride Pagination { get; set; }
}

public class PaginationViewModelNovedadCompraOverride
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Novedad Consulte
public class NovedadCompraConsulteViewModel
{
    public IEnumerable<NovedadCompra> NovedadesCM { get; set; }
    public PaginationViewModelNovedadCompraConsulte Pagination { get; set; }
}

public class PaginationViewModelNovedadCompraConsulte
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}



// RUTA

// Review Index
// Index
public class RutaReviewIndexViewModel
{
    public IEnumerable<Rutum> Rutas { get; set; }
    public PaginationViewModelRutaReviewIndex Pagination { get; set; }
}

public class PaginationViewModelRutaReviewIndex
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}


// Index
public class RutaIndexViewModel
{
    public IEnumerable<Rutum> Rutas { get; set; }
    public PaginationViewModelRutaIndex Pagination { get; set; }
}

public class PaginationViewModelRutaIndex
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

// CONSULTE
public class RutaConsulteViewModel
{
    public IEnumerable<Rutum> Rutas { get; set; }
    public PaginationViewModelRutaConsulte Pagination { get; set; }
}

public class PaginationViewModelRutaConsulte
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}
