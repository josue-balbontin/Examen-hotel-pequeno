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
    
    
    [HttpGet("ObtenerUsuarios")]
    public IActionResult ObtenerUsuarios()
    {
        try
        {
            var usuarios = _servicio.ObtenerUsuarios();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Error interno: " + ex.Message });
        }
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