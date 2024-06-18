using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Tekton.Api.IService;
using Tekton.Api.ViewModel;

namespace Tekton.Api.Service
{
    public class DiscountService : IDiscountService
    {
        private readonly string _urlMockapiService;

        public DiscountService(IConfiguration configuration)
        {
            _urlMockapiService = configuration.GetSection("appSettings:UrlMockapiService").Value;
        }

        public async Task<decimal> GetDiscount(long productId)
        {
            #region MockApi Service 
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(_urlMockapiService + productId.ToString()).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var respuestaServ = await response.Content.ReadAsStringAsync();
                var productDiscount = JsonConvert.DeserializeObject<ProductDiscountViewModel>(respuestaServ);
                return productDiscount.Discount;
            }
            else
            {
                // Cuando el producto no tiene registrado un descuento se estima que el descuento es 0.
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return 0;
                // Cuando el servicio de descuento del producto no esta disponible se estima que el descuento es 0, porque no esta definida esa regla del negocio.
                else
                    return 0;
            }
            #endregion
        }
    }
}