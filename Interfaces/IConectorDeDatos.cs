

using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IConectorDeDatos
    {
        List<Aeronave> RegistrarNuevoIngreso(Ingreso ingreso);

        List<Aeronave> RegistrarRetiro(Retiro retiro);

        List<Marca> BuscarMarcas();

        List<Modelo> BuscarModelosPorMarcaId(int marcaId);

        Aeronave BuscarAeronaveActivaPorSerie(string serie);
    }
}