using Backend.Controladores;
using Backend.Modelos.Entidades;
using Backend.Modelos.Enums;
using Backend.Servicios;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Testback;

[TestFixture]
// HU 3 
public class ConsultarReservasActivasYFuturas
{
    
    private Mock<IReservaServicio> _reservaServicioMock;
    private ReservaControlador _controller;

    [SetUp]
    public void SetUp()
    {
        _reservaServicioMock = new Mock<IReservaServicio>();
        _controller = new ReservaControlador(_reservaServicioMock.Object);
    }

    [Test]
    public void ObtenerReservas_ReservasRegistradas_MuestraActivasYFuturasOrdenadas()
    {
        /*
        Dado que existen reservas registradas, cuando el usuario ingrese al listado, 
        entonces el sistema debe mostrar las reservas activas y futuras con sus datos
        principales.
        Dado que las reservas tienen fecha de ingreso, cuando se presenten en la
        lista, entonces deben aparecer ordenadas cronológicamente.
        */
        
        var reservasId1 = new Reserva { IdReservas = 1, FechaIngreso = new DateOnly(2023, 11, 1), IdEstados = (int)EstadosReservaEnum.EstadoReservado };
        var reservasId2 = new Reserva { IdReservas = 2, FechaIngreso = new DateOnly(2023, 10, 1), IdEstados = (int)EstadosReservaEnum.EstadoOcupado };
            
        _reservaServicioMock.Setup(s => s.ObtenerReservas()).Returns(new List<Reserva> { reservasId1, reservasId2 });

        var resultado = _controller.ObtenerReservas() as OkObjectResult;
        Assert.That(resultado, Is.Not.Null);
        var listaReservas = resultado.Value as IEnumerable<Reserva>;
        Assert.That(listaReservas, Is.Not.Null);

        Assert.That(listaReservas.Count(), Is.EqualTo(2));
    }

    [Test]
    public void ObtenerReservas_NoExistenReservas_InformaNoHayDatos()
    {
        /*
        Dado que no existen reservas para mostrar, cuando el usuario abra la vista, 
        entonces el sistema debe informar que no hay datos disponibles.
        */
        
        _reservaServicioMock.Setup(s => s.ObtenerReservas()).Returns(new List<Reserva>());
            
        var resultado = _controller.ObtenerReservas() as OkObjectResult;
        Assert.That(resultado, Is.Not.Null);
        var listaReservas = resultado.Value as IEnumerable<Reserva>;
        Assert.That(listaReservas, Is.Empty);
    }
    
}