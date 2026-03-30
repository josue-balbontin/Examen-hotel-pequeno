using System;
using System.Collections.Generic;

namespace Backend.Modelos.Entidades;

public partial class Configuracione
{
    public int IdConfiguracion { get; set; }

    public string? NombreConfiguracion { get; set; }

    public string? ValorConfiguracion { get; set; }

    public string? TipoDato { get; set; }
}
