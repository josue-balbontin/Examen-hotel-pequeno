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
    
    [Test]
    public void RegistrarCheckIn_ReservaVigente_RegistraFechaHoraYCambiaEstado()
    {
        /*
        Dado que existe una reserva vigente para la fecha correspondiente, cuando el
        usuario ejecute el check-in, entonces el sistema debe registrar la fecha y hora
        de ingreso.
        Dado que el check-in fue realizado correctamente, cuando finalice la
        operación, entonces la reserva debe cambiar a un estado que indique estadía
        en curso.
        */
        
        var reserva = new Reserva { IdReservas = 1, IdEstados = (int)EstadosReservaEnum.EstadoReservado, FechaCheckin = null };
        _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);

        _reservaServicio.RegistrarCheckIn(1);

        Assert.That(reserva.FechaCheckin, Is.Not.Null);
        Assert.That(reserva.IdEstados, Is.EqualTo((int)EstadosReservaEnum.EstadoOcupado));
        _reservaRepoMock.Verify(r => r.ActualizarReserva(reserva), Times.Once);
    }

    [Test]
    public void RegistrarCheckIn_ReservaCancelada_TiraExcepcion()
    {
        /*
        Dado que la reserva está cancelada, cuando se intente hacer check-in, 
        entonces el sistema debe impedir la operación.
        */
        
        var reserva = new Reserva { IdReservas = 1, IdEstados = (int)EstadosReservaEnum.EstadoCancelado, FechaCheckin = null };
        _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);

        Assert.Throws<InvalidOperationException>(() => _reservaServicio.RegistrarCheckIn(1));
    }

    
}