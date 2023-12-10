using PortaAviones.Models;

namespace PortaAviones.Interfaces;

public interface IServicioModelo
{
    List<Modelo> BuscarPorMarcaId(int marcaId);
}
