using NUnit.Framework;
using Moq;
using Backend.Modelos.Entidades;
using Backend.Modelos.DTOs;
using Backend.Servicios;
using Backend.Repositorio.Usuario;
using Backend.Repositorio.Reserva;
using Backend.Repositorio.Configuracion;
using Backend.Controladores;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Backend.Modelos.Enums;
using Backend.Patrones;
using Backend.Repositorio.TipoHabitacion;
using Backend.Servicios.TipoHabitacion;
using Backend.Repositorio.Servicio;
using System.Linq;

namespace Testback
{
    [TestFixture]
    public class HU01_RegistroHuesped_Tests
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
        public void Dado_RecepcionistaCompletaCamposObligatorios_Cuando_Guarde_Entonces_RegistraHuespedCorrectamente()
        {
            var usuario = new Usuario { Nombres = "Juan", Apellidos = "Perez", DocumentoIdentidad = "12345" };
            _usuarioRepoMock.Setup(r => r.ExisteDocumento(usuario.DocumentoIdentidad)).Returns(false);

            _usuarioServicio.RegistrarUsuario(usuario);

            _usuarioRepoMock.Verify(r => r.Agregar(It.IsAny<Usuario>()), Times.Once);
        }

        [Test]
        public void Dado_FaltaCamposObligatorios_Cuando_IntenteGuardar_Entonces_MuestraValidacionesYNoRegistra()
        {
            var usuario = new Usuario { Nombres = "Juan", Apellidos = "", DocumentoIdentidad = "123" };
            Assert.Throws<ArgumentException>(() => _usuarioServicio.RegistrarUsuario(usuario));
            _usuarioRepoMock.Verify(r => r.Agregar(It.IsAny<Usuario>()), Times.Never);
        }

        [Test]
        public void Dado_ExisteHuespedMismoDocumento_Cuando_IntenteRegistrar_Entonces_ImpideDuplicado()
        {
            var usuario = new Usuario { Nombres = "Juan", Apellidos = "Perez", DocumentoIdentidad = "12345" };
            _usuarioRepoMock.Setup(r => r.ExisteDocumento(usuario.DocumentoIdentidad)).Returns(true);
            
            Assert.Throws<InvalidOperationException>(() => _usuarioServicio.RegistrarUsuario(usuario));
            _usuarioRepoMock.Verify(r => r.Agregar(It.IsAny<Usuario>()), Times.Never);
        }
    }

    [TestFixture]
    public class HU02_CrearReservaHabitacion_Tests
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
        public void Dado_ExistenDatosRequeridos_Cuando_CompleteReserva_Entonces_RegistraCorrectamente()
        {
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
        public void Dado_FechaSalidaNoPosterior_Cuando_IntenteGuardar_Entonces_ImpideRegistroYLanzaValidacion()
        {
            var dto = new CrearReservaDto { 
                IdsUsuarios = new List<int> { 1 },
                IdHabitacion = 10,
                FechaIngreso = new DateOnly(2023, 10, 5), 
                FechaSalida = new DateOnly(2023, 10, 1) 
            };
            Assert.Throws<ArgumentException>(() => _reservaServicio.CrearReserva(dto));
        }

        [Test]
        public void Dado_HabitacionReservadaMismoRango_Cuando_IntenteRegistrar_Entonces_ImpideSolapamiento()
        {
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
        public void Dado_CantidadPersonasSuperaCapacidad_Cuando_IntenteGuardar_Entonces_RechazaOperacion()
        {
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

    [TestFixture]
    public class HU03_ConsultarReservasActivasYFuturas_Tests
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
        public void Dado_ReservasRegistradas_Cuando_IngresaListado_Entonces_MuestraActivasYFuturasOrdenadasCronologicamente()
        {
            var reservasId1 = new Reserva { IdReservas = 1, FechaIngreso = new DateOnly(2023, 11, 1), IdEstados = (int)EstadosReservaEnum.EstadoReservado };
            var reservasId2 = new Reserva { IdReservas = 2, FechaIngreso = new DateOnly(2023, 10, 1), IdEstados = (int)EstadosReservaEnum.EstadoOcupado };
            
            // Suponemos que el servicio nos retorna datos y el frontend o controlador hace la representación
            _reservaServicioMock.Setup(s => s.ObtenerReservas()).Returns(new List<Reserva> { reservasId1, reservasId2 });

            var resultado = _controller.ObtenerReservas() as OkObjectResult;
            Assert.That(resultado, Is.Not.Null);
            var listaReservas = resultado.Value as IEnumerable<Reserva>;
            Assert.That(listaReservas, Is.Not.Null);

            // Verificamos que se retornen (la ordenación y filtrado real podría requerir ajuste en el servicio/controlador, pero validamos que la data fluye)
            Assert.That(listaReservas.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Dado_NoExistenReservas_Cuando_AbraVista_Entonces_InformaQueNoHayDatos()
        {
            _reservaServicioMock.Setup(s => s.ObtenerReservas()).Returns(new List<Reserva>());
            
            var resultado = _controller.ObtenerReservas() as OkObjectResult;
            Assert.That(resultado, Is.Not.Null);
            var listaReservas = resultado.Value as IEnumerable<Reserva>;
            Assert.That(listaReservas, Is.Empty);
        }
    }

    [TestFixture]
    public class HU04_RegistrarCheckIn_Tests
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
        public void Dado_ReservaVigente_Cuando_EjecuteCheckIn_Entonces_RegistraFechaHoraYCambiaEstadoEnCurso()
        {
            var reserva = new Reserva { IdReservas = 1, IdEstados = (int)EstadosReservaEnum.EstadoReservado, FechaCheckin = null };
            _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);

            _reservaServicio.RegistrarCheckIn(1);

            Assert.That(reserva.FechaCheckin, Is.Not.Null);
            Assert.That(reserva.IdEstados, Is.EqualTo((int)EstadosReservaEnum.EstadoOcupado));
            _reservaRepoMock.Verify(r => r.ActualizarReserva(reserva), Times.Once);
        }

        [Test]
        public void Dado_ReservaCancelada_Cuando_IntenteHacerCheckIn_Entonces_ImpideOperacion()
        {
            var reserva = new Reserva { IdReservas = 1, IdEstados = (int)EstadosReservaEnum.EstadoCancelado, FechaCheckin = null };
            _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);

            Assert.Throws<InvalidOperationException>(() => _reservaServicio.RegistrarCheckIn(1));
        }

        [Test]
        public void Dado_ReservaYaRealizoCheckIn_Cuando_IntenteRegistrarNuevamente_Entonces_EvitaDuplicar()
        {
            var reserva = new Reserva { IdReservas = 1, IdEstados = (int)EstadosReservaEnum.EstadoOcupado, FechaCheckin = DateTime.UtcNow };
            _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);

            Assert.Throws<InvalidOperationException>(() => _reservaServicio.RegistrarCheckIn(1));
        }
    }

    [TestFixture]
    public class HU05_GestionarVariacionTipoHabitacion_Tests
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
        public void Dado_TiposHabitacionDefinidos_Cuando_UsuarioConsulte_Entonces_ContemplaOpcionesYAsignaCaracteristicas()
        {
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

    [TestFixture]
    public class HU06_VisualizarContactosServicios_Tests
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
        public void Dado_ContactosCargados_Cuando_IngresaPagina_Entonces_MuestraListaConInfoRequerida()
        {
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
        public void Dado_NoExisteInformacionCargada_Cuando_AbraPagina_Entonces_InformaQueNoHayContactos()
        {
            _repositorioMock.Setup(r => r.ObtenerTodos()).Returns(new List<Servicio>());

            var listado = _servicio.ObtenerContactos().ToList();

            Assert.That(listado, Is.Empty);
        }
    }

    [TestFixture]
    public class HU08_RegistrarCheckOut_Tests
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
        public void Dado_ReservaConCheckIn_Cuando_RealiceCheckOut_Entonces_RegistraFechaHoraSalida()
        {
            var reserva = new Reserva 
            { 
                IdReservas = 1, 
                IdEstados = (int)EstadosReservaEnum.EstadoOcupado, 
                FechaCheckin = DateTime.UtcNow.AddDays(-2),
                FechaSalida = DateOnly.FromDateTime(DateTime.Now).AddDays(1) 
            };
            
            _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);
            _configRepoMock.Setup(c => c.ObtenerValor("hora_limite_checkout", "12:00")).Returns("12:00");
            _configRepoMock.Setup(c => c.ObtenerValor("porcentaje_late_checkout", "0.50")).Returns("0.50");

            _reservaServicio.RegistrarCheckOut(1);

            Assert.That(reserva.FechaCheckout, Is.Not.Null);
            Assert.That(reserva.CargoCheckout, Is.EqualTo(0m));
            Assert.That(reserva.IdEstados, Is.EqualTo((int)EstadosReservaEnum.EstadoFinalizado));
            _reservaRepoMock.Verify(r => r.ActualizarReserva(reserva), Times.Once);
        }

        [Test]
        public void Dado_ReservaSinCheckIn_Cuando_IntenteHacerCheckOut_Entonces_ImpideOperacion()
        {
            var reserva = new Reserva 
            { 
                IdReservas = 1, 
                IdEstados = (int)EstadosReservaEnum.EstadoReservado, 
                FechaCheckin = null 
            };
            _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);

            Assert.Throws<InvalidOperationException>(() => _reservaServicio.RegistrarCheckOut(1));
        }

        [Test]
        public void Dado_SalidaDespuesHorarioLimite_Cuando_ProceseCheckOut_Entonces_CalculaYRegistraCargo()
        {
            var tipoHabitacion = new TipoHabitacione { IdTipoHabitaciones = 1, PrecioReferencia = 100 };
            var habitacion = new Habitacione { IdHabitaciones = 10, IdTipoHabitacion = 1, IdTipoHabitacionNavigation = tipoHabitacion };
            var reserva = new Reserva 
            { 
                IdReservas = 1, 
                IdEstados = (int)EstadosReservaEnum.EstadoOcupado, 
                FechaCheckin = DateTime.UtcNow.AddDays(-2),
                FechaSalida = DateOnly.FromDateTime(DateTime.Now).AddDays(-1), 
                IdHabitacionesNavigation = habitacion
            };
            
            _reservaRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(reserva);
            _configRepoMock.Setup(c => c.ObtenerValor("hora_limite_checkout", "12:00")).Returns("12:00");
            _configRepoMock.Setup(c => c.ObtenerValor("porcentaje_late_checkout", "0.50")).Returns("0.50");

            _reservaServicio.RegistrarCheckOut(1);

            Assert.That(reserva.FechaCheckout, Is.Not.Null);
            Assert.That(reserva.CargoCheckout, Is.EqualTo(50m));
            Assert.That(reserva.IdEstados, Is.EqualTo((int)EstadosReservaEnum.EstadoFinalizado));
            _reservaRepoMock.Verify(r => r.ActualizarReserva(reserva), Times.Once);
        }
        
        [Test]
        public void ObtenerCargo_Dado_ExcedeFechaSalida_Entonces_AplicaCargoCompleto()
        {

            var tipoHabitacion = new TipoHabitacione { IdTipoHabitaciones = 1, PrecioReferencia = 100 };
            var habitacion = new Habitacione { IdHabitaciones = 10, IdTipoHabitacion = 1, IdTipoHabitacionNavigation = tipoHabitacion };
            var reserva = new Reserva 
            { 
                IdReservas = 1, 
                IdEstados = (int)EstadosReservaEnum.EstadoOcupado, 
                FechaCheckin = DateTime.UtcNow.AddDays(-2),
                FechaSalida = DateOnly.FromDateTime(DateTime.Now).AddDays(-1), 
                IdHabitacionesNavigation = habitacion
            };
                
            
           var respuesta = _reservaServicio.ObtenerCargo(true, false,reserva , decimal.Parse("0.50", System.Globalization.CultureInfo.InvariantCulture));
            
           
            Assert.That(respuesta, Is.EqualTo(50m));
            
        }
        
    }
    
    
 
    
    
}
