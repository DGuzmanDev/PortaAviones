using PortaAviones.Datos;
using PortaAviones.Interfaces;
using PortaAviones.Models;

namespace PortaAviones.Servicios;

public class ServicioModelos : IServicioModelo
{

    private readonly IConectorDeDatos ConectorDeDatos;

    public ServicioModelos()
    {
        ConectorDeDatos = new ConectorDeDatos();
    }

    public List<Modelo> BuscarPorMarcaId(int marcaId)
    {
        try
        {
            if (marcaId > 0)
            {
                return ConectorDeDatos.BuscarModelosPorMarcaId(marcaId);
            }
            else
            {
                throw new ArgumentException("El ID de Marca no es valido", nameof(marcaId));
            }
        }
        catch (Exception error)
        {
            Console.WriteLine("Error buscando Modelo por Marca ID. Razon: " + error.Message);
            throw;
        }
    }
}
