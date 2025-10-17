using System;
using System.Collections.Generic;

namespace Management_system.Models
{
    public class SearchViewModel
    {
        public DateOnly FechaSalida { get; set; }
        public int? IdConductor { get; set; }
        public List<Conductor> Conductores { get; set; }
    }

    public class SearchCViewModel
    {
        public DateTime Fecha { get; set; }
        public int? IdUsuario { get; set; }
    }
}