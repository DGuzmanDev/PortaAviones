using Microsoft.IdentityModel.Tokens;
using PortaAviones.Datos;
using PortaAviones.Interfaces;
using PortaAviones.Models;

namespace PortaAviones.Servicios
{
    public class ServicioAterrizaje : IServicioAterrizaje
    {
        private readonly IConectorDeDatos ConectorDeDatos;

        public ServicioAterrizaje()
        {
            ConectorDeDatos = new ConectorDeDatos();
        }

        public Aterrizaje Guardar(Aterrizaje aterrizaje)
        {
            try
            {
                if (aterrizaje != null && !aterrizaje.Aeronaves.IsNullOrEmpty())
                {
                    aterrizaje.DespegueFk = aterrizaje.Despegue.Id;
                    return ConectorDeDatos.RegistrarAterrizaje(aterrizaje);
                }
                else
                {
                    throw new ArgumentNullException("El objeto Aterrizaje provisto es invalido");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error guardando Aterrizaje. Razon: " + error.Message);
                throw;
            }
        }
    }
}