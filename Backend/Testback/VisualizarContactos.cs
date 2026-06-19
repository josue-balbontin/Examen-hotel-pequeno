using Backend.Modelos.Entidades;
using Backend.Repositorio.Servicio;
using Backend.Servicios;
using Moq;

namespace Testback;

[TestFixture]
// HU6
public class VisualizarContactos
{
    private Mock<IServicioRepositorio> _repositorioMock;
    private ServicioServicio _servicio;

    [SetUp]
    public void SetUp()
    {
        _repositorioMock = new Mock<IServicioRepositorio>();
        _servicio = new ServicioServicio(_repositorioMock.Object);
    }

    [Test]
    public void ObtenerContactos_ContactosCargados_MuestraListaConInfoRequerida()
    {
        /*
        Dado que existen contactos cargados en la base de datos, cuando el usuario
        ingrese a la página de servicios, entonces el sistema debe mostrar la lista de
        contactos disponibles.
        Dado que cada servicio tiene información registrada, cuando se visualice en
        la página, entonces deben mostrarse al menos el nombre del servicio,
        encargado y teléfono.
        */
        
        var contactos = new List<Servicio>
        {
            new Servicio { NombreServicio = "Limpieza", Encargado = "Maria", Telefono = "123456789" },
            new Servicio { NombreServicio = "Mantenimiento", Encargado = "Jose", Telefono = "987654321" }
        };
        _repositorioMock.Setup(r => r.ObtenerTodos()).Returns(contactos);

        var listado = _servicio.ObtenerContactos().ToList();

        Assert.That(listado.Count, Is.EqualTo(2));
        Assert.That(listado[0].Encargado, Is.EqualTo("Maria"));
    }
        
    [Test]
    public void ObtenerContactos_SinInformacion_RetornaVacio()
    {
        /*
        Dado que no existe información cargada, cuando se abra la página, entonces
        el sistema debe informar que no hay contactos disponibles.
        */
        
        _repositorioMock.Setup(r => r.ObtenerTodos()).Returns(new List<Servicio>());

        var listado = _servicio.ObtenerContactos().ToList();

        Assert.That(listado, Is.Empty);
    }
    
}