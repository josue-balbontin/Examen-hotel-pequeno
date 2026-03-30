using Backend.Modelos.DTOs;

namespace Backend.Servicios;

public interface IReservaServicio
{
    void CrearReserva(CrearReservaDTO dto);
}
