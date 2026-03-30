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
}
