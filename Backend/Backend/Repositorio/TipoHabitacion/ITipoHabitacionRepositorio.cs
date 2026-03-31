namespace Backend.Repositorio.TipoHabitacion;

public interface ITipoHabitacionRepositorio
{
    IEnumerable<Modelos.Entidades.TipoHabitacione> ObtenerTodos();
}