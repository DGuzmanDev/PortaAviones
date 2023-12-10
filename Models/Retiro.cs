namespace PortaAviones.Models
{
    public class Retiro
    {
        public string? Tecnico { get; set; }
        public string? Razon { get; set; }
        public List<Aeronave>? Aeronaves { get; set; }

        public Retiro(string? tecnico, string? razon, List<Aeronave>? aeronaves)
        {
            Tecnico = tecnico;
            Razon = razon;
            Aeronaves = aeronaves;
        }
    }
}