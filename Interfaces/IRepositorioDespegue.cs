using System.Transactions;
using Microsoft.Data.SqlClient;
using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IRepositorioDespegue
    {
        void Guardar(Despegue despegue, SqlConnection sqlConnection, TransactionScope txScope, bool txCommit);

        int ObtenerSiguienteIdentificador(SqlConnection sqlConnection);

        Despegue BuscarPorCodigo(string codigo, SqlConnection sqlConnection);
    }
}