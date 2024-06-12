using Azure.Core;
using System;
using System.Globalization;
using System.Net;
using System.Text.Json;
using Tekton.Api.Entities;

namespace Tekton.Api.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _dateFormat;

        public RequestMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _dateFormat = configuration.GetSection("appSettings:DateFormat").Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Guid idLog = Guid.NewGuid();
            context.TraceIdentifier = idLog.ToString();
            context.Items["IdLog"] = idLog.ToString();

            string ipAdress = GetIpAddress(context);
            context.Items["IpAdress"] = ipAdress;

            DateTime requestDateTime = DateTime.Now;            
            await _next(context);
            DateTime responseDateTime = DateTime.Now;
            TimeSpan responseTime = responseDateTime.Subtract(requestDateTime);

            RouteData routeData = context.GetRouteData();
            string controllerAction = string.Empty;
            if (routeData != null)
                controllerAction = routeData.Values["controller"].ToString() + " - " + routeData.Values["action"];

            context.RequestServices.GetRequiredService<ILogger<RequestMiddleware>>()
                .LogInformation(string.Format("IdLog: {0} | Date: {1} | Controller - Action: {2} | IpAdress: {3} ! Start Date: {4} | Finish Date: {5} | Response Time (ms): {6}",
                                               idLog, DateTime.Now.ToString(_dateFormat), controllerAction, ipAdress, requestDateTime.ToString(_dateFormat), responseDateTime.ToString(_dateFormat), responseTime.TotalMilliseconds));
        }
        private string GetIpAddress(HttpContext context)
        {
            string ipAdd;
            try
            {
                ipAdd = context.Request.Headers["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ipAdd))
                {
                    ipAdd = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();

                    if (string.IsNullOrEmpty(ipAdd))
                        ipAdd = "Tekton.Api";
                }
            }
            catch (Exception)
            {
                ipAdd = "ProductAPI";
            }

            return ipAdd;
        }
    }

    public static class RequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }
}
