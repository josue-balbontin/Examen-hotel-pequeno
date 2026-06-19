using Backend.Modelos.Entidades;
using Backend.Patrones;
using Backend.Repositorio.TipoHabitacion;
using Backend.Servicios.TipoHabitacion;
using Moq;

namespace Testback;

[TestFixture]
// HU05
public class GestionarVariacionTipoHabitacion
{
    private Mock<ITipoHabitacionRepositorio> _repositorioMock;
    private TipoHabitacionServicio _servicio;

    [SetUp]
    public void SetUp()
    {
        _repositorioMock = new Mock<ITipoHabitacionRepositorio>();
        _servicio = new TipoHabitacionServicio(_repositorioMock.Object);
        TipoHabitacionCache.ObtenerInstancia().Datos.Clear(); 
    }

    [Test]
    public void ObtenerOpciones_TiposHabitacionDefinidos_ContemplaOpcionesYAsignaCaracteristicas()
    {
        /*
        Dado que existen tipos de habitación definidos, cuando el usuario consulte
        las opciones, entonces el sistema debe contemplar al menos: Simple, Suite, 
        Doble con camas individuales y Doble matrimonial.
        Dado que el usuario seleccione una variación de habitación, cuando el
        sistema procese la selección, entonces debe asignar automáticamente sus
        características base correspondientes, como capacidad, descripción o precio
        referencial.
        */
        
        var tipos = new List<TipoHabitacione>
        {
            new TipoHabitacione { IdTipoHabitaciones = 1, Nombre = "Simple", Capacidad = 1, PrecioReferencia = 50 },
            new TipoHabitacione { IdTipoHabitaciones = 2, Nombre = "Suite", Capacidad = 2, PrecioReferencia = 200 }
        };

        _repositorioMock.Setup(r => r.ObtenerTodos()).Returns(tipos);

        var opciones = _servicio.ObtenerOpciones().ToList();

        Assert.That(opciones.Count, Is.EqualTo(2));
        Assert.That(opciones.Any(o => o.Nombre == "Simple"), Is.True);
        Assert.That(opciones.Any(o => o.Nombre == "Suite"), Is.True);
    }
}