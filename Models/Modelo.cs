namespace PortaAviones.Models
{
    public class Modelo
    {
        public int? Id { get; set; }
        public int? MarcaId { get; set; }
        public string? Nombre { get; set; }

        public Modelo() { }

        public Modelo(int? id, string? nombre, int? marca)
        {
            Id = id;
            Nombre = nombre;
            MarcaId = marca;
        }
    }
}