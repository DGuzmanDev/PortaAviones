using Microsoft.Data.SqlClient;
using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IRepositorioMarca
    {
        List<Marca> BuscarTodos(SqlConnection sqlConnection);

        Marca BuscarPorId(int id, SqlConnection sqlConnection);
    }
}