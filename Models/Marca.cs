namespace PortaAviones.Models
{
    public class Marca
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }

        public Marca(){}

        public Marca(int? id, string? nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}