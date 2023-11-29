using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortaAviones.Models;

namespace PortaAviones.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async void ErrorHandler()
    {
        _logger.LogError("Ejecutando filtro de manejo de errores de los controladores");
        int statusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        string responseBody = "Error inesperado del servidor";

        var exceptionHandlerPathFeature =
            HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error != null)
        {
            if (exceptionHandlerPathFeature?.Error is HttpError httpError)
            {
                statusCode = (int)httpError.StatusCode;
                responseBody = httpError.ReasonPhrase;
            }
            else
            {
                _logger.LogError($"Excepcion desconocida:{exceptionHandlerPathFeature?.Error.GetType().FullName}");
            }
        }

        HttpContext.Response.StatusCode = statusCode;
        await HttpContext.Response.WriteAsJsonAsync(responseBody);
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error([FromQuery] string? errorMessage, [FromQuery] string? httpError)
    {
        _logger.LogError("Redireccionamiento a la pagina de error");
        ErrorViewModel errorViewModel = new()
        {
            HttpError = httpError ?? "Error desconocido",
            ErrorMessage = errorMessage ?? "No hay detalles adicionales",
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };
        return View(errorViewModel);
    }
}
