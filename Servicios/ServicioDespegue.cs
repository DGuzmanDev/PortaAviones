using Microsoft.IdentityModel.Tokens;
using PortaAviones.Datos;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Util;

namespace PortaAviones.Servicios
{
    public class ServicioDespegue : IServicioDespegue
    {
        private readonly IConectorDeDatos ConectorDeDatos;

        public ServicioDespegue()
        {
            ConectorDeDatos = new ConectorDeDatos();
        }

        public Despegue BuscarPorCodigo(string codigo)
        {
            if (!StringUtils.IsEmpty(codigo))
            {
                try
                {
                    return ConectorDeDatos.BuscarDespeguePorCodigo(codigo);
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error buscando Despegue por codigo. Razon: " + error.Message);
                    throw;
                }
            }
            else
            {
                throw new ArgumentException("El codigo provisto es invalido");
            }
        }

        public Despegue Guardar(Despegue despegue)
        {
            if (despegue != null && !despegue.Aeronaves.IsNullOrEmpty())
            {
                try
                {
                    return ConectorDeDatos.RegistrarDespegue(despegue);
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error registrando nuevo Despegue. Razon: " + error.Message);
                    throw;
                }
            }
            else
            {
                throw new ArgumentException("El Despegue provisto es invalido");
            }
        }
    }
}