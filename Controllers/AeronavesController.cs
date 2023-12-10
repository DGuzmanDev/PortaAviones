using GestionHerramientas.Controllers;
using Microsoft.AspNetCore.Mvc;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Servicios;
using PortaAviones.Util;

namespace PortaAviones.Controllers
{

    [Route("api/[controller]")]
    public class AeronavesController : Controller
    {
        private readonly ILogger<AeronavesController> _logger;

        private readonly IServicioAeronave ServicioAeronaves;


        public AeronavesController(ILogger<AeronavesController> logger)
        {
            _logger = logger;
            ServicioAeronaves = new ServicioAeronaves();
        }

        [HttpPost]
        [Route("ingresar")]
        public List<Aeronave> PostNuevoIngreso([FromBody] Ingreso ingreso)
        {
            List<Aeronave> respuesta = new();

            try
            {
                _logger.LogInformation("Ejecutando endpoint de registro para nuevo ingreso de aeronaves");

                if (ingreso != null)
                {
                    respuesta = ServicioAeronaves.Ingresar(ingreso);
                }
                else
                {
                    throw new BadHttpRequestException("El cuerpo de la solicitud no es válido");
                }
            }
            catch (Exception exception)
            {
                TopLevelErrorHandler.ManejarError(exception, nameof(AeronavesController), nameof(PostNuevoIngreso), _logger);
            }

            return respuesta;
        }

        [HttpPut]
        [Route("retirar")]
        public List<Aeronave> PutRetiro([FromBody] Retiro retiro)
        {
            if (retiro != null)
            {
                List<Aeronave> resultado = new();

                try
                {
                    resultado = ServicioAeronaves.Retirar(retiro);
                }
                catch (Exception error)
                {
                    TopLevelErrorHandler.ManejarError(error, nameof(AeronavesController), nameof(PutRetiro), _logger);
                }

                return resultado;
            }
            else
            {
                throw new ArgumentNullException(nameof(retiro), "El cuerpo de la solicitud no es valido");
            }
        }

        [HttpGet]
        [Route("buscar/serie/{serie}")]
        public Aeronave BuscarAeronavePorSerie([FromRoute] string serie)
        {
            Aeronave aeronave = new();

            try
            {
                if (!StringUtils.IsEmpty(serie))
                {
                    return ServicioAeronaves.BuscarPorSerie(serie);
                }
                else
                {
                    throw new ArgumentException("La serie provista no es valida", nameof(serie));
                }
            }
            catch (Exception exception)
            {
                TopLevelErrorHandler.ManejarError(exception, nameof(AeronavesController), nameof(BuscarAeronavePorSerie), _logger);
            }

            return aeronave;

        }

        [HttpGet]
        [Route("buscar/activos")]
        public List<Aeronave> BuscarActivos()
        {
            List<Aeronave> resultado = new();

            try
            {
                resultado = ServicioAeronaves.BuscarActivos();
            }
            catch (Exception error)
            {
                TopLevelErrorHandler.ManejarError(error, nameof(AeronavesController), nameof(BuscarActivos), _logger);
            }

            return resultado;
        }

        [HttpGet]
        [Route("contar/agrupado/activos")]
        public List<ModeloAeronaveAgrupado> ContarModelosActivos()
        {
            List<ModeloAeronaveAgrupado> resultado = new();

            try
            {
                resultado = ServicioAeronaves.ContarModelosActivos();
            }
            catch (Exception error)
            {
                TopLevelErrorHandler.ManejarError(error, nameof(AeronavesController), nameof(BuscarActivos), _logger);
            }

            return resultado;
        }
    }
}