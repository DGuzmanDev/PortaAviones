using System.Transactions;
using GestionHerramientas.Datos;
using GestionHerramientas.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using PortaAviones.Datos.Repositorio;
using PortaAviones.Interfaces;
using PortaAviones.Models;

namespace PortaAviones.Datos
{
    public class ConectorDeDatos : IConectorDeDatos
    {
        private static readonly string MENSAJE_UNIQ_KEY_VIOLATION = "Violation of UNIQUE KEY constraint";

        private readonly IRepositorioMarca RepositorioMarca;
        private readonly IRepositorioModelo RepositorioModelo;
        private readonly IRepositorioAeronave RepositorioAeronave;

        public ConectorDeDatos()
        {
            RepositorioAeronave = new RepositorioAeronave();
            RepositorioMarca = new RepositorioMarca();
            RepositorioModelo = new RepositorioModelo();
        }

        public List<Marca> BuscarMarcas()
        {
            SqlConnection connection = ConexionSQLServer.ObenerConexion();

            try
            {
                connection.Open();
                return RepositorioMarca.BuscarTodos(connection);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error buscando Marcas. Razon: " + exception.Message);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Modelo> BuscarModelosPorMarcaId(int marcaId)
        {

            if (marcaId > 0)
            {
                SqlConnection connection = ConexionSQLServer.ObenerConexion();

                try
                {
                    connection.Open();
                    return RepositorioModelo.BuscarPorMarca(marcaId, connection);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error buscando Modelos por Marca ID. Razon: " + exception.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(marcaId), "El ID de Marca provisto no es valido");
            }
        }

        public List<Aeronave> RegistrarNuevoIngreso(Ingreso ingreso)
        {
            if (ingreso != null && !ingreso.Aeronaves.IsNullOrEmpty())
            {
                using (TransactionScope tx = new(TransactionScopeOption.RequiresNew))
                {
                    SqlConnection connection = ConexionSQLServer.ObenerConexion();

                    try
                    {
                        connection.Open();
                        RepositorioAeronave.GuardarTodos(ingreso.Aeronaves, connection, tx);
                        List<Aeronave> registros = new();

                        ingreso.Aeronaves.ForEach(aeronave =>
                        {
                            Marca marca = RepositorioMarca.BuscarPorId(aeronave.Marca.Id.Value, connection);
                            Modelo modelo = RepositorioModelo.BuscarPorId(aeronave.Modelo.Id.Value, connection);
                            Aeronave nuevoRegistro = RepositorioAeronave.BuscarPorSerie(aeronave.Serie, connection);
                            nuevoRegistro.Marca = marca;
                            nuevoRegistro.Modelo = modelo;
                            registros.Add(nuevoRegistro);
                        });

                        return registros;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error guardando nuevo Ingreso. Razon: " + exception.Message);

                        if ((exception is SqlException) && exception.Message.Contains(MENSAJE_UNIQ_KEY_VIOLATION))
                        {
                            throw new DataBaseError.ViolacionDeLlaveUnica("Ya existe una aeronave con la serie dada", exception);
                        }

                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(ingreso), "El objeto Ingreso es invalido");
            }
        }
    }
}

