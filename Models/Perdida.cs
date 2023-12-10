namespace PortaAviones.Models
{
    public class Perdida
    {
        public int? Id { get; set; }
        public int? PerdidaHumana { get; set; }
        public Boolean? PerdidaMaterial { get; set; }

        public Perdida(int? id, Boolean? perdidaMaterial, int? perdidaHumana)
        {
            Id = id;
            PerdidaMaterial = perdidaMaterial;
            PerdidaHumana = perdidaHumana;
        }
    }
}