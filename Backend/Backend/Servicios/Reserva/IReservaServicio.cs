using Backend.Modelos.DTOs;
using Backend.Modelos.Entidades;

namespace Backend.Servicios;

public interface IReservaServicio
{
    void CrearReserva(CrearReservaDTO dto);

    IEnumerable<Reserva> ObtenerReservas();
    
    void RegistrarCheckIn(int idReserva);

    void RegistrarCheckOut(int idReserva);

    public IEnumerable<Habitacione> BuscarDisponibilidad(DateOnly ingreso, DateOnly salida);
    
    void CancelarReserva(int idReserva);
    
}
