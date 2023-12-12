using Microsoft.Data.SqlClient;
using PortaAviones.Models;

namespace PortaAviones.Interfaces
{
    public interface IRepositorioAeronaveAterrizaje
    {
        void Guardar(AeronaveAterrizaje aeronavesAterrizaje, SqlConnection sqlConnection);
    }
}