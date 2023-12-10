using PortaAviones.Datos;
using PortaAviones.Interfaces;
using PortaAviones.Models;

namespace PortaAviones.Servicios;

public class ServicioMarcas : IServicioMarca
{

    private readonly IConectorDeDatos ConectorDeDatos;

    public ServicioMarcas()
    {
        ConectorDeDatos = new ConectorDeDatos();
    }

    public List<Marca> BuscarTodos()
    {
        try
        {
            return ConectorDeDatos.BuscarMarcas();
        }
        catch (Exception error)
        {
            Console.WriteLine("Error buscando todas las Marcas. Razon: " + error.Message);
            throw;
        }
    }
}
