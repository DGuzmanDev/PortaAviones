

using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IConectorDeDatos
    {
        List<Aeronave> RegistrarNuevoIngreso(Ingreso ingreso);

        List<Marca> BuscarMarcas();

        List<Modelo> BuscarModelosPorMarcaId(int marcaId);

        List<Aeronave> BuscarAeronavesActivas();

        List<ModeloAeronaveAgrupado> ContarAeronavesAgrupadasPorModelo();
    }
}