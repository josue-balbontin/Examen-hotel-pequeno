using Backend.Modelos.DTOs;
using Backend.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controladores;

[ApiController]
[Route("[controller]")]
public class ReservaControlador : ControllerBase
{
    private readonly IReservaServicio _servicio;

    public ReservaControlador(IReservaServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpPost("CrearReserva")]
    public IActionResult CrearReserva([FromBody] CrearReservaDTO dto)
    {
        try
        {
            _servicio.CrearReserva(dto);
            return Ok(new { mensaje = "Reserva creada exitosamente." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpGet("ObtenerReservas")]
    public IActionResult ObtenerReservas()
    {
        try
        {
            var reservas = _servicio.ObtenerReservas();
            return Ok(reservas);
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, new { error = "Ocurrió un error interno en el servidor: " + ex.Message });
        }
        

    }
    
    [HttpPut("/checkin/{idReserva}")]
    public IActionResult HacerCheckIn(int idReserva)
    {
        try
        {
            _servicio.RegistrarCheckIn(idReserva);
            return Ok(new { mensaje = "Check-in registrado exitosamente. El huésped ya está en estadía." });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message }); 
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message }); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Error interno: " + ex.Message });
        }
    }

    [HttpPut("/checkout/{idReserva}")]
    public IActionResult HacerCheckOut(int idReserva)
    {
        try
        {
            _servicio.RegistrarCheckOut(idReserva);
            return Ok(new { mensaje = "Check-out registrado exitosamente." });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Error interno: " + ex.Message });
        }
    }
    
    [HttpGet("Disponibilidad")]
    public IActionResult ObtenerDisponibilidad([FromQuery] DateOnly ingreso, [FromQuery] DateOnly salida)
    {
        try
        {
            var libres = _servicio.BuscarDisponibilidad(ingreso, salida);
            
            return Ok(libres);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    
    [HttpPut("/cancelar/{id}")]
    public IActionResult CancelarReserva(int id)
    {
        try
        {
            _servicio.CancelarReserva(id);
            return Ok(new { mensaje = "Reserva cancelada exitosamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    
}
