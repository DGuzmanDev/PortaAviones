
namespace GestionHerramientas.Properties
{
    public class PropiedadesBD
    {
        //Propiedades generales
        public static readonly string _Servidor = "127.0.0.1";
        public static readonly string _Puerto = "1433";
        public static readonly string _BaseDeDatos = "PortaAviones";
        public static readonly string _Esquema = "PortaAviones";
        public static readonly string _UsuarioBaseDeDatos = "Dguzman";
        public static readonly string _PwdBaseDeDatos = "Admin@SQLServer03101";

        //Propiedades especificas
        public static readonly string _TablaAeronave = "aeronave";
        public static readonly string _TablaMarca = "marca";
        public static readonly string _TablaModelo = "modelo";
        public static readonly string _TablaDespegue = "despegue";
         public static readonly string _TablaAeornavesDespegue = "aeronaves_despegue";

        public static string ObtenerStringDeConexion()
        {
            return @"Server=" + _Servidor + "," + _Puerto + ";Database=" + _BaseDeDatos
                + ";User Id=" + _UsuarioBaseDeDatos + ";Password=" + _PwdBaseDeDatos + ";TrustServerCertificate=true";
        }

        public static class Aeronave
        {
            //Propiedades estaticas asocias con la Base de Datos
            public static readonly string _Nombre = "aeronave";
            public static readonly string _ColumnaId = "id";
            public static readonly string _ColumnaSerie = "serie";
            public static readonly string _ColumnaMarcaFk = "marca_fk";
            public static readonly string _ColumnaModeloFk = "modelo_fk";
            public static readonly string _ColumnaNombre = "nombre";
            public static readonly string _ColumnaAlto = "alto";
            public static readonly string _ColumnaAncho = "ancho";
            public static readonly string _ColumnaLargo = "largo";
            public static readonly string _ColumnaRetirado = "retirado";
            public static readonly string _ColumnaTecnicoIngreso = "tecnico_ingreso";
            public static readonly string _ColumnaTecnicoRetiro = "tecnico_retiro";
            public static readonly string _ColumnaRazonRetiro = "razon_retiro";
            public static readonly string _ColumnaPerdidaMaterial = "perdida_material";
            public static readonly string _ColumnaPerdidaHumana = "perdida_humana";
            public static readonly string _ColumnaFechaRegistro = "fecha_registro";
            public static readonly string _ColumnaFechaActualizacion = "fecha_actualizacion";
        }

        public static class Marca
        {
            public static readonly string _ColumnaId = "id";
            public static readonly string _ColumnaNombre = "nombre";
        }

        public static class Modelo
        {
            public static readonly string _ColumnaId = "id";
            public static readonly string _ColumnaNombre = "nombre";
            public static readonly string _ColumnaMarcaFk = "marca_fk";
        }

        public static class Despegue
        {
            public static readonly string _ColumnaId = "id";
            public static readonly string _ColumnaCodigo = "codigo";
            public static readonly string _ColumnaTecnico = "tecnico";
            public static readonly string _ColumnaMision = "mision";
            public static readonly string _ColumnaFechaDespegue = "fecha_despegue";
            public static readonly string _ColumnaFechaRegistro = "fecha_registro";
            public static readonly string _SecuenciaCodigo = "secuencia_despegues";
        }

        public static class AeronavesDespegue
        {
            public static readonly string _ColumnaId = "id";
            public static readonly string _ColumnaPiloto = "piloto";
            public static readonly string _ColumnaDespegueFk = "despegue_fk";
            public static readonly string _ColumnaAeronaveFk = "aeronave_fk";
        }
    }
}

