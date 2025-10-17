using System;
using System.Collections.Generic;

namespace Management_system.Models;

public partial class Auditorium
{
    public int IdAuditoria { get; set; }

    public DateTime? FechaHora { get; set; }

    public int? IdUsuario { get; set; }

    public string? Usuario { get; set; }

    public string? Area { get; set; }

    public string? Cargo { get; set; }

    public string? Accion { get; set; }

    public string? Documento { get; set; }
}
