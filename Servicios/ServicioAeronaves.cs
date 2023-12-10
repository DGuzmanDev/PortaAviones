using Microsoft.IdentityModel.Tokens;
using PortaAviones.Datos;
using PortaAviones.Interfaces;
using PortaAviones.Models;

namespace PortaAviones.Servicios
{
    public class ServicioAeronaves : IServicioAeronave
    {
        private readonly IConectorDeDatos ConectorDeDatos;

        public ServicioAeronaves()
        {
            ConectorDeDatos = new ConectorDeDatos();
        }

        public Aeronave BuscarPorSerie(string serie)
        {
            throw new NotImplementedException();
        }

        public List<Aeronave> Ingresar(Ingreso ingreso)
        {
            if (ingreso != null && !ingreso.Aeronaves.IsNullOrEmpty())
            {
                try
                {
                    ingreso.Aeronaves.ForEach(aeronave =>
                    {
                        aeronave.TecnicoIngreso = ingreso.Tecnico;
                    });
                    return ConectorDeDatos.RegistrarNuevoIngreso(ingreso);
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error guardando nuevo Ingreso. Razon: " + error.Message);
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(ingreso), "El nuevo ingreso provisto es invalido");
            }
        }

        public Retiro Retirar(Retiro retiro)
        {
            throw new NotImplementedException();
        }
    }
}