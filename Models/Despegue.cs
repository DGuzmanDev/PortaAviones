namespace PortaAviones.Models
{
    public class Despegue
    {
        public int? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Tecnico { get; set; }
        public string? Mision { get; set; }
        public DateTime? FechaDespegue { get; set; }
        public List<DespegueAeronave>? Aeronaves { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public Despegue() { }

        public Despegue(int? id, string? codigo, string? tecnico, string? mision, DateTime? fechaDespegue,
         DateTime? fechaRegistro, List<DespegueAeronave>? aeronaves)
        {
            Id = id;
            Codigo = codigo;
            Tecnico = tecnico;
            Mision = mision;
            FechaDespegue = fechaDespegue;
            FechaRegistro = fechaRegistro;
            Aeronaves = aeronaves;
        }
    }
}