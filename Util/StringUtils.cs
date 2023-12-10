namespace PortaAviones.Util
{
    public class StringUtils
    {
        /**
		 * Valida si el String dado es null, esta vacio o solo se compone de espacio en blanco
		 * 
		 * Retorna True si y solo si el argumento str cumple con alguno de los criterios de evaluacion
		 */
        public static bool IsEmpty(string? str)
        {
            return string.IsNullOrEmpty(str)
                     || string.IsNullOrWhiteSpace(str);
        }
    }
}

