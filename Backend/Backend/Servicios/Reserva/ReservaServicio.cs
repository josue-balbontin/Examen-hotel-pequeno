using Backend.Modelos.DTOs;
using Backend.Modelos.Entidades;
using Backend.Repositorio.Reserva;

namespace Backend.Servicios;

public class ReservaServicio : IReservaServicio
{
    private readonly IReservaRepositorio _repositorio;
    private const int EstadoReservado = 3;
    private const int EstadoCancelado = 4; 
    private const int EstadoOcupado = 2;
    private const int EstadoDisponible = 1; 

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

        var reserva = dto.MapearAReserva(EstadoReservado);

        _repositorio.Crear(reserva, dto.IdsUsuarios);
    }

    public IEnumerable<Reserva> ObtenerReservas()
    {
        return _repositorio.ObtenerTodas();
    }

    public void RegistrarCheckIn(int idReserva)
    {
        var reserva = _repositorio.ObtenerPorId(idReserva);
        
        if (reserva == null)
        {
            throw new ArgumentException("Reserva no encontrada.");
        }
        
        if (reserva.FechaCheckin != null)
        {
            throw new InvalidOperationException("El check-in ya fue realizado previamente.");
        }

        if (reserva.IdEstados == EstadoCancelado || reserva.IdEstados ==  EstadoOcupado)
        {
            throw new InvalidOperationException("No se puede hacer Check-in en reservas Canceladas o Ocupadas.");
        }
        
        reserva.FechaCheckin = DateTime.UtcNow;
        reserva.IdEstados = EstadoOcupado; 
        
        _repositorio.ActualizarReserva(reserva); 
    }
}
