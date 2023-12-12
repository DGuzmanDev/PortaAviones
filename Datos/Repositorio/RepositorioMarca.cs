using System.Data;
using PortaAviones.Properties;
using Microsoft.Data.SqlClient;
using PortaAviones.Interfaces;
using PortaAviones.Models;

namespace PortaAviones.Datos.Repositorio
{
    public class RepositorioMarca : IRepositorioMarca
    {

        private static readonly string SELECT_MARCAS = "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "."
                    + PropiedadesBD._Esquema + "."
                    + PropiedadesBD._TablaMarca;

        private static readonly string SELECT_MARCA_POR_ID = "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "."
                    + PropiedadesBD._Esquema + "."
                    + PropiedadesBD._TablaMarca
                    + " WHERE " + PropiedadesBD.Marca._ColumnaId + " = @id";

        public Marca BuscarPorId(int id, SqlConnection sqlConnection)
        {
            if (id > 0)
            {
                SqlCommand select = new(SELECT_MARCA_POR_ID, sqlConnection);
                select.Parameters.Add("@id", SqlDbType.Int).Value = id;

                SqlDataReader sqlDataReader = select.ExecuteReader();
                Marca marca = new();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    marca = LeerRegistro(sqlDataReader);
                }

                sqlDataReader.Close();
                return marca;
            }
            else
            {
                throw new ArgumentException("El ID dado es invalida");
            }
        }

        public List<Marca> BuscarTodos(SqlConnection sqlConnection)
        {
            SqlCommand select = new(SELECT_MARCAS, sqlConnection);

            SqlDataReader sqlDataReader = select.ExecuteReader();
            List<Marca> marcas = new();
            while (sqlDataReader.Read())
            {
                marcas.Add(LeerRegistro(sqlDataReader));
            }

            sqlDataReader.Close();
            return marcas;

        }

        private static Marca LeerRegistro(SqlDataReader sqlDataReader)
        {
            int id = (int)sqlDataReader[PropiedadesBD.Marca._ColumnaId];
            string nombre = (string)sqlDataReader[PropiedadesBD.Marca._ColumnaNombre];
            return new(id, nombre);
        }


    }
}