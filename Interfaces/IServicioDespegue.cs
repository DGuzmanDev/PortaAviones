using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IServicioDespegue
    {
        Despegue Guardar(Despegue despegue);

        Despegue BuscarPorCodigo(string codigo);

        List<Despegue> BuscarTodos();
    }
}