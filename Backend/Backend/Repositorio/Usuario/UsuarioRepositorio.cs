using Backend.Modelos;

namespace Backend.Repositorio.Usuario;



public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly HotelDbContext _contexto;

    public UsuarioRepositorio(HotelDbContext contexto)
    {
       _contexto = contexto;
    }

    public bool ExisteDocumento(string documentoIdentidad)
    {
        return _contexto.Usuarios.Any(u => u.DocumentoIdentidad == documentoIdentidad);
    }

    public void Agregar(Modelos.Usuario usuario)
    {
        _contexto.Usuarios.Add(usuario);
        _contexto.SaveChanges(); 
    }
}