using Backend.Modelos.Entidades;

namespace Backend.Repositorio.Reserva;

public interface IReservaRepositorio
{
    int ObtenerCapacidadHabitacion(int idHabitacion);

    bool ExisteSolapamiento(int idHabitacion, DateOnly ingreso, DateOnly salida);

    void Crear(Modelos.Entidades.Reserva reserva, List<int> idsUsuarios);

    List<Modelos.Entidades.Reserva> ObtenerTodas();

    Modelos.Entidades.Reserva ObtenerPorId(int id);
    
    void ActualizarReserva(Modelos.Entidades.Reserva reserva);

}
