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

        private static readonly string SELECT_AERONAVE_POR_RETIRO = "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "."
            + PropiedadesBD._Esquema + "."
            + PropiedadesBD._TablaAeronave
            + " WHERE " + PropiedadesBD.Aeronave._ColumnaRetirado + " = @retirado";

        private static readonly string SELECT_AERONAVES_CUENTA_POR_MODELO = "SELECT modelo.nombre AS " + ModeloAeronaveAgrupado._AgrupadoModelo
                    + ", marca.nombre AS " + ModeloAeronaveAgrupado._AgrupadoMarca + "," +
                    " COUNT(aeronave.id) AS " + ModeloAeronaveAgrupado._AgrupadoCuenta + " FROM " + PropiedadesBD._BaseDeDatos + "."
                    + PropiedadesBD._Esquema + "." + PropiedadesBD._TablaAeronave
                    + " JOIN " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "." + PropiedadesBD._TablaMarca
                    + " marca ON marca." + PropiedadesBD.Marca._ColumnaId + " = aeronave." + PropiedadesBD.Aeronave._ColumnaMarcaFk
                    + " JOIN " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "." + PropiedadesBD._TablaModelo
                    + " modelo ON modelo." + PropiedadesBD.Modelo._ColumnaId + " = aeronave." + PropiedadesBD.Aeronave._ColumnaModeloFk
                    + " WHERE aeronave." + PropiedadesBD.Aeronave._ColumnaRetirado + " = @retirado"
                    + " GROUP BY aeronave." + PropiedadesBD.Aeronave._ColumnaModeloFk
                    + ", marca." + PropiedadesBD.Marca._ColumnaNombre
                    + ", modelo." + PropiedadesBD.Modelo._ColumnaNombre;

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

        public List<ModeloAeronaveAgrupado> ContarModelos(SqlConnection sqlConnection, bool retirado)
        {

            SqlCommand select = new(SELECT_AERONAVES_CUENTA_POR_MODELO, sqlConnection);
            select.Parameters.Add("@retirado", SqlDbType.Bit).Value = retirado;

            SqlDataReader sqlDataReader = select.ExecuteReader();
            List<ModeloAeronaveAgrupado> agrupados = new();
            while (sqlDataReader.Read())
            {
                agrupados.Add(LeerRegistroModelo(sqlDataReader));
            }

            sqlDataReader.Close();
            return agrupados;
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

        public List<Aeronave> BuscarTodosPorRetiro(bool retirado, SqlConnection sqlConnection)
        {

            SqlCommand select = new(SELECT_AERONAVE_POR_RETIRO, sqlConnection);
            select.Parameters.Add("@retirado", SqlDbType.Bit).Value = retirado;

            SqlDataReader sqlDataReader = select.ExecuteReader();
            List<Aeronave> aeronaves = new();
            while (sqlDataReader.Read())
            {
                aeronaves.Add(LeerRegistro(sqlDataReader));
            }

            sqlDataReader.Close();
            return aeronaves;
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

        private static ModeloAeronaveAgrupado LeerRegistroModelo(SqlDataReader sqlDataReader)
        {
            string modelo = (string)sqlDataReader[ModeloAeronaveAgrupado._AgrupadoModelo];
            string marca = (string)sqlDataReader[ModeloAeronaveAgrupado._AgrupadoMarca];
            int cuenta = (int)sqlDataReader[ModeloAeronaveAgrupado._AgrupadoCuenta];
            return new(cuenta, new Marca(null, marca), new Modelo(null, modelo, null));
        }
    }
}