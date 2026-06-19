using Backend.Modelos.Entidades;
using Backend.Modelos.Enums;
using Backend.Repositorio.Configuracion;
using Backend.Repositorio.Reserva;
using Backend.Servicios;
using Moq;

namespace Testback;

[TestFixture]
// HU 8
public class RegistrarCheckOut
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
        public void RegistrarCheckOut_ReservaConCheckIn_RegistraFechaHoraSalida()
        {
            /*
            Dado que una reserva tiene check-in registrado, cuando el usuario realice el
            check-out, entonces el sistema debe registrar la fecha y hora de salida.
            */
            
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
        public void RegistrarCheckOut_SinCheckIn_TiraExcepcion()
        {
            /*
            Dado que la reserva no tiene check-in previo, cuando se intente hacer check
            out, entonces el sistema debe impedir la operación.
            */
            
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
        public void RegistrarCheckOut_SalidaDespuesLimite_CalculaYRegistraCargo()
        {
            /*
            Dado que la salida ocurre después del horario límite definido, cuando se
            procese el check-out, entonces el sistema debe calcular y registrar el cargo
            por late check-out.
            */
            
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
            /*
            Dado que la salida ocurre después del horario límite definido, cuando se
            procese el check-out, entonces el sistema debe calcular y registrar el cargo
            por late check-out 
            */
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