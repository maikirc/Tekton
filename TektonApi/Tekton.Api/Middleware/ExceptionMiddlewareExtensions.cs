using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;
using Tekton.Api.ViewModel;

namespace Tekton.Api.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Configura el manejador de excepciones.
        /// </summary>
        /// <param name="app">La aplicación.</param>
        /// <param name="mostrarErrorTecnico">Indica si se muestra o no los mensajes Técnicos</param>
        /// <param name="logger"></param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, string mostrarErrorTecnico, ILogger logger)
        {
            _ = app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var ex = contextFeature.Error;

                        var respuesta = new RespuestaViewModel<string>();

                        Guid idLog = Guid.NewGuid();

                        var props = new Dictionary<string, object>(){
                                    { "Capa", "Api" },
                                    { "Sitio", "PRODUCT-API" },
                                    { "IdLog", idLog }
                        };

                        respuesta.Resultado.Mensajes.Add(mostrarErrorTecnico.Equals("S") ? string.Format(ProductMessages.EXCEPCION_NO_CONTROLADA, idLog.ToString() + " " + ex.ToString()) :
                                            string.Format(ProductMessages.EXCEPCION_NO_CONTROLADA, idLog.ToString() + " En este momento no es posible procesar su solicitud, inténtelo más tarde."));
                        respuesta.DataResult = null;
                        respuesta.Resultado.Ok = false;
                        respuesta.Resultado.StatusCode = 500;
                        using (logger.BeginScope(props))
                            logger.LogError(ex, "Api.Permission: Registro en SeriLog");

                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        await context.Response.WriteAsync(JsonSerializer.Serialize(respuesta));
                    }
                });
            });
        }
    }
}
