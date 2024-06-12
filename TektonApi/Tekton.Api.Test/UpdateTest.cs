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
    public class UpdateTest
    {
        [TestMethod]
        public async Task IfOK()
        {
            #region Arrange
            var productoRsponse = new ProductResponseDTO()
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
            mockProductRepository.Setup(x => x.Update(It.IsAny<ProductRequestUpdateDTO>(), It.IsAny<string>())).ReturnsAsync(true);
            mockProductRepository.Setup(x => x.GetById(It.IsAny<long>())).ReturnsAsync(productoRsponse);

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

            var producto = new ProductRequestUpdateDTO()
            {
                ProductId = 1,
                Name = "MOUSE",
                Status = true,
                Stock = 10,
                Description = "MOUSE INALAMBRICO",
                Price = 20
            };
            #endregion

            #region Act
            var result = await productController.Update(producto);
            RespuestaViewModel<bool> respuesta = null;
            if (result is ObjectResult objectResult && objectResult.Value is RespuestaViewModel<bool> respuestaResult)
            {
                respuesta = respuestaResult;
            }
            #endregion

            #region Assert
            Assert.IsNotNull(respuesta);
            Assert.IsTrue(respuesta.DataResult);
            Assert.IsNotNull(respuesta.Resultado);
            Assert.IsFalse(respuesta.Resultado.ErrorValidacion);
            Assert.IsNull(respuesta.Resultado.Mensajes);
            Assert.IsTrue(respuesta.Resultado.Ok);
            Assert.AreEqual(200, respuesta.Resultado.StatusCode);
            #endregion
        }

        [TestMethod]
        public async Task IfBadRequest()
        {
            #region Arrange   
            var productoRsponse = new ProductResponseDTO()
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
            mockProductRepository.Setup(x => x.Update(It.IsAny<ProductRequestUpdateDTO>(), It.IsAny<string>())).ReturnsAsync(false);
            mockProductRepository.Setup(x => x.GetById(It.IsAny<long>())).ReturnsAsync(productoRsponse);

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

            var producto = new ProductRequestUpdateDTO()
            {
                Name = "MOUSE",
                Stock = -1,
                Description = "MOUSE INALAMBRICO",
                Price = 20
            };
            #endregion

            #region Act
            var result = await productController.Update(producto);
            RespuestaViewModel<bool> respuesta = null;
            if (result is ObjectResult objectResult && objectResult.Value is RespuestaViewModel<bool> respuestaResult)
            {
                respuesta = respuestaResult;
            }
            #endregion

            #region Assert
            Assert.IsNotNull(respuesta);
            Assert.IsFalse(respuesta.DataResult);
            Assert.IsNotNull(respuesta.Resultado);
            Assert.IsTrue(respuesta.Resultado.ErrorValidacion);
            Assert.IsNotNull(respuesta.Resultado.Mensajes);
            Assert.IsTrue(respuesta.Resultado.Ok);
            Assert.AreEqual(400, respuesta.Resultado.StatusCode);
            #endregion
        }

        [TestMethod]
        public async Task IfNotFound()
        {
            #region Arrange            
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.Update(It.IsAny<ProductRequestUpdateDTO>(), It.IsAny<string>())).ReturnsAsync(true);
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

            var producto = new ProductRequestUpdateDTO()
            {
                ProductId = 999,
                Name = "MOUSE",
                Status = true,
                Stock = 10,
                Description = "MOUSE INALAMBRICO",
                Price = 20
            };
            #endregion

            #region Act
            var result = await productController.Update(producto);
            RespuestaViewModel<bool> respuesta = null;
            if (result is ObjectResult objectResult && objectResult.Value is RespuestaViewModel<bool> respuestaResult)
            {
                respuesta = respuestaResult;
            }
            #endregion

            #region Assert
            Assert.IsNotNull(respuesta);
            Assert.IsFalse(respuesta.DataResult);
            Assert.IsNotNull(respuesta.Resultado);
            Assert.IsTrue(respuesta.Resultado.ErrorValidacion);
            Assert.IsNotNull(respuesta.Resultado.Mensajes);
            Assert.IsTrue(respuesta.Resultado.Ok);
            Assert.AreEqual(404, respuesta.Resultado.StatusCode);
            #endregion
        }
    }
}
