using PortaAviones.Controllers;
using Microsoft.AspNetCore.Mvc;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Servicios;

namespace PortaAviones.Controllers
{

    [Route("api/[controller]")]
    public class MarcasController : Controller
    {

        private readonly ILogger<MarcasController> _logger;

        private readonly IServicioMarca ServicioMarcas;

        public MarcasController(ILogger<MarcasController> logger)
        {
            _logger = logger;
            ServicioMarcas = new ServicioMarcas();
        }

        [HttpGet]
        [Route("buscar")]
        public List<Marca> ListarTodos()
        {
            List<Marca> marcas = new();

            try
            {
                marcas = ServicioMarcas.BuscarTodos();
            }
            catch (Exception error)
            {
                TopLevelErrorHandler.ManejarError(error, nameof(MarcasController), nameof(ListarTodos), _logger);
            }

            return marcas;
        }

    }
}