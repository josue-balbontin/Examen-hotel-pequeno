using System;
using Backend.Modelos.Entidades;
using Backend.Repositorio.Configuracion;
using Backend.Repositorio.Reserva;
using Backend.Servicios;
using Moq;
using NUnit.Framework;
using Backend.Modelos.Enums;

namespace Testback;

[TestFixture]
// (Historia Adicional) - Cancelar Reserva
public class CancelarReserva
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
    public void CancelarReserva_EstadoReservado_CambiaEstadoYActualiza()
    {
        /*Dado que existe una reserva válida en estado 'Reservado', 
        cuando el usuario proceda a cancelarla, entonces el sistema debe actualizar
        su estado a 'Cancelado'.*/
        
        var reserva = new Reserva { IdReservas = 1, IdEstados = (int)EstadosReservaEnum.EstadoReservado };
        _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);

        _reservaServicio.CancelarReserva(1);

        Assert.That(reserva.IdEstados, Is.EqualTo((int)EstadosReservaEnum.EstadoCancelado));
        _reservaRepoMock.Verify(r => r.ActualizarReserva(reserva), Times.Once);
    }
}
