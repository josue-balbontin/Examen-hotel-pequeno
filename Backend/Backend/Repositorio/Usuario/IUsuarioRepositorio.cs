namespace Backend.Repositorio.Usuario;

using Backend.Modelos.Entidades;

public interface IUsuarioRepositorio
{
    bool ExisteDocumento(string documentoIdentidad);
    
    void Agregar(Usuario usuario);
    
}