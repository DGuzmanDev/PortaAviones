namespace PortaAviones.Models
{
    public class Aeronave
    {
        public int? Id { get; set; }
        public string? Serie { get; set; }
        public Marca? Marca { get; set; }
        public Modelo? Modelo { get; set; }
        public string? Nombre { get; set; }
        public double? Ancho { get; set; }
        public double? Alto { get; set; }
        public double? Largo { get; set; }
        public bool? Retirado { get; set; }
        public string? TecnicoIngreso { get; set; }
        public string? TecnicoRetiro { get; set; }
        public string? RazonRetiro { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public Aeronave() { }

        public Aeronave(int? id, string? serie, string? nombre, Marca? marca, Modelo? modelo)
        {
            Id = id;
            Serie = serie;
            Nombre = nombre;
            Marca = marca;
            Modelo = modelo;
        }

        public Aeronave(int? id, string? serie, Marca? marca, Modelo? modelo, string? nombre, double? ancho,
            double? alto, double? largo, bool? retirado, string? tecnicoIngreso, string? tecnicoRetiro,
            string? razonRetiro, DateTime? fechaRegistro, DateTime? fechaActualizacion)
        {
            Id = id;
            Serie = serie;
            Marca = marca;
            Modelo = modelo;
            Nombre = nombre;
            Ancho = ancho;
            Alto = alto;
            Largo = largo;
            Retirado = retirado;
            TecnicoIngreso = tecnicoIngreso;
            TecnicoRetiro = tecnicoRetiro;
            RazonRetiro = razonRetiro;
            FechaRegistro = fechaRegistro;
            FechaActualizacion = fechaActualizacion;
        }
    }
}