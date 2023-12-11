using System.Data;
using System.Transactions;
using GestionHerramientas.Exceptions;
using GestionHerramientas.Properties;
using Microsoft.Data.SqlClient;
using PortaAviones.Models;
using PortaAviones.Util;

namespace PortaAviones.Interfaces
{
    public class RepositorioDespegueAeronave : IRepositorioDespegueAeronave
    {

        public static readonly string INSERT_DESPEGUE_AERONAVE =
                "INSERT INTO " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "."
                + PropiedadesBD._TablaAeornavesDespegue
                + " (" + PropiedadesBD.AeronavesDespegue._ColumnaDespegueFk + ", "
                + PropiedadesBD.AeronavesDespegue._ColumnaAeronaveFk + ", "
                + PropiedadesBD.AeronavesDespegue._ColumnaPiloto + ")"
                + " VALUES (@despegueFk, @aeronaveFk, @piloto)";

        public static readonly string SELECT_POR_DESPEGUE_ID =
                "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "."
                + PropiedadesBD._TablaAeornavesDespegue
                + " WHERE " + PropiedadesBD.AeronavesDespegue._ColumnaDespegueFk + " = @despegueId";

        public List<DespegueAeronave> BuscarPorDespegueId(int despegueId, SqlConnection sqlConnection)
        {
            if (despegueId > 0)
            {
                SqlCommand select = new(SELECT_POR_DESPEGUE_ID, sqlConnection);
                select.Parameters.Add("@despegueId", SqlDbType.Int).Value = despegueId;

                SqlDataReader sqlDataReader = select.ExecuteReader();
                List<DespegueAeronave> despegueAeronave = new();
                while (sqlDataReader.Read())
                {
                    despegueAeronave.Add(LeerRegistro(sqlDataReader));
                }

                sqlDataReader.Close();
                return despegueAeronave;
            }
            else
            {
                throw new ArgumentException("El Despegue ID dado es invalido");
            }
        }

        public void Guardar(DespegueAeronave despegueAeronave, SqlConnection sqlConnection,
            TransactionScope txScope, bool txCommit)
        {
            if (despegueAeronave != null)
            {
                SqlCommand insert = new(INSERT_DESPEGUE_AERONAVE, sqlConnection);
                insert.Parameters.Add("@despegueFk", SqlDbType.Int).Value = despegueAeronave.DespegueFk;
                insert.Parameters.Add("@aeronaveFk", SqlDbType.Int).Value = despegueAeronave.AeronaveFk;
                insert.Parameters.Add("@piloto", SqlDbType.VarChar).Value = despegueAeronave.Piloto;

                if (insert.ExecuteNonQuery() == 1)
                {
                    if (txCommit)
                    {
                        txScope.Complete();//Commit INSERT
                    }
                }
                else
                {
                    throw new DataBaseError.ErrorDeConsistenciaDeDatos("No se pudo guardar el DespegueAeronave");
                }
            }
            else
            {
                throw new ArgumentException("El DespegueAeronave provisto no es valido");
            }
        }

        private static DespegueAeronave LeerRegistro(SqlDataReader sqlDataReader)
        {
            int? id = (int)sqlDataReader[PropiedadesBD.AeronavesDespegue._ColumnaId];
            int? aeronaveFk = (int)sqlDataReader[PropiedadesBD.AeronavesDespegue._ColumnaAeronaveFk];
            int? despegueFk = (int)sqlDataReader[PropiedadesBD.AeronavesDespegue._ColumnaDespegueFk];
            string? piloto = (string)sqlDataReader[PropiedadesBD.AeronavesDespegue._ColumnaPiloto];
            return new(id, despegueFk, aeronaveFk, piloto);
        }
    }
}