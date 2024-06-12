using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Tekton.Api.Controllers;
using Tekton.Api.IRepository;
using Tekton.Api.IService;
using Tekton.Api.Logic;
using Tekton.Api.Service;
using Tekton.Api.ViewModel;
using Tekton.Api.ViewModel.DTO;

namespace Tekton.Api.Test
{
    [TestClass]
    public class GetByIdTest
    {
        [TestMethod]
        public async Task IfOk()
        {
            #region Arrange
            var producto = new ProductResponseDTO()
            { 
                ProductId = 1,
                Name = "MOUSE",
                Status = true,
                StatusName = "ACTIVE",
                Stock = 10,
                Description = "MOUSE INALAMBRICO",
                Price = 20,
                Discount = 10,
                FinalPrice = 18
            };
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetById(It.IsAny<long>())).ReturnsAsync(producto);

            var productLogic = new ProductLogic(mockProductRepository.Object);

            var mockLogger = new Mock<ILogger<ProductService>>();

            var mockStatusService = new Mock<IStatusService>();
            var status = new List<StatusViewModel>();
            status.Add(new StatusViewModel { StatusId = 0, Name = "INACTIVE" });
            status.Add(new StatusViewModel { StatusId = 1, Name = "ACTIVE" });
            mockStatusService.Setup(x => x.GetStatus()).Returns(status);

            var mockConfiguration = new Mock<IConfiguration>();
            string keyAppSettings = "appSettings:UrlMockapiService";
            string url = "https://665f41e71e9017dc16f381c8.mockapi.io/GetDiscount/";
            mockConfiguration.Setup(x => x.GetSection(keyAppSettings).Value).Returns(url);

            var productService = new ProductService(productLogic, mockLogger.Object, mockStatusService.Object, mockConfiguration.Object);
            var productController = new ProductController(productService);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(x => x.Items["IpAdress"]).Returns("172.20.2.111");
            mockHttpContext.SetupGet(x => x.Items["IdLog"]).Returns(Guid.NewGuid());
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            productController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            long productId = 1;
            #endregion

            #region Act
            var result = await productController.GetById(productId);
            RespuestaViewModel<ProductResponseDTO> respuesta = null;
            if (result is ObjectResult objectResult && objectResult.Value is RespuestaViewModel<ProductResponseDTO> respuestaResult)
            {
                respuesta = respuestaResult;
            }
            #endregion

            #region Assert
            Assert.IsNotNull(respuesta);
            Assert.IsNotNull(respuesta.DataResult);
            Assert.IsNotNull(respuesta.Resultado);
            Assert.IsFalse(respuesta.Resultado.ErrorValidacion);
            Assert.IsNull(respuesta.Resultado.Mensajes);
            Assert.IsTrue(respuesta.Resultado.Ok);
            Assert.AreEqual(200, respuesta.Resultado.StatusCode);
            #endregion
        }

        [TestMethod]
        public async Task IfNotFound()
        {
            #region Arrange
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetById(It.IsAny<long>())).ReturnsAsync((ProductResponseDTO)null);

            var productLogic = new ProductLogic(mockProductRepository.Object);

            var mockLogger = new Mock<ILogger<ProductService>>();

            var mockStatusService = new Mock<IStatusService>();
            var status = new List<StatusViewModel>();
            status.Add(new StatusViewModel { StatusId = 0, Name = "INACTIVE" });
            status.Add(new StatusViewModel { StatusId = 1, Name = "ACTIVE" });
            mockStatusService.Setup(x => x.GetStatus()).Returns(status);

            var mockConfiguration = new Mock<IConfiguration>();
            string keyAppSettings = "appSettings:UrlMockapiService";
            string url = "https://665f41e71e9017dc16f381c8.mockapi.io/GetDiscount/";
            mockConfiguration.Setup(x => x.GetSection(keyAppSettings).Value).Returns(url);

            var productService = new ProductService(productLogic, mockLogger.Object, mockStatusService.Object, mockConfiguration.Object);
            var productController = new ProductController(productService);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(x => x.Items["IpAdress"]).Returns("172.20.2.111");
            mockHttpContext.SetupGet(x => x.Items["IdLog"]).Returns(Guid.NewGuid());
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            productController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            long productId = 999;
            #endregion

            #region Act
            var result = await productController.GetById(productId);
            RespuestaViewModel<ProductResponseDTO> respuesta = null;
            if (result is ObjectResult objectResult && objectResult.Value is RespuestaViewModel<ProductResponseDTO> respuestaResult)
            {
                respuesta = respuestaResult;
            }
            #endregion

            #region Assert
            Assert.IsNotNull(respuesta);
            Assert.IsNull(respuesta.DataResult);
            Assert.IsNotNull(respuesta.Resultado);
            Assert.IsFalse(respuesta.Resultado.ErrorValidacion);
            Assert.IsNotNull(respuesta.Resultado.Mensajes);
            Assert.IsTrue(respuesta.Resultado.Ok);
            Assert.AreEqual(404, respuesta.Resultado.StatusCode);
            #endregion
        }

        [TestMethod]
        public async Task IfBadRequest()
        {
            #region Arrange
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetById(It.IsAny<long>())).ReturnsAsync((ProductResponseDTO)null);

            var productLogic = new ProductLogic(mockProductRepository.Object);

            var mockLogger = new Mock<ILogger<ProductService>>();

            var mockStatusService = new Mock<IStatusService>();
            var status = new List<StatusViewModel>();
            status.Add(new StatusViewModel { StatusId = 0, Name = "INACTIVE" });
            status.Add(new StatusViewModel { StatusId = 1, Name = "ACTIVE" });
            mockStatusService.Setup(x => x.GetStatus()).Returns(status);

            var mockConfiguration = new Mock<IConfiguration>();
            string keyAppSettings = "appSettings:UrlMockapiService";
            string url = "https://665f41e71e9017dc16f381c8.mockapi.io/GetDiscount/";
            mockConfiguration.Setup(x => x.GetSection(keyAppSettings).Value).Returns(url);

            var productService = new ProductService(productLogic, mockLogger.Object, mockStatusService.Object, mockConfiguration.Object);
            var productController = new ProductController(productService);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(x => x.Items["IpAdress"]).Returns("172.20.2.111");
            mockHttpContext.SetupGet(x => x.Items["IdLog"]).Returns(Guid.NewGuid());
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            productController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            long productId = 0;
            #endregion

            #region Act
            var result = await productController.GetById(productId);
            RespuestaViewModel<ProductResponseDTO> respuesta = null;
            if (result is ObjectResult objectResult && objectResult.Value is RespuestaViewModel<ProductResponseDTO> respuestaResult)
            {
                respuesta = respuestaResult;
            }
            #endregion

            #region Assert
            Assert.IsNotNull(respuesta);
            Assert.IsNull(respuesta.DataResult);
            Assert.IsNotNull(respuesta.Resultado);
            Assert.IsTrue(respuesta.Resultado.ErrorValidacion);
            Assert.IsNotNull(respuesta.Resultado.Mensajes);
            Assert.IsTrue(respuesta.Resultado.Ok);
            Assert.AreEqual(400, respuesta.Resultado.StatusCode);
            #endregion
        }

        [TestMethod]
        public async Task IfInternalServerError()
        {
            #region Arrange
            var producto = new ProductResponseDTO()
            {
                ProductId = 1,
                Name = "MOUSE",
                Status = true,
                StatusName = "ACTIVE",
                Stock = 10,
                Description = "MOUSE INALAMBRICO",
                Price = 20,
                Discount = 10,
                FinalPrice = 18
            };
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetById(It.IsAny<long>())).ReturnsAsync(producto);
 
            var productLogic = new ProductLogic(mockProductRepository.Object);

            var mockLogger = new Mock<ILogger<ProductService>>();

            var mockStatusService = new Mock<IStatusService>();
            var status = new List<StatusViewModel>();
            mockStatusService.Setup(x => x.GetStatus()).Returns(status);

            var mockConfiguration = new Mock<IConfiguration>();
            string keyAppSettings = "appSettings:UrlMockapiService";
            string url = "https://665f41e71e9017dc16f381c8.mockapi.io/GetDiscount/";
            mockConfiguration.Setup(x => x.GetSection(keyAppSettings).Value).Returns(url);

            var productService = new ProductService(productLogic, mockLogger.Object, mockStatusService.Object, mockConfiguration.Object);
            var productController = new ProductController(productService);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(x => x.Items["IpAdress"]).Returns("172.20.2.111");
            mockHttpContext.SetupGet(x => x.Items["IdLog"]).Returns(Guid.NewGuid());
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            productController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };


            long productId = 1;
            #endregion

            #region Act
            var result = await productController.GetById(productId);
            RespuestaViewModel<ProductResponseDTO> respuesta = null;
            if (result is ObjectResult objectResult && objectResult.Value is RespuestaViewModel<ProductResponseDTO> respuestaResult)
            {
                respuesta = respuestaResult;
            }
            #endregion

            #region Assert
            Assert.IsNotNull(respuesta);
            Assert.IsNull(respuesta.DataResult);
            Assert.IsNotNull(respuesta.Resultado);
            Assert.IsFalse(respuesta.Resultado.ErrorValidacion);
            Assert.IsNotNull(respuesta.Resultado.Mensajes);
            Assert.IsFalse(respuesta.Resultado.Ok);
            Assert.AreEqual(500, respuesta.Resultado.StatusCode);
            #endregion
        }
    }
}
