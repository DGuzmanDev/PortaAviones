using GestionHerramientas.Controllers;
using Microsoft.AspNetCore.Mvc;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Servicios;
using PortaAviones.Util;

namespace PortaAviones.Controllers
{

    [Route("/api/[controller]")]
    public class DespegueController : Controller
    {
        private readonly ILogger<DespegueController> _logger;

        private readonly IServicioDespegue ServicioDespegue;

        public DespegueController(ILogger<DespegueController> logger)
        {
            _logger = logger;
            ServicioDespegue = new ServicioDespegue();
        }

        [HttpPost]
        [Route("guardar")]
        public Despegue Guardar([FromBody] Despegue despegue)
        {
            Despegue respuesta = new();

            try
            {
                if (despegue != null)
                {
                    respuesta = ServicioDespegue.Guardar(despegue);
                }
                else
                {
                    throw new ArgumentNullException(nameof(despegue), "El cuerpo de la solicitud es invalid");
                }
            }
            catch (Exception error)
            {
                TopLevelErrorHandler.ManejarError(error, nameof(DespegueController), nameof(Guardar), _logger);
            }

            return respuesta;
        }

        [HttpGet]
        [Route("buscar/codigo/{codigo}")]
        public Despegue BuscarPorCodigo([FromRoute] string codigo)
        {
            Despegue respuesta = new();

            try
            {
                if (!StringUtils.IsEmpty(codigo))
                {
                    respuesta = ServicioDespegue.BuscarPorCodigo(codigo);
                }
                else
                {
                    throw new ArgumentException("El codigo provisto es invalido", nameof(codigo));
                }
            }
            catch (Exception error)
            {
                TopLevelErrorHandler.ManejarError(error, nameof(DespegueController), nameof(BuscarPorCodigo), _logger);
            }

            return respuesta;
        }
    }
}