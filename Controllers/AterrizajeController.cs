using Microsoft.AspNetCore.Mvc;
using PortaAviones.Interfaces;
using PortaAviones.Models;
using PortaAviones.Servicios;

namespace PortaAviones.Controllers
{
    [Route("api/[controller]")]
    public class AterrizajeController : Controller
    {
        private readonly ILogger<AterrizajeController> _logger;

        private readonly IServicioAterrizaje ServicioAterrizaje;

        public AterrizajeController(ILogger<AterrizajeController> logger)
        {
            _logger = logger;
            ServicioAterrizaje = new ServicioAterrizaje();
        }

        [HttpPost]
        [Route("guardar")]
        public Aterrizaje Guardar([FromBody] Aterrizaje aterrizaje)
        {
            Aterrizaje respuesta = new();

            try
            {
                if (aterrizaje != null)
                {
                    respuesta = ServicioAterrizaje.Guardar(aterrizaje);
                }
                else
                {
                    throw new HttpError.BadRequest("El cuerpo de la solicitud no es valido");
                }
            }
            catch (Exception error)
            {
                TopLevelErrorHandler.ManejarError(error, nameof(AterrizajeController), nameof(Guardar), _logger);
            }

            return respuesta;
        }

    }
}