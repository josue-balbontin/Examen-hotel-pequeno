namespace Backend.Modelos.DTOs;

public class RegistrarUsuarioDTO
{
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string DocumentoIdentidad { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public int? Edad { get; set; }


    public Usuario Mapear()
    {
        return new Usuario
        {
            Nombres = this.Nombres,
            Apellidos = this.Apellidos,
            DocumentoIdentidad = this.DocumentoIdentidad,
            Telefono = this.Telefono,
            Email = this.Email,
            Edad = this.Edad
        };
        
    }
    
}