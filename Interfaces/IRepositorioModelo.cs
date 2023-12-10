using Microsoft.Data.SqlClient;
using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IRepositorioModelo
    {
        Modelo BuscarPorId(int id, SqlConnection sqlConnection);

        List<Modelo> BuscarPorMarca(int marcaId, SqlConnection sqlConnection);
    }
}