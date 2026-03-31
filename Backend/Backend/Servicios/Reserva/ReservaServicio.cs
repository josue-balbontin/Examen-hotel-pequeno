using Backend.Modelos.DTOs;
using Backend.Modelos.Entidades;
using Backend.Patrones;
using Backend.Repositorio.Configuracion;
using Backend.Repositorio.Reserva;
using System.Globalization;

namespace Backend.Servicios;

public class ReservaServicio : IReservaServicio
{
    private readonly IReservaRepositorio _repositorio;
    private readonly IConfiguracionRepositorio _configuracionRepositorio;
    private const int EstadoReservado = 3;
    private const int EstadoCancelado = 4; 
    private const int EstadoOcupado = 2;
    private const int EstadoDisponible = 1; 

    public ReservaServicio(IReservaRepositorio repositorio, IConfiguracionRepositorio configuracionRepositorio)
    {
        _repositorio = repositorio;
        _configuracionRepositorio = configuracionRepositorio;
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

    public void RegistrarCheckOut(int idReserva)
    {
        var reserva = _repositorio.ObtenerPorId(idReserva);

        if (reserva == null)
        {
            throw new ArgumentException("Reserva no encontrada.");
        }

        if (reserva.FechaCheckin == null)
        {
            throw new InvalidOperationException("No se puede registrar check-out sin check-in previo.");
        }

        if (reserva.FechaCheckout != null)
        {
            throw new InvalidOperationException("El check-out ya fue registrado previamente.");
        }

        if (reserva.FechaSalida == null)
        {
            throw new InvalidOperationException("La reserva no tiene fecha de salida definida.");
        }

        var ahora = DateTime.UtcNow;
        reserva.FechaCheckout = ahora;

        var horaLimiteStr = _configuracionRepositorio.ObtenerValor("hora_limite_checkout", "12:00");
        var porcentajeStr = _configuracionRepositorio.ObtenerValor("porcentaje_late_checkout", "0.50");

        var horaLimite = TimeSpan.Parse(horaLimiteStr, CultureInfo.InvariantCulture);
        var porcentajeRecargo = decimal.Parse(porcentajeStr, CultureInfo.InvariantCulture);

        var fechaSalida = reserva.FechaSalida.Value.ToDateTime(TimeOnly.MinValue);
        var limiteCheckout = fechaSalida.Add(horaLimite);

        var cargo = 0m;

        if (ahora > limiteCheckout)
        {
            if (reserva.IdHabitacionesNavigation?.IdTipoHabitacion == null)
            {
                throw new InvalidOperationException("No se pudo determinar el tipo de habitación para calcular el recargo.");
            }

            var cache = TipoHabitacionCache.ObtenerInstancia();
            var detalle = cache.ObtenerDetalle(reserva.IdHabitacionesNavigation.IdTipoHabitacion.Value);
            var precio = detalle.PrecioReferencia ?? throw new InvalidOperationException("El tipo de habitación no tiene un precio de referencia configurado.");

            cargo = precio * porcentajeRecargo;
        }

        reserva.CargoCheckout = cargo;
        reserva.IdEstados = EstadoDisponible;

        _repositorio.ActualizarReserva(reserva);
    }
}
