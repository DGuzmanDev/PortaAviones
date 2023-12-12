using System.Data;
using System.Transactions;
using PortaAviones.Exceptions;
using PortaAviones.Properties;
using Microsoft.Data.SqlClient;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Util;

namespace PortaAviones.Datos.Repositorio
{
    public class RepositorioDespegue : IRepositorioDespegue
    {
        public static readonly string SELECT_SIGUIENTE_SECUENCIA =
                "SELECT NEXT VALUE FOR PortaAviones." + PropiedadesBD.Despegue._SecuenciaCodigo;

        public static readonly string SELECT_TODOS =
                "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "."
                + PropiedadesBD._TablaDespegue;

        public static readonly string SELECT_POR_CODIGO =
                "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "."
                + PropiedadesBD._TablaDespegue + " WHERE " + PropiedadesBD.Despegue._ColumnaCodigo + " = @codigo";

        public static readonly string INSERT_DESPEGUE =
                "INSERT INTO " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "."
                + PropiedadesBD._TablaDespegue
                + " (" + PropiedadesBD.Despegue._ColumnaCodigo + ", "
                + PropiedadesBD.Despegue._ColumnaTecnico + ", "
                + PropiedadesBD.Despegue._ColumnaMision + ", "
                + PropiedadesBD.Despegue._ColumnaFechaDespegue + ")"
                + " VALUES(@codigo, @tecnico, @mision, @fechaDespegue)";

        public Despegue BuscarPorCodigo(string codigo, SqlConnection sqlConnection)
        {
            if (!StringUtils.IsEmpty(codigo))
            {
                SqlCommand select = new(SELECT_POR_CODIGO, sqlConnection);
                select.Parameters.Add("@codigo", SqlDbType.VarChar).Value = codigo;

                SqlDataReader sqlDataReader = select.ExecuteReader();
                Despegue despegue = new();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    despegue = LeerRegistro(sqlDataReader);
                }

                sqlDataReader.Close();
                return despegue;
            }
            else
            {
                throw new ArgumentException("El codigo dado es invalido");
            }
        }

        public void Guardar(Despegue despegue, SqlConnection sqlConnection, TransactionScope txScope, bool txCommit)
        {
            if (despegue != null)
            {
                SqlCommand insert = new(INSERT_DESPEGUE, sqlConnection);
                insert.Parameters.Add("@codigo", SqlDbType.VarChar).Value = despegue.Codigo;
                insert.Parameters.Add("@tecnico", SqlDbType.VarChar).Value = despegue.Tecnico;
                insert.Parameters.Add("@mision", SqlDbType.VarChar).Value = despegue.Mision;
                insert.Parameters.Add("@fechaDespegue", SqlDbType.DateTime2).Value = despegue.FechaDespegue;

                if (insert.ExecuteNonQuery() == 1)
                {
                    if (txCommit)
                    {
                        txScope.Complete();//Commit INSERT
                    }
                }
                else
                {
                    throw new DataBaseError.ErrorDeConsistenciaDeDatos("No se pudo guardar el Despegue");
                }
            }
            else
            {
                throw new ArgumentException("El Despegue provisto no es valido");
            }
        }

        public int ObtenerSiguienteIdentificador(SqlConnection sqlConnection)
        {

            SqlCommand select = new(SELECT_SIGUIENTE_SECUENCIA, sqlConnection);

            SqlDataReader sqlDataReader = select.ExecuteReader();
            try
            {
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    return sqlDataReader.GetInt32(0);
                }
                else
                {
                    throw new DataBaseError.ErrorDeConsistenciaDeDatos("No se ha podido obtener la secuencia para el registro del despegue");
                }
            }
            finally
            {
                sqlDataReader.Close();
            }
        }

        private static Despegue LeerRegistro(SqlDataReader sqlDataReader)
        {
            int id = (int)sqlDataReader[PropiedadesBD.Despegue._ColumnaId];
            string codigo = (string)sqlDataReader[PropiedadesBD.Despegue._ColumnaCodigo];
            string tecnico = (string)sqlDataReader[PropiedadesBD.Despegue._ColumnaTecnico];
            string mision = (string)sqlDataReader[PropiedadesBD.Despegue._ColumnaMision];
            DateTime fechaRegistro = (DateTime)sqlDataReader[PropiedadesBD.Despegue._ColumnaFechaRegistro];
            DateTime fechaDespegue = (DateTime)sqlDataReader[PropiedadesBD.Despegue._ColumnaFechaDespegue];
            return new(id, codigo, tecnico, mision, fechaDespegue, fechaRegistro, null);
        }

        public List<Despegue> BuscarTodos(SqlConnection sqlConnection)
        {
            SqlCommand select = new(SELECT_TODOS, sqlConnection);

            SqlDataReader sqlDataReader = select.ExecuteReader();
            List<Despegue> despegues = new();
            while (sqlDataReader.Read())
            {
                despegues.Add(LeerRegistro(sqlDataReader));
            }

            sqlDataReader.Close();
            return despegues;
        }
    }
}