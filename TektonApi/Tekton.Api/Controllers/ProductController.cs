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
        /// Este m�todo permite consultar un Producto activo.
        /// </summary>
        /// <param name="productId">
        /// El par�metro de entrada "productId" es de tipo n�merico y debe ser mayor a 0, contiene el id del producto a consultar.
        /// </param>
        /// <remarks>
        /// El m�todo retorna una Respuesta, que contiene 2 propiedades:
        /// 
        /// 1.  DataResult, si la ejecuci�n no fue exitos retorna NULL caso contrario un objeto con la siguiente estructura:
        ///     
        ///         {"ProductId": 1, "Name": "MOUSE", "Status": true, "StatusName": "Active", "Stock": 100, "Description": "MOUSE INALAMBRICO", "Price": 20, "Discount": 10, "FinalPrice": 18}
        ///         
        /// 2.  Resultado, que contiene 4 propiedades:
        ///     
        ///     2.1 Ok, true si la ejecuci�n del m�todo fue exitosa caso contrario false.
        ///         
        ///     2.2 Mensajes, lista vac�a si la ejecuci�n o validaci�n del m�todo fue exitosa caso contrario una lista de mensajes de error o validaci�n.
        ///         
        ///     2.3 ErrorValidacion, true si la validaci�n del m�todo fue exitosa caso contrario false.
        ///         
        ///     2.4 StatusCode, c�digo de respuesta del m�todo, existen 4 posibles respuestas:
        ///         
        ///     200 OK                      El producto fue encontrado.
        ///             
        ///     400 Bad Request             El par�metro de entrada "productId" es menor o igual a 0. 
        ///             
        ///     404 Not Found               El producto no fue encontrado.
        ///             
        ///     500 Internal Server Error   Existi� un error no controlado.
        /// </remarks>
        /// <returns>Este m�todo retorna un Producto.</returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(long productId)
        {
            RespuestaViewModel<ProductResponseDTO> respuesta = await _ProductService.GetById(productId, HttpContext.Items["IpAdress"].ToString(), HttpContext.Items["IdLog"].ToString());          
            return this.StatusCode(respuesta.Resultado.StatusCode, respuesta);
        }

        /// <summary>
        /// Este m�todo permite insertar un Producto.
        /// </summary>
        /// <param name="product">
        /// El par�metro de entrada "product" es un objeto con la siguiente estructura:
        /// 
        ///     {"Name": "MOUSE", "Stock": 100, "Description": "MOUSE INALAMBRICO", "Price": 20}
        /// 
        /// Name: Es de tipo alfanum�rico y no debe ser vacia o nula.
        /// 
        /// Stock: Es de tipo num�rico y debe ser mayor o igual a 0.
        /// 
        /// Description: Es de tipo alfanum�rico y no debe ser vacia o nula.
        /// 
        /// Price: Es de tipo num�rico y debe ser mayor o igual a 0.
        /// </param>
        /// <remarks>
        /// El m�todo retorna una Respuesta, que contiene 2 propiedades:
        /// 
        /// 1.  DataResult, si la ejecuci�n no fue exitos retorna NULL caso contrario retorna el id del Producto insertado.
        ///         
        /// 2.  Resultado, que contiene 4 propiedades:
        ///     
        ///     2.1 Ok, true si la ejecuci�n del m�todo fue exitosa caso contrario false.
        ///         
        ///     2.2 Mensajes, lista vac�a si la ejecuci�n o validaci�n del m�todo fue exitosa caso contrario una lista de mensajes de error o validaci�n.
        ///         
        ///     2.3 ErrorValidacion, true si la validaci�n del m�todo fue exitosa caso contrario false.
        ///         
        ///     2.4 StatusCode, c�digo de respuesta del m�todo, existen 3 posibles respuestas:
        ///         
        ///     200 OK                      El producto fue insertado.
        ///             
        ///     400 Bad Request             El par�metro de entrada "product" no cumple las reglas de validaci�n. 
        ///             
        ///     500 Internal Server Error   Existi� un error no controlado.
        /// </remarks>
        /// <returns>Este m�todo retorna el id del Producto insertado.</returns>
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] ProductRequestInsertDTO product)
        {
            RespuestaViewModel<long> respuesta = await _ProductService.Insert(product, HttpContext.Items["IpAdress"].ToString(), HttpContext.Items["IdLog"].ToString());
            return this.StatusCode(respuesta.Resultado.StatusCode, respuesta);
        }

        /// <summary>
        /// Este m�todo permite actualizar un Producto existente.
        /// </summary>
        /// <param name="product">
        /// El par�metro de entrada "product" es un objeto con la siguiente estructura:
        /// 
        ///     {"ProductId": 1, "Name": "MOUSE", "Status": true, "Stock": 100, "Description": "MOUSE INALAMBRICO", "Price": 20}
        /// 
        /// ProductId: Es de tipo num�rico, debe ser mayor a 0 y existir en la tabla Product.
        /// 
        /// Name: Es de tipo alfanum�rico y no debe ser vacia o nula.
        /// 
        /// Status: Es de tipo booleano (true / false), cuando el valor es false inactiva el Producto. 
        /// 
        /// Stock: Es de tipo num�rico y debe ser mayor o igual a 0.
        /// 
        /// Description: Es de tipo alfanum�rico y no debe ser vacia o nula.
        /// 
        /// Price: Es de tipo num�rico y debe ser mayor o igual a 0.
        /// </param>
        /// <remarks>
        /// El m�todo retorna una Respuesta, que contiene 2 propiedades:
        /// 
        /// 1.  DataResult, si la ejecuci�n no fue exitos retorna false caso contrario retorna true.
        ///         
        /// 2.  Resultado, que contiene 4 propiedades:
        ///     
        ///     2.1 Ok, true si la ejecuci�n del m�todo fue exitosa caso contrario false.
        ///         
        ///     2.2 Mensajes, lista vac�a si la ejecuci�n o validaci�n del m�todo fue exitosa caso contrario una lista de mensajes de error o validaci�n.
        ///         
        ///     2.3 ErrorValidacion, true si la validaci�n del m�todo fue exitosa caso contrario false.
        ///         
        ///     2.4 StatusCode, c�digo de respuesta del m�todo, existen 3 posibles respuestas:
        ///         
        ///     200 OK                      El producto fue actualizado.
        ///             
        ///     400 Bad Request             El par�metro de entrada "product" no cumple las reglas de validaci�n. 
        ///
        ///     404 Not Found               El producto a actualizar no fue encontrado.
        ///             
        ///     500 Internal Server Error   Existi� un error no controlado.
        /// </remarks>
        /// <returns>Este m�todo retorna true si la ejecuci�n fue exitosa caso contrario retorna false.</returns>
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ProductRequestUpdateDTO product)
        {
            RespuestaViewModel<bool> respuesta = await _ProductService.Update(product, HttpContext.Items["IpAdress"].ToString(), HttpContext.Items["IdLog"].ToString());
            return this.StatusCode(respuesta.Resultado.StatusCode, respuesta);
        }
    }
}