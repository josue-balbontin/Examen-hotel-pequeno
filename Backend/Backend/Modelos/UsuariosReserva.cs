using System;
using System.Collections.Generic;

namespace Backend.Modelos;

public partial class UsuariosReserva
{
    public int IdUsuariosReservas { get; set; }

    public int? IdUsuarios { get; set; }

    public int? IdReservas { get; set; }

    public virtual Reserva? IdReservasNavigation { get; set; }

    public virtual Usuario? IdUsuariosNavigation { get; set; }
}
