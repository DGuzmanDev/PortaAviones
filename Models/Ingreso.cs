using System.Numerics;

namespace PortaAviones.Models
{
    public class Ingreso
    {
        public string? Tecnico { get; set; }
        public List<Aeronave>? Aeronaves { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public Ingreso() { }

        public Ingreso(string? tecnico, List<Aeronave>? aeronaves)
        {
            Tecnico = tecnico;
            Aeronaves = aeronaves;
        }
    }
}