

using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IConectorDeDatos
    {
        List<Aeronave> RegistrarNuevoIngreso(Ingreso ingreso);

        List<Aeronave> RegistrarRetiro(Retiro retiro);

        List<Marca> BuscarMarcas();

        List<Modelo> BuscarModelosPorMarcaId(int marcaId);

        List<Aeronave> BuscarAeronavesActivas();

        List<ModeloAeronaveAgrupado> ContarAeronavesAgrupadasPorModelo();

        Aeronave BuscarAeronaveActivaPorSerie(string serie);
    }
}