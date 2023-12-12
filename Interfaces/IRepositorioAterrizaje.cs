using Microsoft.Data.SqlClient;
using PortaAviones.Models;

namespace PortaAviones.Interfaces;

public interface IRepositorioAterrizaje
{
    void Guardar(Aterrizaje aterrizaje, SqlConnection sqlConnection);

    Aterrizaje BuscarPorDespegueId(int despegueId, SqlConnection sqlConnection);
}
