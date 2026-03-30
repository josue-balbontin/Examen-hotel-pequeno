using System;
using System.Collections.Generic;

namespace Backend.Modelos.Entidades;

public partial class Estado
{
    public int IdEstados { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
