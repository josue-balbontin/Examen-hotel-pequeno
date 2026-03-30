using System;
using System.Collections.Generic;

namespace Backend.Modelos.Entidades;

public partial class TipoHabitacione
{
    public int IdTipoHabitaciones { get; set; }

    public string? Nombre { get; set; }

    public int? Capacidad { get; set; }

    public decimal? PrecioReferencia { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Habitacione> Habitaciones { get; set; } = new List<Habitacione>();
}
