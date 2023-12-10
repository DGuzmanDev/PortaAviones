using System.Data;
using System.Transactions;
using GestionHerramientas.Exceptions;
using GestionHerramientas.Properties;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Util;

namespace PortaAviones.Datos.Repositorio
{
    public class RepositorioAeronave : IRepositorioAeronave
    {

        private static readonly string SELECT_AERONAVE_POR_SERIE = "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "."
                    + PropiedadesBD._Esquema + "."
                    + PropiedadesBD._TablaAeronave
                    + " WHERE " + PropiedadesBD.Aeronave._ColumnaSerie + " = @serie";

        private static readonly string INSERT_AERONAVE = "INSERT INTO " + PropiedadesBD._BaseDeDatos + "."
                    + PropiedadesBD._Esquema + "."
                    + PropiedadesBD._TablaAeronave + "("
                    + PropiedadesBD.Aeronave._ColumnaSerie + ", "
                    + PropiedadesBD.Aeronave._ColumnaNombre + ", "
                    + PropiedadesBD.Aeronave._ColumnaAlto + ", "
                    + PropiedadesBD.Aeronave._ColumnaAncho + ", "
                    + PropiedadesBD.Aeronave._ColumnaLargo + ", "
                    + PropiedadesBD.Aeronave._ColumnaMarcaFk + ", "
                    + PropiedadesBD.Aeronave._ColumnaModeloFk + ", "
                    + PropiedadesBD.Aeronave._ColumnaTecnicoIngreso
                    + ") VALUES (@serie, @nombre, @alto, @ancho, @largo, @marca, @modelo, @tecnico)";

        public Aeronave BuscarPorSerie(string serie, SqlConnection sqlConnection)
        {
            if (!StringUtils.IsEmpty(serie))
            {
                SqlCommand select = new(SELECT_AERONAVE_POR_SERIE, sqlConnection);
                select.Parameters.Add("@serie", SqlDbType.VarChar).Value = serie;

                SqlDataReader sqlDataReader = select.ExecuteReader();
                Aeronave aeronave = new();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    aeronave = LeerRegistro(sqlDataReader);
                }

                sqlDataReader.Close();
                return aeronave;
            }
            else
            {
                throw new ArgumentException("La series dada es invalida");
            }
        }

        public void GuardarTodos(List<Aeronave> aeronaves, SqlConnection connection, TransactionScope tx)
        {
            if (!aeronaves.IsNullOrEmpty())
            {
                int registros = 0;
                aeronaves.ForEach(aeronave =>
                {
                    EjecutarInsert(aeronave, connection, null);
                    registros++;
                });

                if (registros == aeronaves.Count)
                {
                    tx.Complete();//Commit de todos los INSERT
                }
                else
                {
                    throw new DataBaseError.ErrorDeConsistenciaDeDatos("No se pudieron guardar todas las Aeronaves");
                }
            }
            else
            {
                throw new ArgumentException("La lista de Aeronaves es invalida");
            }
        }


        private static int EjecutarInsert(Aeronave aeronave, SqlConnection sqlConnection, TransactionScope? tx)
        {
            SqlCommand insert = new(INSERT_AERONAVE, sqlConnection);
            insert.Parameters.Add("@serie", SqlDbType.VarChar).Value = aeronave.Serie;
            insert.Parameters.Add("@nombre", SqlDbType.VarChar).Value = aeronave.Nombre;
            insert.Parameters.Add("@alto", SqlDbType.Decimal).Value = aeronave.Alto;
            insert.Parameters.Add("@ancho", SqlDbType.Decimal).Value = aeronave.Ancho;
            insert.Parameters.Add("@largo", SqlDbType.Decimal).Value = aeronave.Largo;
            insert.Parameters.Add("@marca", SqlDbType.Int).Value = aeronave.Marca.Id;
            insert.Parameters.Add("@modelo", SqlDbType.Int).Value = aeronave.Modelo.Id;
            insert.Parameters.Add("@tecnico", SqlDbType.VarChar).Value = aeronave.TecnicoIngreso;

            int rowsAffected = insert.ExecuteNonQuery();

            if (tx != null)
            {
                if (rowsAffected == 1)
                {
                    tx.Complete();//Commit INSERT
                }
                else
                {
                    throw new DataBaseError.ErrorDeConsistenciaDeDatos("No se pudo guardar la Aeronave");
                }
            }

            return rowsAffected;
        }

        private static Aeronave LeerRegistro(SqlDataReader sqlDataReader)
        {
            int id = (int)sqlDataReader[PropiedadesBD.Aeronave._ColumnaId];
            string serie = (string)sqlDataReader[PropiedadesBD.Aeronave._ColumnaSerie];
            string nombre = (string)sqlDataReader[PropiedadesBD.Aeronave._ColumnaNombre];
            decimal alto = (decimal)sqlDataReader[PropiedadesBD.Aeronave._ColumnaAlto];
            decimal ancho = (decimal)sqlDataReader[PropiedadesBD.Aeronave._ColumnaAncho];
            decimal largo = (decimal)sqlDataReader[PropiedadesBD.Aeronave._ColumnaLargo];
            int marca = (int)sqlDataReader[PropiedadesBD.Aeronave._ColumnaMarcaFk];
            int modelo = (int)sqlDataReader[PropiedadesBD.Aeronave._ColumnaModeloFk];
            string tecnicoIngreso = (string)sqlDataReader[PropiedadesBD.Aeronave._ColumnaTecnicoIngreso];
            string? tecnicoRetiro = DBUtil.ConvertFromDBVal<string>(sqlDataReader[PropiedadesBD.Aeronave._ColumnaTecnicoRetiro]);
            bool? retirado = DBUtil.ConvertFromDBVal<bool>(sqlDataReader[PropiedadesBD.Aeronave._ColumnaRetirado]);
            bool? perdidaMaterial = DBUtil.ConvertFromDBVal<bool>(sqlDataReader[PropiedadesBD.Aeronave._ColumnaPerdidaMaterial]);
            int? perdidaHumana = DBUtil.ConvertFromDBVal<int>(sqlDataReader[PropiedadesBD.Aeronave._ColumnaPerdidaHumana]);
            DateTime fechaRegistro = (DateTime)sqlDataReader[PropiedadesBD.Aeronave._ColumnaFechaRegistro];
            DateTime fechaActualizacion = (DateTime)sqlDataReader[PropiedadesBD.Aeronave._ColumnaFechaActualizacion];
            return new(id, serie, new Marca(marca, null), new Modelo(modelo, null, marca), nombre, decimal.ToDouble(ancho), decimal.ToDouble(alto),
                decimal.ToDouble(largo), retirado, perdidaMaterial, perdidaHumana, tecnicoIngreso, tecnicoRetiro, fechaRegistro, fechaActualizacion);
        }

    }
}