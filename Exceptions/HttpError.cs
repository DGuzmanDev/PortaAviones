using System.Net;
using Microsoft.AspNetCore.WebUtilities;

[Serializable]
public class HttpError : Exception
{
    public string ReasonPhrase { get; set; } = "Error Interno del Servidor";

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

    public HttpError() : base() { }

    public HttpError(string mensaje) : base(mensaje) { }

    public HttpError(string mensaje, Exception causa) : base(mensaje, causa) { }

    public class InternalServerError : HttpError
    {
        public InternalServerError() : base() { }

        public InternalServerError(string mensaje) : base(mensaje)
        {
            ReasonPhrase = mensaje;
        }

        public InternalServerError(string mensaje, Exception causa) : base(mensaje, causa)
        {
            ReasonPhrase = mensaje;
        }
    }

    public class BadRequest : HttpError
    {
        public BadRequest() : base()
        {
            ReasonPhrase = "Solicitud invalida";
            StatusCode = HttpStatusCode.BadRequest;
        }

        public BadRequest(string mensaje) : base(mensaje)
        {
            ReasonPhrase = mensaje;
            StatusCode = HttpStatusCode.BadRequest;
        }

        public BadRequest(string mensaje, Exception causa) : base(mensaje, causa)
        {
            ReasonPhrase = mensaje;
            StatusCode = HttpStatusCode.BadRequest;
        }
    }

    public class Conflict : HttpError
    {
        public Conflict() : base()
        {
            ReasonPhrase = "Recurso duplicado";
            StatusCode = HttpStatusCode.Conflict;
        }

        public Conflict(string mensaje) : base(mensaje)
        {
            ReasonPhrase = mensaje;
            StatusCode = HttpStatusCode.Conflict;
        }

        public Conflict(string mensaje, Exception causa) : base(mensaje, causa)
        {
            ReasonPhrase = mensaje;
            StatusCode = HttpStatusCode.Conflict;
        }
    }

}