namespace PortaAviones.Models
{
    public class DespegueAeronave
    {
        public int? Id { get; set; }
        public int? DespegueFk { get; set; }
        public int? AeronaveFk { get; set; }
        public string? Piloto { get; set; }

        public Aeronave? Aeronave { get; set; }

        public DespegueAeronave() { }

        public DespegueAeronave(int? id, int? despegue_fk, int? aeronave_fk, string? piloto)
        {
            Id = id;
            DespegueFk = despegue_fk;
            AeronaveFk = aeronave_fk;
            Piloto = piloto;
        }
    }
}