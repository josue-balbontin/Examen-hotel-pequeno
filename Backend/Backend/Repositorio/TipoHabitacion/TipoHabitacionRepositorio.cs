using Backend.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositorio.TipoHabitacion;

public class TipoHabitacionRepositorio : ITipoHabitacionRepositorio
{
    private readonly HotelDbContext _contexto;

    public TipoHabitacionRepositorio(HotelDbContext contexto)
    {
        _contexto = contexto;
    }

    public IEnumerable<TipoHabitacione> ObtenerTodos()
    {
        return _contexto.TipoHabitaciones.AsNoTracking().ToList();
    }
}