namespace PortaAviones.Models
{
    public class Retiro
    {
        public string? Tecnico { get; set; }
        public List<Aeronave>? Aeronaves{get; set;}

        public Retiro(string? tecnico, List<Aeronave>? aeronaves)
        {
            Tecnico = tecnico;
            Aeronaves = aeronaves;
        }
    }
}