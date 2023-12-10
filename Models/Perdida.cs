namespace PortaAviones.Models
{
    public class Perdida
    {
        public int? PerdidaHumana { get; set; }
        public Boolean? PerdidaMaterial { get; set; }

        public Perdida(Boolean? perdidaMaterial, int? perdidaHumana)
        {
            PerdidaMaterial = perdidaMaterial;
            PerdidaHumana = perdidaHumana;
        }
    }
}