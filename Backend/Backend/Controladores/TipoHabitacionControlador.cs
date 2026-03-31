using Backend.Patrones;
using Backend.Servicios.TipoHabitacion;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controladores;

[ApiController]
[Route("[controller]")]
public class TipoHabitacionControlador : ControllerBase
{
    private readonly ITipoHabitacionServicio _servicio;
    
    public TipoHabitacionControlador(ITipoHabitacionServicio servicio)
    {
        _servicio = servicio;
    }
    
    
    
    [HttpGet("Opciones")]
    public IActionResult ObtenerOpciones()
    {
        var opciones = _servicio.ObtenerOpciones();
        return Ok(opciones);
    }

  
}

