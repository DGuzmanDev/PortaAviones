using GestionHerramientas.Properties;
using Microsoft.Data.SqlClient;

namespace GestionHerramientas.Datos
{
    public class ConexionSQLServer
    {

        private static readonly string _connStr = PropiedadesBD.ObtenerStringDeConexion();

        /// <summary>
        /// Crea una nueva instancia <see cref="SqlConnection"/> configurada con los parametros de conexion
        /// esperados para la base de datos del sistema
        /// </summary>
        /// <returns></returns>
        public static SqlConnection ObenerConexion()
        {
            return new SqlConnection(_connStr);
        }
    }
}

