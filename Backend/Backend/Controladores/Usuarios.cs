using Microsoft.AspNetCore.Mvc;

namespace Backend.Controladores;


[ApiController]
[Route("[controller]")]
public class Usuarios : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "Usuario1", "Usuario2" };
    }
    
    
}