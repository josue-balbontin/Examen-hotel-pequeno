using Backend.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

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
}
