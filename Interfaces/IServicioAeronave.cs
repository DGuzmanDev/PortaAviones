using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IServicioAeronave
    {
        List<Aeronave> Ingresar(Ingreso ingreso);

        List<Aeronave> Retirar(Retiro retiro);

        Aeronave BuscarPorSerie(string serie);

        List<Aeronave> BuscarActivos();

        List<ModeloAeronaveAgrupado> ContarModelosActivos();
    }
}