namespace GestionHerramientas.Exceptions
{
    [Serializable]
    public class DataBaseError : Exception
    {
        public DataBaseError() : base() { }

        public DataBaseError(string error) : base(error) { }

        public DataBaseError(string error, Exception causa) : base(error, causa) { }

        public class ErrorDeConsistenciaDeDatos : DataBaseError
        {

            public ErrorDeConsistenciaDeDatos() : base() { }

            public ErrorDeConsistenciaDeDatos(string error) : base(error) { }

            public ErrorDeConsistenciaDeDatos(string error, Exception causa) : base(error, causa) { }
        }

        public class ViolacionDeLlaveUnica : DataBaseError
        {

            public ViolacionDeLlaveUnica() : base() { }

            public ViolacionDeLlaveUnica(string error) : base(error) { }

            public ViolacionDeLlaveUnica(string error, Exception causa) : base(error, causa) { }
        }

    }
}

