using System.Transactions;
using GestionHerramientas.Datos;
using GestionHerramientas.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using PortaAviones.Datos.Repositorio;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Util;

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
                            registros.Add(BuscarAeronaveActivaPorSerie(aeronave.Serie, connection));
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

        public Aeronave BuscarAeronaveActivaPorSerie(string serie)
        {
            if (!StringUtils.IsEmpty(serie))
            {
                SqlConnection connection = ConexionSQLServer.ObenerConexion();

                try
                {
                    connection.Open();
                    return BuscarAeronaveActivaPorSerie(serie, connection);
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error buscando Aeronave por serie. Razon: " + error.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                throw new ArgumentException("La serie provista en invalida", nameof(serie));
            }
        }

        public List<Aeronave> RegistrarRetiro(Retiro retiro)
        {
            if (retiro != null && !retiro.Aeronaves.IsNullOrEmpty())
            {
                using (TransactionScope tx = new(TransactionScopeOption.RequiresNew))
                {
                    SqlConnection connection = ConexionSQLServer.ObenerConexion();

                    try
                    {
                        connection.Open();
                        RepositorioAeronave.ActualizarTodos(retiro.Aeronaves, connection, tx);
                        List<Aeronave> registros = new();

                        retiro.Aeronaves.ForEach(aeronave =>
                        {
                            registros.Add(BuscarAeronaveActivaPorSerie(aeronave.Serie, connection));
                        });

                        return registros;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error registrando nuevo Retiro. Razon: " + exception.Message);
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
                throw new ArgumentNullException(nameof(retiro), "El objeto Ingreso es invalido");
            }
        }

        public List<ModeloAeronaveAgrupado> ContarAeronavesAgrupadasPorModelo()
        {

            SqlConnection connection = ConexionSQLServer.ObenerConexion();

            try
            {
                connection.Open();
                return RepositorioAeronave.ContarModelos(connection, false);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error buscando Aeronaves activas. Razon: " + exception.Message);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Aeronave> BuscarAeronavesActivas()
        {
            SqlConnection connection = ConexionSQLServer.ObenerConexion();

            try
            {
                connection.Open();
                List<Aeronave> aeronaves = RepositorioAeronave.BuscarTodosPorRetiro(false, connection);
                aeronaves.ForEach(aeronave =>
                {
                    aeronave.Marca = RepositorioMarca.BuscarPorId(aeronave.Marca.Id.Value, connection);
                    aeronave.Modelo = RepositorioModelo.BuscarPorId(aeronave.Modelo.Id.Value, connection);
                });
                return aeronaves;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error buscando Aeronaves activas. Razon: " + exception.Message);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private Aeronave BuscarAeronaveActivaPorSerie(string serie, SqlConnection connection)
        {
            Aeronave aeronave = RepositorioAeronave.BuscarActivaPorSerie(serie, connection);

            if (aeronave != null && aeronave.Id != null && aeronave.Serie != null)
            {
                aeronave.Marca = RepositorioMarca.BuscarPorId(aeronave.Marca.Id.Value, connection);
                aeronave.Modelo = RepositorioModelo.BuscarPorId(aeronave.Modelo.Id.Value, connection);
            }

            return aeronave;
        }

    }
}

