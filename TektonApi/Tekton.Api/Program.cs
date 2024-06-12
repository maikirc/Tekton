using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Tekton.Api;
using Tekton.Api.Service.Extension;
using System.Text.Json.Serialization;
using Serilog;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Tekton.Api.Repository;
using System.Text.Json;
using Tekton.Api.Middleware;
using Tekton.Api.IService;
using Tekton.Api.Service;

#region Variable
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
var services = builder.Services;
var configuration = builder.Configuration;
WebApplication app = null;


using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Disabled);
});
var logger = loggerFactory.CreateLogger<Program>();

services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(i => i.FullName);//evitar conflicto entre esquemas de clases duplicados
    c.DocInclusionPredicate((docName, description) => true);
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Service Api",
        Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
        Description = "Web API in ASP.NET Core"
    });

    string xmlPath = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; ;
    if (!string.IsNullOrEmpty(xmlPath))
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});
#endregion

#region Services
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

services.AddEndpointsApiExplorer();

services.AddProductService();

services.AddSingleton<IStatusService, StatusService>();
services.AddMemoryCache();

services.AddHttpContextAccessor();
#endregion

services.AddAutoMapper(typeof(ProductRepository));

#region App
app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger(c => { c.RouteTemplate = "{documentName}/swagger.json"; });

    // Habilite el middleware para  swagger-ui (HTML, JS, CSS, etc.), especificando el punto final Swagger JSON.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "v1");
        c.RoutePrefix = string.Empty;
        c.DefaultModelsExpandDepth(-1); // establecido en -1 ocultar completamente los esquemas
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseRequest();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.ConfigureExceptionHandler(configuration.GetSection("appSettings:MostrarErrores").Value, logger);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
#endregion