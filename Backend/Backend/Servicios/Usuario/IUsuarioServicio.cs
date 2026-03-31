using Backend.Modelos.Entidades;

namespace Backend.Servicios;

public interface IUsuarioServicio
{
    void RegistrarUsuario(Usuario usuario);

    IEnumerable<Usuario> ObtenerUsuarios();
}