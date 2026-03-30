using System;
using System.Collections.Generic;

namespace Backend.Modelos.Entidades;

public partial class Usuario
{
    public int IdUsuarios { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string DocumentoIdentidad { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public int? Edad { get; set; }

    public virtual ICollection<UsuariosReserva> UsuariosReservas { get; set; } = new List<UsuariosReserva>();
}
