using System.Transactions;
using Microsoft.Data.SqlClient;
using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IRepositorioAeronave
    {
        Aeronave BuscarActivaPorSerie(string serie, SqlConnection sqlConnection);

        List<ModeloAeronaveAgrupado> ContarModelos(SqlConnection sqlConnection, bool retirado);

        List<Aeronave> BuscarTodosPorRetiro(bool retirado, SqlConnection sqlConnection);

        void GuardarTodos(List<Aeronave> aeronaves, SqlConnection sqlConnection, TransactionScope txScope);

        void ActualizarTodos(List<Aeronave> aeronaves, SqlConnection sqlConnection, TransactionScope txScope);
    }
}