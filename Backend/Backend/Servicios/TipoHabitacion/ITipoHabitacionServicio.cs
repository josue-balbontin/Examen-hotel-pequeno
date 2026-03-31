namespace Backend.Servicios.TipoHabitacion;

public interface ITipoHabitacionServicio
{
    public IEnumerable<Modelos.Entidades.TipoHabitacione> ObtenerOpciones();
}