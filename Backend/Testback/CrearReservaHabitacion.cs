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
    
    
    
}