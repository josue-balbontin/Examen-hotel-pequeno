using Backend.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositorio.Servicio;

public class ServicioRepositorio : IServicioRepositorio
{
    private readonly HotelDbContext _contexto;

    public ServicioRepositorio(HotelDbContext contexto)
    {
        _contexto = contexto;
    }

    public IEnumerable<Modelos.Entidades.Servicio> ObtenerTodos()
    {
        return _contexto.Servicios.AsNoTracking().ToList();
    }
}

