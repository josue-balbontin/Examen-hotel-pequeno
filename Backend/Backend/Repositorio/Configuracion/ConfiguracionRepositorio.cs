using Backend.Modelos.Entidades;

namespace Backend.Repositorio.Configuracion;

public class ConfiguracionRepositorio : IConfiguracionRepositorio
{
    private readonly HotelDbContext _contexto;

    public ConfiguracionRepositorio(HotelDbContext contexto)
    {
        _contexto = contexto;
    }

    public string ObtenerValor(string nombreConfiguracion, string valorPorDefecto)
    {
        var valor = _contexto.Configuraciones
            .Where(c => c.NombreConfiguracion == nombreConfiguracion)
            .Select(c => c.ValorConfiguracion)
            .FirstOrDefault();

        return string.IsNullOrWhiteSpace(valor) ? valorPorDefecto : valor;
    }
}

