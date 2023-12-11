using System.Transactions;
using Microsoft.Data.SqlClient;
using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IRepositorioDespegueAeronave
    {
        void Guardar(DespegueAeronave despegueAeronave, SqlConnection sqlConnection, TransactionScope txScope, bool txCommit);

        List<DespegueAeronave> BuscarPorDespegueId(int despegueId, SqlConnection sqlConnection);
    }
}