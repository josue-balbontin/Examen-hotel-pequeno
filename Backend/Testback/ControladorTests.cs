using NUnit.Framework;
using Moq;
using Backend.Modelos.Entidades;
using Backend.Modelos.DTOs;
using Backend.Servicios;
using Backend.Controladores;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Testback
{
    [TestFixture]
    public class ControladorTests
    {
        private Mock<IReservaServicio> _reservaServicioMock;
        private ReservaControlador _reservaControlador;

        private Mock<IUsuarioServicio> _usuarioServicioMock;
        private UsuarioControlador _usuarioControlador;

        [SetUp]
        public void SetUp()
        {
            _reservaServicioMock = new Mock<IReservaServicio>();
            _reservaControlador = new ReservaControlador(_reservaServicioMock.Object);

            _usuarioServicioMock = new Mock<IUsuarioServicio>();
            _usuarioControlador = new UsuarioControlador(_usuarioServicioMock.Object);
        }

        [Test]
        public void CrearReserva_RetornaOk_CuandoServicioNoLanzaExcepcion()
        {
            var dto = new CrearReservaDto { IdsUsuarios = new List<int> { 1 }, IdHabitacion = 10, FechaIngreso = DateOnly.FromDateTime(DateTime.Now), FechaSalida = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) };
            var result = _reservaControlador.CrearReserva(dto) as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void CrearReserva_RetornaBadRequest_CuandoArgumentException()
        {
            var dto = new CrearReservaDto { IdsUsuarios = new List<int> { 1 }, IdHabitacion = 10, FechaIngreso = DateOnly.FromDateTime(DateTime.Now), FechaSalida = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) };
            _reservaServicioMock.Setup(s => s.CrearReserva(dto)).Throws(new ArgumentException("Error"));
            var result = _reservaControlador.CrearReserva(dto) as BadRequestObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void CrearReserva_RetornaConflict_CuandoInvalidOperationException()
        {
            var dto = new CrearReservaDto { IdsUsuarios = new List<int> { 1 }, IdHabitacion = 10, FechaIngreso = DateOnly.FromDateTime(DateTime.Now), FechaSalida = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) };
            _reservaServicioMock.Setup(s => s.CrearReserva(dto)).Throws(new InvalidOperationException("Error"));
            var result = _reservaControlador.CrearReserva(dto) as ConflictObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(409));
        }

        [Test]
        public void HacerCheckIn_RetornaOk()
        {
            var result = _reservaControlador.HacerCheckIn(1) as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void HacerCheckOut_RetornaOk()
        {
            var result = _reservaControlador.HacerCheckOut(1) as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void CancelarReserva_RetornaOk()
        {
            var result = _reservaControlador.CancelarReserva(1) as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ObtenerDisponibilidad_RetornaOk()
        {
            _reservaServicioMock.Setup(s => s.BuscarDisponibilidad(It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).Returns(new List<Habitacione>());
            var result = _reservaControlador.ObtenerDisponibilidad(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(1))) as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void RegistrarUsuario_RetornaOk()
        {
            var dto = new RegistrarUsuarioDTO();
            var result = _usuarioControlador.Registrar(dto) as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void RegistrarUsuario_RetornaConflict_CuandoInvalidOperationException()
        {
            var dto = new RegistrarUsuarioDTO();
            _usuarioServicioMock.Setup(s => s.RegistrarUsuario(It.IsAny<Usuario>())).Throws(new InvalidOperationException("Error"));
            var result = _usuarioControlador.Registrar(dto) as ConflictObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(409));
        }
        
        [Test]
        public void ObtenerUsuarios_RetornaOk()
        {
            _usuarioServicioMock.Setup(s => s.ObtenerUsuarios()).Returns(new List<Usuario>());
            var result = _usuarioControlador.ObtenerUsuarios() as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }
    }
}
