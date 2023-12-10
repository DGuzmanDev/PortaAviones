using System.Transactions;
using Microsoft.Data.SqlClient;
using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IRepositorioAeronave
    {
        Aeronave BuscarPorSerie(string serie, SqlConnection sqlConnection);

        void GuardarTodos(List<Aeronave> aeronaves, SqlConnection sqlConnection, TransactionScope txScope);
    }
}