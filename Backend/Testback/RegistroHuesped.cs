using Backend.Modelos.Entidades;
using Backend.Repositorio.Usuario;
using Backend.Servicios;
using Moq;

namespace Testback;

[TestFixture]
//HU 1 
public class RegistroHuesped
{
    
    private Mock<IUsuarioRepositorio> _usuarioRepoMock;
    private UsuarioServicio _usuarioServicio;

    [SetUp]
    public void SetUp()
    {
        _usuarioRepoMock = new Mock<IUsuarioRepositorio>();
        _usuarioServicio = new UsuarioServicio(_usuarioRepoMock.Object);
    }
        
        

    [Test]
    public void RegistrarUsuario_CamposObligatorios_RegistraHuespedCorrectamente()
    {
        /*
        Dado que la recepcionista accede al formulario de registro, cuando complete
        los campos obligatorios y guarde, entonces el sistema debe registrar
        correctamente al huésped.
        */
        
        var usuario = new Usuario { Nombres = "Juan", Apellidos = "Perez", DocumentoIdentidad = "12345" };
        _usuarioRepoMock.Setup(r => r.ExisteDocumento(usuario.DocumentoIdentidad)).Returns(false);

        _usuarioServicio.RegistrarUsuario(usuario);

        _usuarioRepoMock.Verify(r => r.Agregar(It.IsAny<Usuario>()), Times.Once);
    }

    [Test]
    public void RegistrarUsuario_FaltaCampos_TiraExcepcion()
    {
        /*
        Dado que falta uno o más campos obligatorios, cuando intente guardar el
        formulario, entonces el sistema debe mostrar validaciones y no registrar el
        huésped.
        */
        
        var usuario = new Usuario { Nombres = "Juan", Apellidos = "", DocumentoIdentidad = "123" };
        Assert.Throws<ArgumentException>(() => _usuarioServicio.RegistrarUsuario(usuario));
        _usuarioRepoMock.Verify(r => r.Agregar(It.IsAny<Usuario>()), Times.Never);
    }

    [Test]
    public void RegistrarUsuario_Duplicado_TiraExcepcion()
    {
        /*
        Dado que ya existe un huésped con el mismo documento de identidad, 
        cuando se intente registrar nuevamente, entonces el sistema debe impedir el
        duplicado.
        */
        
        var usuario = new Usuario { Nombres = "Juan", Apellidos = "Perez", DocumentoIdentidad = "12345" };
        _usuarioRepoMock.Setup(r => r.ExisteDocumento(usuario.DocumentoIdentidad)).Returns(true);
            
        Assert.Throws<InvalidOperationException>(() => _usuarioServicio.RegistrarUsuario(usuario));
        _usuarioRepoMock.Verify(r => r.Agregar(It.IsAny<Usuario>()), Times.Never);
    }
    
}