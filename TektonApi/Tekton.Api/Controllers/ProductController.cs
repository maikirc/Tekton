using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tekton.Api.IService;
using Tekton.Api.ViewModel;
using Tekton.Api.ViewModel.DTO;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Tekton.Api.ViewModel;
using Tekton.Api.Entities;
using System.Security;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Net;

namespace Tekton.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _ProductService;
        private readonly string _ipAdress;
        private readonly string _idLog;

        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        /// <summary>
        /// Este método permite consultar un Producto activo.
        /// </summary>
        /// <param name="productId">
        /// El parámetro de entrada "productId" es de tipo númerico y debe ser mayor a 0, contiene el id del producto a consultar.
        /// </param>
        /// <remarks>
        /// El método retorna una Respuesta, que contiene 2 propiedades:
        /// 
        /// 1.  DataResult, si la ejecución no fue exitos retorna NULL caso contrario un objeto con la siguiente estructura:
        ///     
        ///         {"ProductId": 1, "Name": "MOUSE", "Status": true, "StatusName": "Active", "Stock": 100, "Description": "MOUSE INALAMBRICO", "Price": 20, "Discount": 10, "FinalPrice": 18}
        ///         
        /// 2.  Resultado, que contiene 4 propiedades:
        ///     
        ///     2.1 Ok, true si la ejecución del método fue exitosa caso contrario false.
        ///         
        ///     2.2 Mensajes, lista vacía si la ejecución o validación del método fue exitosa caso contrario una lista de mensajes de error o validación.
        ///         
        ///     2.3 ErrorValidacion, true si la validación del método fue exitosa caso contrario false.
        ///         
        ///     2.4 StatusCode, código de respuesta del método, existen 4 posibles respuestas:
        ///         
        ///     200 OK                      El producto fue encontrado.
        ///             
        ///     400 Bad Request             El parámetro de entrada "productId" es menor o igual a 0. 
        ///             
        ///     404 Not Found               El producto no fue encontrado.
        ///             
        ///     500 Internal Server Error   Existió un error no controlado.
        /// </remarks>
        /// <returns>Este método retorna un Producto.</returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(long productId)
        {
            RespuestaViewModel<ProductResponseDTO> respuesta = await _ProductService.GetById(productId, HttpContext.Items["IpAdress"].ToString(), HttpContext.Items["IdLog"].ToString());          
            return this.StatusCode(respuesta.Resultado.StatusCode, respuesta);
        }

        /// <summary>
        /// Este método permite insertar un Producto.
        /// </summary>
        /// <param name="product">
        /// El parámetro de entrada "product" es un objeto con la siguiente estructura:
        /// 
        ///     {"Name": "MOUSE", "Stock": 100, "Description": "MOUSE INALAMBRICO", "Price": 20}
        /// 
        /// Name: Es de tipo alfanumérico y no debe ser vacia o nula.
        /// 
        /// Stock: Es de tipo numérico y debe ser mayor o igual a 0.
        /// 
        /// Description: Es de tipo alfanumérico y no debe ser vacia o nula.
        /// 
        /// Price: Es de tipo numérico y debe ser mayor o igual a 0.
        /// </param>
        /// <remarks>
        /// El método retorna una Respuesta, que contiene 2 propiedades:
        /// 
        /// 1.  DataResult, si la ejecución no fue exitos retorna NULL caso contrario retorna el id del Producto insertado.
        ///         
        /// 2.  Resultado, que contiene 4 propiedades:
        ///     
        ///     2.1 Ok, true si la ejecución del método fue exitosa caso contrario false.
        ///         
        ///     2.2 Mensajes, lista vacía si la ejecución o validación del método fue exitosa caso contrario una lista de mensajes de error o validación.
        ///         
        ///     2.3 ErrorValidacion, true si la validación del método fue exitosa caso contrario false.
        ///         
        ///     2.4 StatusCode, código de respuesta del método, existen 3 posibles respuestas:
        ///         
        ///     200 OK                      El producto fue insertado.
        ///             
        ///     400 Bad Request             El parámetro de entrada "product" no cumple las reglas de validación. 
        ///             
        ///     500 Internal Server Error   Existió un error no controlado.
        /// </remarks>
        /// <returns>Este método retorna el id del Producto insertado.</returns>
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] ProductRequestInsertDTO product)
        {
            RespuestaViewModel<long> respuesta = await _ProductService.Insert(product, HttpContext.Items["IpAdress"].ToString(), HttpContext.Items["IdLog"].ToString());
            return this.StatusCode(respuesta.Resultado.StatusCode, respuesta);
        }

        /// <summary>
        /// Este método permite actualizar un Producto existente.
        /// </summary>
        /// <param name="product">
        /// El parámetro de entrada "product" es un objeto con la siguiente estructura:
        /// 
        ///     {"ProductId": 1, "Name": "MOUSE", "Status": true, "Stock": 100, "Description": "MOUSE INALAMBRICO", "Price": 20}
        /// 
        /// ProductId: Es de tipo numérico, debe ser mayor a 0 y existir en la tabla Product.
        /// 
        /// Name: Es de tipo alfanumérico y no debe ser vacia o nula.
        /// 
        /// Status: Es de tipo booleano (true / false), cuando el valor es false inactiva el Producto. 
        /// 
        /// Stock: Es de tipo numérico y debe ser mayor o igual a 0.
        /// 
        /// Description: Es de tipo alfanumérico y no debe ser vacia o nula.
        /// 
        /// Price: Es de tipo numérico y debe ser mayor o igual a 0.
        /// </param>
        /// <remarks>
        /// El método retorna una Respuesta, que contiene 2 propiedades:
        /// 
        /// 1.  DataResult, si la ejecución no fue exitos retorna false caso contrario retorna true.
        ///         
        /// 2.  Resultado, que contiene 4 propiedades:
        ///     
        ///     2.1 Ok, true si la ejecución del método fue exitosa caso contrario false.
        ///         
        ///     2.2 Mensajes, lista vacía si la ejecución o validación del método fue exitosa caso contrario una lista de mensajes de error o validación.
        ///         
        ///     2.3 ErrorValidacion, true si la validación del método fue exitosa caso contrario false.
        ///         
        ///     2.4 StatusCode, código de respuesta del método, existen 3 posibles respuestas:
        ///         
        ///     200 OK                      El producto fue actualizado.
        ///             
        ///     400 Bad Request             El parámetro de entrada "product" no cumple las reglas de validación. 
        ///
        ///     404 Not Found               El producto a actualizar no fue encontrado.
        ///             
        ///     500 Internal Server Error   Existió un error no controlado.
        /// </remarks>
        /// <returns>Este método retorna true si la ejecución fue exitosa caso contrario retorna false.</returns>
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ProductRequestUpdateDTO product)
        {
            RespuestaViewModel<bool> respuesta = await _ProductService.Update(product, HttpContext.Items["IpAdress"].ToString(), HttpContext.Items["IdLog"].ToString());
            return this.StatusCode(respuesta.Resultado.StatusCode, respuesta);
        }
    }
}