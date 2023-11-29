using PortaAviones.Interfaces;

namespace PortaAviones.Datos
{
    public class ConectorDeDatos : IConectorDeDatos
    {
        private static readonly string MENSAJE_UNIQ_KEY_VIOLATION = "Violation of UNIQUE KEY constraint";

        // private readonly IRepositorioColaborador RepositorioColaborador;
        // private readonly IRepositorioHerramienta RepositorioHerramienta;

        public ConectorDeDatos()
        {
            // RepositorioHerramienta = new RepositorioHerramienta();
            // RepositorioColaborador = new RepositorioColaborador();
        }
    }
}

