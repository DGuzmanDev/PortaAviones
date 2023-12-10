namespace PortaAviones.Util
{
    public class DBUtil
    {
        /// <summary>
        /// Metodo auxiliar para convertir las columnas DBNull.Value del SqlDataReader a un tipo de dato default.
        /// Este metodo se utiliza solamente para resolver los valores de columnas que permiten valores NULL
        /// </summary>
        /// <returns>
        /// El valor por defecto del tipo T dado o el valor proviniente de la base de datos si esta presente
        /// </returns>
        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // retorna el valor por defecto del tipo de dato del modelo
            }
            else
            {
                return (T)obj;
            }
        }
    }
}