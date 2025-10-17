namespace Management_system.Models
{
    public class VendedorPaginationViewModel
    {
    }


    // Vendedores
    public class VendedorIndexViewModel
    {
        public IEnumerable<Vendedor> Vendedores { get; set; }
        public PaginationViewModelVendedor Pagination { get; set; }
    }

    public class PaginationViewModelVendedor
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }

}
