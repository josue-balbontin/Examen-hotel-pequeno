using Backend.Modelos.DTOs;
using Backend.Modelos.Entidades;
using Backend.Repositorio.Configuracion;
using Backend.Repositorio.Reserva;
using Backend.Servicios;
using Moq;

namespace Testback;

[TestFixture]
// HU2 
public class CrearReservaHabitacion
{
    private Mock<IReservaRepositorio> _reservaRepoMock;
    private Mock<IConfiguracionRepositorio> _configRepoMock;
    private ReservaServicio _reservaServicio;

    [SetUp]
    public void SetUp()
    {
        _reservaRepoMock = new Mock<IReservaRepositorio>();
        _configRepoMock = new Mock<IConfiguracionRepositorio>();
        _reservaServicio = new ReservaServicio(_reservaRepoMock.Object, _configRepoMock.Object);
    }
    
    [Test]
    public void RegistrarReserva_CamposCompletos_RegistroCorrecto()
    {
        /*Dado que existen huéspedes y habitaciones precargadas, cuando el usuario
        complete los datos requeridos de la reserva, entonces el sistema debe
        registrarla correctamente.*/
        
        var dto = new CrearReservaDto {
            IdsUsuarios = new List<int> { 1, 2 },
            IdHabitacion = 10,
            FechaIngreso = new DateOnly(2023, 10, 1),
            FechaSalida = new DateOnly(2023, 10, 5)
        };

        _reservaRepoMock.Setup(r => r.ObtenerCapacidadHabitacion(10)).Returns(2);
        _reservaRepoMock.Setup(r => r.ExisteSolapamiento(10, dto.FechaIngreso, dto.FechaSalida)).Returns(false);

        _reservaServicio.CrearReserva(dto);

        _reservaRepoMock.Verify(r => r.Crear(It.IsAny<Reserva>(), dto.IdsUsuarios), Times.Once);
    }
    
    [Test]
    public void RegistrarReserva_FechaSalidaInvalida_TiraExcepcion()
    {
        /*
        Dado que la fecha de salida no es posterior a la fecha de ingreso, cuando se
        intente guardar la reserva, entonces el sistema debe impedir el registro y
        mostrar una validación.
        */
        
        var dto = new CrearReservaDto { 
            IdsUsuarios = new List<int> { 1 },
            IdHabitacion = 10,
            FechaIngreso = new DateOnly(2023, 10, 5), 
            FechaSalida = new DateOnly(2023, 10, 1) 
        };
        Assert.Throws<ArgumentException>(() => _reservaServicio.CrearReserva(dto));
    }

    [Test]
    public void RegistrarReserva_Solapamiento_TiraExcepcion()
    {
        /*
        Dado que una habitación ya está reservada en el mismo rango de fechas, 
        cuando se intente registrar una nueva reserva para esa habitación, entonces
        el sistema debe impedir el solapamiento.
        */
        
        var dto = new CrearReservaDto {
            IdsUsuarios = new List<int> { 1 },
            IdHabitacion = 10,
            FechaIngreso = new DateOnly(2023, 10, 1),
            FechaSalida = new DateOnly(2023, 10, 5)
        };
        _reservaRepoMock.Setup(r => r.ObtenerCapacidadHabitacion(10)).Returns(2);
        _reservaRepoMock.Setup(r => r.ExisteSolapamiento(10, dto.FechaIngreso, dto.FechaSalida)).Returns(true);

        Assert.Throws<InvalidOperationException>(() => _reservaServicio.CrearReserva(dto));
    }

    [Test]
    public void RegistrarReserva_SuperaCapacidad_TiraExcepcion()
    {
        /*
        Dado que la cantidad de personas supera la capacidad de la habitación, 
        cuando se intente guardar la reserva, entonces el sistema debe rechazar la
        operación.
        */
        
        var dto = new CrearReservaDto {
            IdsUsuarios = new List<int> { 1, 2, 3 },
            IdHabitacion = 10,
            FechaIngreso = new DateOnly(2023, 10, 1),
            FechaSalida = new DateOnly(2023, 10, 5)
        };
        _reservaRepoMock.Setup(r => r.ObtenerCapacidadHabitacion(10)).Returns(2);

        Assert.Throws<ArgumentException>(() => _reservaServicio.CrearReserva(dto));
    }
}