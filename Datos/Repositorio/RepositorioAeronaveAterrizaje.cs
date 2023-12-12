using System.Data;
using PortaAviones.Properties;
using Microsoft.Data.SqlClient;
using PortaAviones.Interfaces;
using PortaAviones.Models;

namespace PortaAviones.Datos.Repositorio
{
    public class RepositorioAeronaveAterrizaje : IRepositorioAeronaveAterrizaje
    {
        public static readonly string INSERT_AERONAVE_ATERRIZAJE =
                "INSERT INTO " + PropiedadesBD._BaseDeDatos + "." + PropiedadesBD._Esquema + "."
                + PropiedadesBD._TablaAeornavesAterrizaje
                + "(" + PropiedadesBD.AeronavesAterrizaje._ColumnAterrizajeFk
                + ", " + PropiedadesBD.AeronavesAterrizaje._ColumnaAeronaveFk
                + ", " + PropiedadesBD.AeronavesAterrizaje._ColumnaFechaAterrizaje
                + ", " + PropiedadesBD.AeronavesAterrizaje._ColumnaPerdidaMaterial
                + ", " + PropiedadesBD.AeronavesAterrizaje._ColumnaPerdidaHumana
                + ") VALUES(@aterrizajeFk, @aeronaveFk, @fechaAterrizaje, @perdidaMaterial, @perdidaHumana)";

        public void Guardar(AeronaveAterrizaje aeronavesAterrizaje, SqlConnection sqlConnection)
        {
            if (aeronavesAterrizaje != null)
            {
                SqlCommand insert = new(INSERT_AERONAVE_ATERRIZAJE, sqlConnection);
                insert.Parameters.Add("@aterrizajeFk", SqlDbType.Int).Value = aeronavesAterrizaje.AterrizajeFk;
                insert.Parameters.Add("@aeronaveFk", SqlDbType.Int).Value = aeronavesAterrizaje.AeronaveFk;
                insert.Parameters.Add("@fechaAterrizaje", SqlDbType.DateTime2).Value = aeronavesAterrizaje.FechaAterrizaje;
                insert.Parameters.Add("@perdidaMaterial", SqlDbType.Bit).Value = aeronavesAterrizaje.PerdidaMaterial;
                insert.Parameters.Add("@perdidaHumana", SqlDbType.Int).Value = aeronavesAterrizaje.PerdidaHumana;
                insert.ExecuteNonQuery();
            }
            else
            {
                throw new ArgumentException("El AeronavesAterrizaje provisto no es valido");
            }
        }
    }
}