using System.Data;
using PortaAviones.Exceptions;
using PortaAviones.Properties;
using Microsoft.Data.SqlClient;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Util;

namespace PortaAviones.Datos.Repositorio
{
    public class RepositorioAterrizaje : IRepositorioAterrizaje
    {
        public static readonly string INSERT_ATERRIZAJE =
            "INSERT INTO " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "."
                + PropiedadesBD._TablaAterrizaje
                + " ( " + PropiedadesBD.Aterrizaje._ColumnaDespegueFk + ") VALUES(@despegueFk)";

        public static readonly string SELECT_POR_CODIGO =
            "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "."
            + PropiedadesBD._TablaAterrizaje + " WHERE " + PropiedadesBD.Aterrizaje._ColumnaDespegueFk + " = @despegueFk";

        public Aterrizaje BuscarPorDespegueId(int despegueId, SqlConnection sqlConnection)
        {
            if (despegueId > 0)
            {
                SqlCommand select = new(SELECT_POR_CODIGO, sqlConnection);
                select.Parameters.Add("@despegueFk", SqlDbType.Int).Value = despegueId;

                SqlDataReader sqlDataReader = select.ExecuteReader();
                Aterrizaje aterrizaje = new();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    aterrizaje = LeerRegistro(sqlDataReader);
                }

                sqlDataReader.Close();
                return aterrizaje;
            }
            else
            {
                throw new ArgumentException("El Despegue ID dado es invalido");
            }
        }

        public void Guardar(Aterrizaje aterrizaje, SqlConnection sqlConnection)
        {
            if (aterrizaje != null)
            {
                SqlCommand insert = new(INSERT_ATERRIZAJE, sqlConnection);
                insert.Parameters.Add("@despegueFk", SqlDbType.Int).Value = aterrizaje.DespegueFk;
                insert.ExecuteNonQuery();
            }
            else
            {
                throw new ArgumentException("El Aterrizaje provisto no es valido");
            }
        }

        private static Aterrizaje LeerRegistro(SqlDataReader sqlDataReader)
        {
            int id = (int)sqlDataReader[PropiedadesBD.Despegue._ColumnaId];
            int despegueFk = (int)sqlDataReader[PropiedadesBD.Aterrizaje._ColumnaDespegueFk];
            DateTime fechaRegistro = (DateTime)sqlDataReader[PropiedadesBD.Aterrizaje._ColumnaFechaRegistro];
            return new(id, despegueFk, fechaRegistro);
        }
    }
}