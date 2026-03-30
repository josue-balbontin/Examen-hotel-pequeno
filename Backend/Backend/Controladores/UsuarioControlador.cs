using Backend.Modelos.Entidades;
using Backend.Modelos.DTOs;
using Backend.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controladores;


[ApiController]
[Route("[controller]")]
public class UsuarioControlador : ControllerBase
{
    private readonly IUsuarioServicio _servicio; 
    
    public UsuarioControlador(IUsuarioServicio servicio)
    {
        _servicio = servicio;
    }
    
    
    [HttpGet]
    public IEnumerable<string> ObtenerUsuarios()
    {
        return new string[] { "Usuario1", "Usuario2" };
    }
    
    
    [HttpPost("RegistrarUsuario")]
    public IActionResult Registrar(RegistrarUsuarioDTO usuariojson)
    {
        try
        {
            var nuevoUsuario = usuariojson.Mapear();
            
            _servicio.RegistrarUsuario(nuevoUsuario);
            
            return Ok(new { mensaje = "Huésped registrado exitosamente." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
    
    
}