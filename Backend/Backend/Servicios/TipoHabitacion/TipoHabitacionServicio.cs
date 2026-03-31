using Backend.Patrones;
using Backend.Repositorio.TipoHabitacion;

namespace Backend.Servicios.TipoHabitacion;

public class TipoHabitacionServicio : ITipoHabitacionServicio
{
    private readonly ITipoHabitacionRepositorio _repositorio;

    public TipoHabitacionServicio(ITipoHabitacionRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public IEnumerable<Modelos.Entidades.TipoHabitacione> ObtenerOpciones()
    {
        var cache = TipoHabitacionCache.ObtenerInstancia();
        
        if (cache.Datos.Count == 0)
        {
            var tipos = _repositorio.ObtenerTodos(); 
            
            foreach (var tipo in tipos)
            {
                cache.Datos[tipo.IdTipoHabitaciones] = tipo;
            }
        }
        
        return cache.Datos.Values.ToList();
    }
}