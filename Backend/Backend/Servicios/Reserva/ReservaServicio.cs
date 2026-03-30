using Backend.Modelos.DTOs;
using Backend.Modelos.Entidades;
using Backend.Repositorio.Reserva;

namespace Backend.Servicios;

public class ReservaServicio : IReservaServicio
{
    private readonly IReservaRepositorio _repositorio;
    private const int EstadoReservadaPorDefecto = 3;

    public ReservaServicio(IReservaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public void CrearReserva(CrearReservaDTO dto)
    {
        if (dto.FechaSalida <= dto.FechaIngreso)
        {
            throw new ArgumentException("La fecha de salida debe ser posterior a la fecha de ingreso.");
        }

        var capacidad = _repositorio.ObtenerCapacidadHabitacion(dto.IdHabitacion);

        if (dto.IdsUsuarios.Count > capacidad)
        {
            throw new ArgumentException("La cantidad de huéspedes supera la capacidad de la habitación seleccionada.");
        }

        if (_repositorio.ExisteSolapamiento(dto.IdHabitacion, dto.FechaIngreso, dto.FechaSalida))
        {
            throw new InvalidOperationException("La habitación ya tiene una reserva en el rango de fechas seleccionado.");
        }

        var reserva = dto.MapearAReserva(EstadoReservadaPorDefecto);

        _repositorio.Crear(reserva, dto.IdsUsuarios);
    }

    public IEnumerable<Reserva> ObtenerReservas()
    {
        return _repositorio.ObtenerTodas();
    }
}
