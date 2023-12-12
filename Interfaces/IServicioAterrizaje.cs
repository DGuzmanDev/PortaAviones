using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IServicioAterrizaje
    {
        Aterrizaje Guardar(Aterrizaje aterrizaje);
    }
}