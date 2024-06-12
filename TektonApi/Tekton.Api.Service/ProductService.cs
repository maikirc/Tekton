using Azure.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using Tekton.Api.Entities;
using Tekton.Api.IService;
using Tekton.Api.Logic;
using Tekton.Api.Validator;
using Tekton.Api.ViewModel;
using Tekton.Api.ViewModel.DTO;
using static Azure.Core.HttpHeader;

namespace Tekton.Api.Service
{
    public class ProductService : IProductService
    {
        private readonly ProductLogic _ProductLogic;
        private readonly ILogger<ProductService> _logger;
        private readonly IStatusService _StatusService;
        private readonly string _urlMockapiService;

        public ProductService(ProductLogic productLogic, ILogger<ProductService> logger, IStatusService StatusService, IConfiguration configuration)
        {
            _ProductLogic = productLogic;
            _logger = logger;
            _StatusService = StatusService;
            _urlMockapiService = configuration.GetSection("appSettings:UrlMockapiService").Value;
        }

        public async Task<RespuestaViewModel<ProductResponseDTO>> GetById(long productId, string ipAdress, string idLog)
        {
            RespuestaViewModel<ProductResponseDTO> respuesta = new();
            string mensajeErrorValidacion = string.Empty;

            try
            {
                #region Validación de Propiedades
                if (productId <= 0)
                    mensajeErrorValidacion = "El parámetro Id Producto debe ser mayor a 0.";

                if (!string.IsNullOrEmpty(mensajeErrorValidacion))
                {
                    var parametros = $"GetById Service Layer: {productId.ToString()} Mensajes {mensajeErrorValidacion}";
                    var props = new Dictionary<string, object>(){
                            { "Metodo", "GetById" },
                            { "Sitio", "PRODUCT-API" },
                            { "Parametros", parametros },
                            { "IdLog", idLog },
                            { "IpAdress", ipAdress }
                    };

                    using (_logger.BeginScope(props))
                        _logger.LogWarning("Api.Product: Registro en SeriLog");

                    respuesta.Resultado.Ok = true;
                    respuesta.Resultado.ErrorValidacion = true;
                    respuesta.Resultado.Mensajes.Add(string.Format(ProductMessages.EXCEPCION_VALIDACION, idLog + " " + mensajeErrorValidacion));
                    respuesta.Resultado.StatusCode = 400;
                    return respuesta;
                }
                #endregion

                ProductResponseDTO productResult = await _ProductLogic.GetById(productId);

                if (productResult != null)
                {
                    List<StatusViewModel> status = _StatusService.GetStatus();
                    productResult.StatusName = status.Find(s => s.StatusId == (productResult.Status ? 1 : 0)).Name;

                    #region MockApi Service 
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(_urlMockapiService + productResult.ProductId.ToString()).Result;
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var respuestaServ = await response.Content.ReadAsStringAsync();
                        var productDiscount = JsonConvert.DeserializeObject<ProductDiscountViewModel>(respuestaServ);
                        productResult.Discount = productDiscount.Discount;
                        productResult.FinalPrice = productResult.Price * (100 - productResult.Discount) / 100;
                    }
                    else 
                    {
                        // Cuando el producto no tiene registrado un descuento se estima que el descuento es 0.
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                            productResult.FinalPrice = productResult.Price;
                        // Cuando el servicio de descuento del producto no esta disponible se estima que el descuento es 0, porque no esta definida esa regla del negocio.
                        else
                            productResult.FinalPrice = productResult.Price;
                    }
                    #endregion
                }

                respuesta.DataResult = (productResult != null) ? productResult : null;
                respuesta.Resultado.Ok = true;
                respuesta.Resultado.StatusCode = (productResult != null) ? 200 : 404;
                respuesta.Resultado.Mensajes = (productResult != null) ? null : new List<string>(new string[] { ProductMessages.SIN_DATOS });
                return respuesta;
            }
            catch (Exception ex)
            {
                var parametros = $"GetById Service Layer: {productId.ToString()} Mensajes {ex.ToString()}";
                var props = new Dictionary<string, object>(){
                            { "Metodo", "GetById" },
                            { "Sitio", "PRODUCT-API" },
                            { "Parametros", parametros },
                            { "IdLog", idLog },
                            { "IpAdress", ipAdress }
                    };

                respuesta.Resultado.Mensajes.Add(string.Format(ProductMessages.EXCEPCION_NO_CONTROLADA, idLog + "-" + ex.ToString()));
                respuesta.DataResult = null;
                respuesta.Resultado.Ok = false;
                respuesta.Resultado.StatusCode = 500;
                using (_logger.BeginScope(props))
                    _logger.LogError(ex, "Api.Product: Registro en SeriLog");
                return respuesta;
            }
        }       

        public async Task<RespuestaViewModel<long>> Insert(ProductRequestInsertDTO product, string ipAdress, string idLog)
        {
            RespuestaViewModel<long> respuesta = new();
            long productId;

            try
            {
                #region Validación de Propiedades
                var validator = new ProductRequestInsertDTOValidator();
                var validatorResult = validator.Validate(product);
                if (!validatorResult.IsValid)
                {
                    var parametros = $"Insert Service Layer: Mensajes {validatorResult.ToString()}";
                    var props = new Dictionary<string, object>(){
                            { "Metodo", "Insert" },
                            { "Sitio", "PRODUCT-API" },
                            { "Parametros", parametros },
                            { "IdLog", idLog },
                            { "IpAdress", ipAdress }
                    };

                    using (_logger.BeginScope(props))
                        _logger.LogWarning("Api.Product: Registro en SeriLog");

                    respuesta.Resultado.Ok = true;
                    respuesta.Resultado.ErrorValidacion = true;
                    respuesta.Resultado.Mensajes.Add(string.Format(ProductMessages.EXCEPCION_VALIDACION, idLog + " " + validatorResult.ToString()));
                    respuesta.Resultado.StatusCode = 400;
                    return respuesta;
                }
                #endregion                

                productId = await _ProductLogic.Insert(product, ipAdress);

                respuesta.DataResult = productId;
                respuesta.Resultado.Ok = (productId > 0) ? true : false;
                respuesta.Resultado.StatusCode = (productId > 0) ? 200 : 500;
                respuesta.Resultado.Mensajes = (productId > 0) ? null : new List<string>(new string[] { ProductMessages.EXCEPCION_INSERT });
                return respuesta;
            }
            catch (Exception ex)
            {
                var parametros = $"Insert Service Layer: Mensajes {ex.ToString()}";
                var props = new Dictionary<string, object>(){
                            { "Metodo", "Insert" },
                            { "Sitio", "PRODUCT-API" },
                            { "Parametros", parametros },
                            { "IdLog", idLog },
                            { "IpAdress", ipAdress }
                    };

                respuesta.Resultado.Mensajes.Add(string.Format(ProductMessages.EXCEPCION_NO_CONTROLADA, idLog) + "-" + ex.ToString());
                respuesta.DataResult = 0;
                respuesta.Resultado.Ok = false;
                respuesta.Resultado.StatusCode = 500;
                using (_logger.BeginScope(props))
                    _logger.LogError(ex, "Api.Product: Registro en SeriLog");
                return respuesta;
            }
        }

        public async Task<RespuestaViewModel<bool>> Update(ProductRequestUpdateDTO product, string ipAdress, string idLog)
        {
            RespuestaViewModel<bool> respuesta = new();
            string mensajeErrorValidacion = string.Empty;
            bool result;

            try
            {
                #region Validación de Propiedades
                var validator = new ProductRequestUpdateDTOValidator();
                var validatorResult = validator.Validate(product);
                if (validatorResult.IsValid)
                {
                    ProductResponseDTO productResult = await _ProductLogic.GetById(product.ProductId);
                    if (productResult == null)
                    {
                        mensajeErrorValidacion = "El producto nro. " + product.ProductId.ToString() + " no puede ser actualizado porque no existe.";
                        respuesta.Resultado.StatusCode = 404;
                    }
                }
                else
                {
                    mensajeErrorValidacion = validatorResult.ToString();
                    respuesta.Resultado.StatusCode = 400;
                }
                
                if (!string.IsNullOrEmpty(mensajeErrorValidacion))
                {
                    var parametros = $"Update Service Layer: Mensajes {mensajeErrorValidacion}";
                    var props = new Dictionary<string, object>(){
                            { "Metodo", "Update" },
                            { "Sitio", "PRODUCT-API" },
                            { "Parametros", parametros },
                            { "IdLog", idLog },
                            { "IpAdress", ipAdress }
                    };

                    using (_logger.BeginScope(props))
                        _logger.LogWarning("Api.Product: Registro en SeriLog");

                    respuesta.Resultado.Ok = true;
                    respuesta.Resultado.ErrorValidacion = true;
                    respuesta.Resultado.Mensajes.Add(string.Format(ProductMessages.EXCEPCION_VALIDACION, idLog + " " + mensajeErrorValidacion));
                    return respuesta;
                }
                #endregion

                result = await _ProductLogic.Update(product, ipAdress);

                respuesta.DataResult = result;
                respuesta.Resultado.Ok = result;
                respuesta.Resultado.StatusCode = result ? 200 : 500;
                respuesta.Resultado.Mensajes = result ? null : new List<string>(new string[] { ProductMessages.EXCEPCION_UPDATE });
                return respuesta;
            }
            catch (Exception ex)
            {
                var parametros = $"Update Service Layer:";
                var props = new Dictionary<string, object>(){
                            { "Metodo", "Update" },
                            { "Sitio", "PRODUCT-API" },
                            { "Parametros", parametros },
                            { "IdLog", idLog },
                            { "IpAdress", ipAdress }
                    };

                respuesta.Resultado.Mensajes.Add(string.Format(ProductMessages.EXCEPCION_NO_CONTROLADA, idLog + "-" + ex.ToString()));
                respuesta.DataResult = false;
                respuesta.Resultado.Ok = false;
                respuesta.Resultado.StatusCode = 500;
                using (_logger.BeginScope(props))
                    _logger.LogError(ex, "Api.Product: Registro en SeriLog");
                return respuesta;
            }
        }
    }
}