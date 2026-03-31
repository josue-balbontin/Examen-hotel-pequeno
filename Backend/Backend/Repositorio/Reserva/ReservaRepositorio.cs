using Backend.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;
using static Backend.Modelos.Enums.EstadosReservaEnum;

namespace Backend.Repositorio.Reserva;

public class ReservaRepositorio : IReservaRepositorio
{
    private readonly HotelDbContext _contexto;

    public ReservaRepositorio(HotelDbContext contexto)
    {
        _contexto = contexto;
    }

    public int ObtenerCapacidadHabitacion(int idHabitacion)
    {
        var habitacion = _contexto.Habitaciones
            .Include(h => h.IdTipoHabitacionNavigation)
            .FirstOrDefault(h => h.IdHabitaciones == idHabitacion);

        if (habitacion == null)
        {
            throw new ArgumentException("La habitación indicada no existe.");
        }

        return habitacion.IdTipoHabitacionNavigation?.Capacidad ?? 0;
    }

    public bool ExisteSolapamiento(int idHabitacion, DateOnly ingreso, DateOnly salida)
    {
        return _contexto.Reservas.Any(r =>
            r.IdHabitaciones == idHabitacion &&
            r.IdEstados != (int)EstadoCancelado && 
            r.IdEstados != (int)EstadoFinalizado && 
            r.FechaIngreso != null && r.FechaSalida != null &&
            ingreso < r.FechaSalida && salida > r.FechaIngreso);
        
    }

    public void Crear(Modelos.Entidades.Reserva reserva, List<int> idsUsuarios)
    {
        _contexto.Reservas.Add(reserva);

        foreach (var idUsuario in idsUsuarios)
        {
            _contexto.UsuariosReservas.Add(new UsuariosReserva
            {
                IdUsuarios = idUsuario,
                IdReservasNavigation = reserva
            });
        }

        _contexto.SaveChanges();
    }
    
    public List<Modelos.Entidades.Reserva> ObtenerTodas()
    {
        return _contexto.Reservas.AsNoTracking().ToList();
    }

    public Modelos.Entidades.Reserva ObtenerPorId(int id)
    {
        return _contexto.Reservas.
            Include(r => r.IdHabitacionesNavigation)
            .FirstOrDefault(r => r.IdReservas == id);
    }

    public void ActualizarReserva(Modelos.Entidades.Reserva reserva)
    {
        _contexto.Reservas.Update(reserva);
        _contexto.SaveChanges();
    }
    
    public IEnumerable<Habitacione> ObtenerHabitacionesDisponibles(DateOnly ingreso, DateOnly salida)
    {
       
        return _contexto.Habitaciones
            .Include(h => h.IdTipoHabitacionNavigation) 
            .Where(h => !_contexto.Reservas.Any(r =>
                r.IdHabitaciones == h.IdHabitaciones &&
                r.IdEstados != (int)EstadoCancelado &&
                r.IdEstados != (int)EstadoFinalizado && 
                salida > r.FechaIngreso))
            .ToList();
    }
    
}
