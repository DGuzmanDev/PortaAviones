﻿using System.Transactions;
using PortaAviones.Datos;
using PortaAviones.Exceptions;
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
        private static readonly string SELECT_YEAR = "SELECT YEAR(GETDATE())";
        private static readonly string MENSAJE_UNIQ_KEY_VIOLATION = "Violation of UNIQUE KEY constraint";

        private readonly IRepositorioMarca RepositorioMarca;
        private readonly IRepositorioModelo RepositorioModelo;
        private readonly IRepositorioAeronave RepositorioAeronave;
        private readonly IRepositorioDespegue RepositorioDespegue;
        private readonly IRepositorioAterrizaje RepositorioAterrizaje;
        private readonly IRepositorioAeronaveDespegue RepositorioDespegueAeronave;
        private readonly IRepositorioAeronaveAterrizaje RepositorioAeronaveAterrizaje;

        public ConectorDeDatos()
        {
            RepositorioAeronave = new RepositorioAeronave();
            RepositorioMarca = new RepositorioMarca();
            RepositorioModelo = new RepositorioModelo();
            RepositorioDespegue = new RepositorioDespegue();
            RepositorioDespegueAeronave = new RepositorioAeronaveDespegue();
            RepositorioAterrizaje = new RepositorioAterrizaje();
            RepositorioAeronaveAterrizaje = new RepositorioAeronaveAterrizaje();
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

        public Despegue RegistrarDespegue(Despegue despegue)
        {
            if (despegue != null && !despegue.Aeronaves.IsNullOrEmpty())
            {
                using (TransactionScope tx = new(TransactionScopeOption.RequiresNew))
                {
                    SqlConnection connection = ConexionSQLServer.ObenerConexion();

                    try
                    {
                        connection.Open();
                        despegue.Codigo = ObtenerCodigoDespegue(connection);
                        RepositorioDespegue.Guardar(despegue, connection, tx, false);
                        Despegue nuevoRegistro = RepositorioDespegue.BuscarPorCodigo(despegue.Codigo, connection);

                        despegue.Aeronaves.ForEach(despegueAeronave =>
                        {
                            despegueAeronave.DespegueFk = nuevoRegistro.Id;
                            despegueAeronave.AeronaveFk = despegueAeronave.Aeronave.Id;
                            RepositorioDespegueAeronave.Guardar(despegueAeronave, connection, tx, false);
                        });

                        tx.Complete();
                        return nuevoRegistro;
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("Error registrando el nuevo Despegue. Razon: " + error.Message);
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
                throw new ArgumentException("El Despegue provisto no es valido");
            }
        }

        public Despegue BuscarDespeguePorCodigo(string codigo)
        {
            if (!StringUtils.IsEmpty(codigo))
            {
                SqlConnection connection = ConexionSQLServer.ObenerConexion();

                try
                {
                    connection.Open();
                    Despegue despegue = RepositorioDespegue.BuscarPorCodigo(codigo, connection);
                    if (despegue != null && despegue.Id != null)
                    {
                        List<AeronaveDespegue> despegueAeronaves = RepositorioDespegueAeronave.BuscarPorDespegueId(despegue.Id.Value, connection);
                        despegueAeronaves.ForEach(despegueAeronave =>
                        {
                            Aeronave aeronave = RepositorioAeronave.BuscarPorId(despegueAeronave.AeronaveFk.Value, connection);
                            aeronave.Marca = RepositorioMarca.BuscarPorId(aeronave.Marca.Id.Value, connection);
                            aeronave.Modelo = RepositorioModelo.BuscarPorId(aeronave.Modelo.Id.Value, connection);
                            despegueAeronave.Aeronave = aeronave;
                        });
                        despegue.Aeronaves = despegueAeronaves;
                    }

                    return despegue ?? new();
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error buscando Despegue por codigo. Razon: " + error.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                throw new ArgumentException("El codigo provisto es invalido");
            }
        }

        public List<Despegue> BuscarDespegues()
        {

            SqlConnection sqlConnection = ConexionSQLServer.ObenerConexion();
            try
            {
                sqlConnection.Open();
                return RepositorioDespegue.BuscarTodos(sqlConnection);
            }
            catch (Exception error)
            {
                Console.WriteLine("Error buscando todos los Despegues. Razon: " + error.Message);
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private static string ObtenerAnnioSqlServer(SqlConnection sqlConnection)
        {
            SqlCommand select = new(SELECT_YEAR, sqlConnection);
            SqlDataReader sqlDataReader = select.ExecuteReader();

            try
            {
                sqlDataReader.Read();
                int annio = sqlDataReader.GetInt32(0);
                return annio.ToString();
            }
            finally
            {
                sqlDataReader.Close();
            }
        }

        private string ObtenerCodigoDespegue(SqlConnection connection)
        {
            int secuencia = RepositorioDespegue.ObtenerSiguienteIdentificador(connection);
            string secuenciaCodigo = secuencia.ToString();
            string annioSqlServer = ObtenerAnnioSqlServer(connection);

            if (secuenciaCodigo.Length < 6)
            {
                int cuentaRelleno = 6 - secuenciaCodigo.Length;
                string relleno = "";
                for (int pos = 1; pos < cuentaRelleno; pos++)
                {
                    relleno += "0";
                }

                secuenciaCodigo = relleno + secuenciaCodigo;
            }

            return annioSqlServer + "DE" + secuenciaCodigo;
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

        public Aterrizaje RegistrarAterrizaje(Aterrizaje aterrizaje)
        {
            if (aterrizaje != null && !aterrizaje.Aeronaves.IsNullOrEmpty())
            {
                using (TransactionScope tx = new(TransactionScopeOption.RequiresNew))
                {
                    SqlConnection connection = ConexionSQLServer.ObenerConexion();

                    try
                    {
                        connection.Open();
                        RepositorioAterrizaje.Guardar(aterrizaje, connection);
                        Aterrizaje nuevoRegistro = RepositorioAterrizaje.BuscarPorDespegueId(aterrizaje.Despegue.Id.Value, connection);

                        aterrizaje.Aeronaves.ForEach(aeronaveAterrizaje =>
                        {
                            aeronaveAterrizaje.AterrizajeFk = nuevoRegistro.Id;
                            aeronaveAterrizaje.AeronaveFk = aeronaveAterrizaje.Aeronave.Id;
                            RepositorioAeronaveAterrizaje.Guardar(aeronaveAterrizaje, connection);
                        });

                        tx.Complete();
                        return nuevoRegistro;
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("Error registrando el nuevo Despegue. Razon: " + error.Message);
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
                throw new ArgumentException("El Aterrizaje provisto no es valido");
            }
        }
    }
}

