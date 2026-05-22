using System.ComponentModel.DataAnnotations;
using Backend.Modelos.Entidades;

namespace Backend.Modelos.DTOs;

public class CrearReservaDto
{
    [Required]
    public required List<int> IdsUsuarios { get; set; } = new();

    [Required]
    public required int IdHabitacion { get; set; }

    [Required]
    public required DateOnly FechaIngreso { get; set; }

    [Required]
    public required DateOnly FechaSalida { get; set; }

    public Reserva MapearAReserva(int idEstadoPorDefecto)
    {
        return new Reserva
        {
            IdHabitaciones = IdHabitacion,
            FechaIngreso = FechaIngreso,
            FechaSalida = FechaSalida,
            IdEstados = idEstadoPorDefecto
        };
    }
}