using System.Transactions;
using Microsoft.Data.SqlClient;
using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IRepositorioAeronaveDespegue
    {
        void Guardar(AeronaveDespegue despegueAeronave, SqlConnection sqlConnection, TransactionScope txScope, bool txCommit);

        List<AeronaveDespegue> BuscarPorDespegueId(int despegueId, SqlConnection sqlConnection);
    }
}