namespace PortaAviones.Models
{
    public class AeronaveAterrizaje
    {
        public int? Id { get; set; }
        public int? AeronaveFk { get; set; }
        public int? AterrizajeFk { get; set; }
        public bool? PerdidaMaterial { get; set; }
        public int? PerdidaHumana { get; set; }
        public DateTime? FechaAterrizaje { get; set; }
        public Aeronave? Aeronave { get; set; }

        public AeronaveAterrizaje() { }

        public AeronaveAterrizaje(int? id, int? aeronaveFk, int? aterrizajeFk, bool? perdidaMaterial,
            int? perdidaHumana, DateTime? fechaAterrizaje, Aeronave? aeronave)
        {
            Id = id;
            AeronaveFk = aeronaveFk;
            AterrizajeFk = aterrizajeFk;
            PerdidaMaterial = perdidaMaterial;
            PerdidaHumana = perdidaHumana;
            FechaAterrizaje = fechaAterrizaje;
            Aeronave = aeronave;
        }
    }
}