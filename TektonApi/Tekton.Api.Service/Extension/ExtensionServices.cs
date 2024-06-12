using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Tekton.Api.Data;
using Tekton.Api.IRepository;
using Tekton.Api.IService;
using Tekton.Api.Logic;
using Tekton.Api.Repository;

namespace Tekton.Api.Service.Extension
{
    public static class ExtensionServices
    {
        public static IServiceCollection AddProductService([NotNullAttribute] this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ProductLogic, ProductLogic>();

            services.AddDbContext<TektonContext>(ServiceLifetime.Transient);

            return services;
        }
    }
}
