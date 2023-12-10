using System.Data;
using GestionHerramientas.Properties;
using Microsoft.Data.SqlClient;
using PortaAviones.Interfaces;
using PortaAviones.Models;

namespace PortaAviones.Datos.Repositorio
{
    public class RepositorioModelo : IRepositorioModelo
    {
        private static readonly string SELECT_MODELO_POR_ID = "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "."
                    + PropiedadesBD._Esquema + "."
                      + PropiedadesBD._TablaModelo
                      + " WHERE " + PropiedadesBD.Modelo._ColumnaId + " = @id";

        private static readonly string SELECT_MODELO_POR_MARCA = "SELECT * FROM " + PropiedadesBD._BaseDeDatos + "."
                    + PropiedadesBD._Esquema + "."
                    + PropiedadesBD._TablaModelo
                    + " WHERE " + PropiedadesBD.Modelo._ColumnaMarcaFk + " = @id";

        public Modelo BuscarPorId(int id, SqlConnection sqlConnection)
        {
            if (id > 0)
            {
                SqlCommand select = new(SELECT_MODELO_POR_ID, sqlConnection);
                select.Parameters.Add("@id", SqlDbType.Int).Value = id;

                SqlDataReader sqlDataReader = select.ExecuteReader();
                Modelo modelo = new();
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    modelo = LeerRegistro(sqlDataReader);
                }

                sqlDataReader.Close();
                return modelo;
            }
            else
            {
                throw new ArgumentException("El ID dado es invalida");
            }
        }

        public List<Modelo> BuscarPorMarca(int marcaId, SqlConnection sqlConnection)
        {
            if (marcaId > 0)
            {
                SqlCommand select = new(SELECT_MODELO_POR_MARCA, sqlConnection);
                select.Parameters.Add("@id", SqlDbType.Int).Value = marcaId;

                SqlDataReader sqlDataReader = select.ExecuteReader();
                List<Modelo> modelo = new();
                while (sqlDataReader.Read())
                {
                    modelo.Add(LeerRegistro(sqlDataReader));
                }

                sqlDataReader.Close();
                return modelo;
            }
            else
            {
                throw new ArgumentException("La marca dada es invalida");
            }
        }

        private static Modelo LeerRegistro(SqlDataReader sqlDataReader)
        {
            int id = (int)sqlDataReader[PropiedadesBD.Modelo._ColumnaId];
            int marca_fk = (int)sqlDataReader[PropiedadesBD.Modelo._ColumnaMarcaFk];
            string nombre = (string)sqlDataReader[PropiedadesBD.Aeronave._ColumnaNombre];
            return new(id, nombre, marca_fk);
        }
    }
}