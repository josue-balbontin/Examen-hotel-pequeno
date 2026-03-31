using Backend.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controladores;

[ApiController]
[Route("[controller]")]
public class ServicioControlador : ControllerBase
{
    private readonly IServicioServicio _servicio;

    public ServicioControlador(IServicioServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet("Contactos")]
    public IActionResult ObtenerContactos()
    {
        try
        {
            var contactos = _servicio.ObtenerContactos().ToList();

            if (contactos.Count == 0)
            {
                return Ok(new { mensaje = "No hay contactos disponibles." });
            }

            return Ok(contactos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Ocurrió un error interno en el servidor: " + ex.Message });
        }
    }
}

