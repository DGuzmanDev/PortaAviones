using GestionHerramientas.Controllers;
using Microsoft.AspNetCore.Mvc;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Servicios;

namespace PortaAviones.Controllers
{

    [Route("api/[controller]")]
    public class ModelosController : Controller
    {
        private readonly ILogger<ModelosController> _logger;

        public readonly IServicioModelo ServicioModelos;

        public ModelosController(ILogger<ModelosController> logger)
        {
            _logger = logger;
            ServicioModelos = new ServicioModelos();
        }

        [HttpGet]
        [Route("buscar/marca/{marcaId}")]
        public List<Modelo> BuscarPorMarca([FromRoute] int marcaId)
        {
            List<Modelo> modelos = new();

            try
            {
                if (marcaId > 0)
                {
                    modelos = ServicioModelos.BuscarPorMarcaId(marcaId);
                }
                else
                {
                    throw new BadHttpRequestException("Marca ID invalido");
                }
            }
            catch (Exception error)
            {
                TopLevelErrorHandler.ManejarError(error, nameof(ModelosController), nameof(BuscarPorMarca), _logger);
            }


            return modelos;
        }
    }
}


