namespace Management_system.Models.Others.ViewModel.Cliente
{
    public class ClientesImportarViewModel
    {
        public List<ClienteResumen> ClientesAgregados { get; set; } = new();
        public List<ClienteResumen> ClientesDuplicados { get; set; } = new();
        public List<ClienteResumen> ClientesErroneos { get; set; } = new();
    }


    public class ClienteResumen
    {
        public int? Nit { get; set; }
        public string? Nombre { get; set; }
        public string? Motivo { get; set; }
    }

}
