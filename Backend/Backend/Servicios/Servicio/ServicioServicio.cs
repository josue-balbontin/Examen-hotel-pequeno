using Backend.Modelos.Entidades;
using Backend.Repositorio.Servicio;

namespace Backend.Servicios;

public class ServicioServicio : IServicioServicio
{
    private readonly IServicioRepositorio _repositorio;

    public ServicioServicio(IServicioRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public IEnumerable<Servicio> ObtenerContactos()
    {
        return _repositorio.ObtenerTodos();
    }
}

