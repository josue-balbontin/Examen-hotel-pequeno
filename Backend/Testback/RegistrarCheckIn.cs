using Backend.Modelos.Entidades;
using Backend.Modelos.Enums;
using Backend.Repositorio.Configuracion;
using Backend.Repositorio.Reserva;
using Backend.Servicios;
using Moq;

namespace Testback;

[TestFixture]
//HU 4
public class RegistrarCheckIn
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
    public void RegistrarCheckIn_Duplicacdo_TiraExcepcion()
    {   /*
        Dado que una reserva ya realizó check-in, cuando el usuario intente
        registrarlo nuevamente, entonces el sistema debe evitar duplicar la acción.
        */

        var reserva = new Reserva { IdReservas = 1, IdEstados = (int)EstadosReservaEnum.EstadoOcupado, FechaCheckin = DateTime.UtcNow };
        _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);

        Assert.Throws<InvalidOperationException>(() => _reservaServicio.RegistrarCheckIn(1));
    }
    
}