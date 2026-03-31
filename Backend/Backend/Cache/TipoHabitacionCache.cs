namespace Backend.Patrones;

public class TipoHabitacionCache
{
    private static TipoHabitacionCache? _instancia;
    
    public Dictionary<int, Modelos.Entidades.TipoHabitacione> Datos { get; set; } = new();

    private TipoHabitacionCache() { }

    public static TipoHabitacionCache ObtenerInstancia()
    {
        if (_instancia == null)
        {
            _instancia = new TipoHabitacionCache();
        }
        return _instancia;
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