using System;
using System.Collections.Generic;

namespace Backend.Modelos;

public partial class Reserva
{
    public int IdReservas { get; set; }

    public int? IdHabitaciones { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public DateOnly? FechaSalida { get; set; }

    public int? CantidadPersonas { get; set; }

    public int? IdEstados { get; set; }

    public DateTime? FechaCheckin { get; set; }

    public DateTime? FechaCheckout { get; set; }

    public decimal? CargoCheckout { get; set; }

    public virtual Estado? IdEstadosNavigation { get; set; }

    public virtual Habitacione? IdHabitacionesNavigation { get; set; }

    public virtual ICollection<UsuariosReserva> UsuariosReservas { get; set; } = new List<UsuariosReserva>();
}
