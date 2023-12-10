namespace PortaAviones.Models
{
    public class ModeloAeronaveAgrupado
    {
        public static readonly string _AgrupadoModelo = "modelo";
        public static readonly string _AgrupadoMarca = "marca";
        public static readonly string _AgrupadoCuenta = "cuenta";
    

        public int? Cuenta { get; set; }
        public Marca? Marca { get; set; }
        public Modelo? Modelo { get; set; }

        public ModeloAeronaveAgrupado(int? cuenta, Marca? marca, Modelo? modelo)
        {
            Cuenta = cuenta;
            Marca = marca;
            Modelo = modelo;
        }
    }
}