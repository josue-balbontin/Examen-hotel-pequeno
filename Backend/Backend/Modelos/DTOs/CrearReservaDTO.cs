using System.ComponentModel.DataAnnotations;
using Backend.Modelos.Entidades;

namespace Backend.Modelos.DTOs;

public class CrearReservaDTO
{
    [Required]
    public List<int> IdsUsuarios { get; set; } = new();

    [Required]
    public int IdHabitacion { get; set; }

    [Required]
    public DateOnly FechaIngreso { get; set; }

    [Required]
    public DateOnly FechaSalida { get; set; }

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