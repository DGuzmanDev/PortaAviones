using Microsoft.IdentityModel.Tokens;
using PortaAviones.Datos;
using PortaAviones.Datos.Repositorio;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Util;

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
            if (!StringUtils.IsEmpty(serie))
            {
                try
                {
                    return ConectorDeDatos.BuscarAeronaveActivaPorSerie(serie);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error buscando Aeronave por serie. Razon: " + exception.Message);
                    throw;
                }
            }
            else
            {
                throw new ArgumentException("La serie provista no es valida", nameof(serie));
            }
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

        public List<Aeronave> Retirar(Retiro retiro)
        {
            if (retiro != null && !retiro.Aeronaves.IsNullOrEmpty())
            {
                try
                {
                    retiro.Aeronaves.ForEach(aeronave =>
                    {
                        aeronave.TecnicoRetiro = retiro.Tecnico;
                        aeronave.RazonRetiro = retiro.Razon;
                        aeronave.Retirado = true;
                    });
                    return ConectorDeDatos.RegistrarRetiro(retiro);
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error registrando nuevo Retiro. Razon: " + error.Message);
                    throw;
                }
            }
            else
            {
                throw new ArgumentException("El retiro es invalido", nameof(retiro));
            }
        }

        public List<Aeronave> BuscarActivos()
        {
            try
            {
                return ConectorDeDatos.BuscarAeronavesActivas();
            }
            catch (Exception error)
            {
                Console.WriteLine("Error buscando todas las aeronaves activas. Razon: " + error.Message);
                throw;
            }
        }

        public List<ModeloAeronaveAgrupado> ContarModelosActivos()
        {
            try
            {
                return ConectorDeDatos.ContarAeronavesAgrupadasPorModelo();
            }
            catch (Exception error)
            {
                Console.WriteLine("Error contando todas aeronaves activas agrupadas por modelo. Razon: " + error.Message);
                throw;
            }
        }
    }
}