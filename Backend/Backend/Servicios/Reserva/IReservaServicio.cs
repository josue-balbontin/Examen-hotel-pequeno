using Backend.Modelos.DTOs;
using Backend.Modelos.Entidades;

namespace Backend.Servicios;

public interface IReservaServicio
{
    void CrearReserva(CrearReservaDTO dto);

    IEnumerable<Reserva> ObtenerReservas();
    
    void RegistrarCheckIn(int idReserva);

    void RegistrarCheckOut(int idReserva);
    
}
