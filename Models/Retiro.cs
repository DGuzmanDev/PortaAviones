namespace PortaAviones.Models
{
    public class Retiro
    {
        public int? Id { get; set; }
        public string? Tecnico { get; set; }
        public string? Detalle { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public Retiro(int? id, string? tecnico, string? detalle, DateTime? fechaRegistro)
        {
            Id = id;
            Tecnico = tecnico;
            Detalle = detalle;
            FechaRegistro = fechaRegistro;
        }
    }
}