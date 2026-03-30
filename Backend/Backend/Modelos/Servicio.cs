using System;
using System.Collections.Generic;

namespace Backend.Modelos;

public partial class Servicio
{
    public int IdServicios { get; set; }

    public string? NombreServicio { get; set; }

    public string? Telefono { get; set; }

    public string? Encargado { get; set; }
}
