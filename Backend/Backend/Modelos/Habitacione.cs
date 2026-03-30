using System;
using System.Collections.Generic;

namespace Backend.Modelos;

public partial class Habitacione
{
    public int IdHabitaciones { get; set; }

    public string? NumeroHabitacion { get; set; }

    public int? IdTipoHabitacion { get; set; }

    public virtual TipoHabitacione? IdTipoHabitacionNavigation { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
