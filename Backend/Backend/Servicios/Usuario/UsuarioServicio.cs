using Backend.Modelos;
using Backend.Repositorio.Usuario;

namespace Backend.Servicios;

public class UsuarioServicio : IUsuarioServicio
{
    private readonly IUsuarioRepositorio _repositorio;

    public UsuarioServicio(IUsuarioRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public void RegistrarUsuario(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.Nombres) || string.IsNullOrWhiteSpace(usuario.Apellidos) ||  string.IsNullOrWhiteSpace(usuario.DocumentoIdentidad))
        {
            throw new ArgumentException("Los campos Nombres, Apellidos y Documento de Identidad son obligatorios.");
        }

        
        if (_repositorio.ExisteDocumento(usuario.DocumentoIdentidad))
        {
            throw new InvalidOperationException("Ya existe un huésped registrado con este documento de identidad.");
        }

        _repositorio.Agregar(usuario);
    }
}