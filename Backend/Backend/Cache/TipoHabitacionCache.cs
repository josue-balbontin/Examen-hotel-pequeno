namespace Backend.Patrones;

public class TipoHabitacionCache
{
    private static readonly Lazy<TipoHabitacionCache> _instancia = new(() => new TipoHabitacionCache());
    
    public System.Collections.Concurrent.ConcurrentDictionary<int, Modelos.Entidades.TipoHabitacione> Datos { get; } = new();

    private TipoHabitacionCache() { }

    public static TipoHabitacionCache ObtenerInstancia()
    {
        return _instancia.Value;
    }
    
    public Modelos.Entidades.TipoHabitacione ObtenerDetalle(int idTipoHabitacion)
    {
        if (Datos.TryGetValue(idTipoHabitacion, out var detalle))
        {
            return detalle;
        }

        throw new KeyNotFoundException("Tipo de habitación no encontrado en caché.");
    }
}