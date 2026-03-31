namespace Backend.Repositorio.Configuracion;

public interface IConfiguracionRepositorio
{
    string ObtenerValor(string nombreConfiguracion, string valorPorDefecto);
}

