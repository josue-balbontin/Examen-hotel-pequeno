using Backend.Modelos.DTOs;
using Backend.Modelos.Entidades;
using Backend.Patrones;
using Backend.Repositorio.Configuracion;
using Backend.Repositorio.Reserva;
using System.Globalization;
using static Backend.Modelos.Enums.EstadosReservaEnum;


namespace Backend.Servicios;

public class ReservaServicio : IReservaServicio
{
    private readonly IReservaRepositorio _repositorio;
    private readonly IConfiguracionRepositorio _configuracionRepositorio;


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

        var reserva = dto.MapearAReserva((int)EstadoReservado);

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
        
        if (reserva.IdEstados != (int)EstadoReservado)
        {
            throw new InvalidOperationException("Solo se puede hacer Check-in a una reserva en estado 'Reservado'.");
        }


        
        reserva.FechaCheckin = DateTime.UtcNow;
        reserva.IdEstados = (int)EstadoOcupado; 
        
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
        reserva.IdEstados = (int)EstadoFinalizado;

        _repositorio.ActualizarReserva(reserva);
    }
    
    public IEnumerable<Habitacione> BuscarDisponibilidad(DateOnly ingreso, DateOnly salida)
    {
        if (salida <= ingreso)
        {
            throw new ArgumentException("La fecha de salida debe ser posterior a la de ingreso.");
        }

        return _repositorio.ObtenerHabitacionesDisponibles(ingreso, salida);
    }

    public void CancelarReserva(int idReserva)
    {
        var reserva = _repositorio.ObtenerPorId(idReserva);
        
        if (reserva == null)
        {
            throw new ArgumentException("Reserva no encontrada.");
        }
        
        if (reserva.IdEstados != (int)EstadoReservado)
        {
            throw new InvalidOperationException("Solo se pueden cancelar reservas que estén en estado 'Reservado'.");
        }
        

        reserva.IdEstados = (int)EstadoCancelado; 
        
        _repositorio.ActualizarReserva(reserva);
    }
    
}
