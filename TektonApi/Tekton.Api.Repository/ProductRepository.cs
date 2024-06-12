using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tekton.Api.Entities;
using Tekton.Api.IRepository;
using Tekton.Api.ViewModel.DTO;

namespace Tekton.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// El proveedor de servicios.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ProductRepository"/>.
        /// </summary>
        /// <param name="serviceProvider">The service providerervicios.</param>
        public ProductRepository(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public async Task<ProductResponseDTO> GetById(long productId)
        {
            using var db = _serviceProvider.GetService<Data.TektonContext>();
            var r = await db.Products.FindAsync(productId);

            if (r != null && r.Status)
                return _mapper.Map<ProductResponseDTO>(r);
            else
                return null;
        }

        public async Task<long> Insert(ProductRequestInsertDTO product, string ipAdress)
        {
            using var db = _serviceProvider.GetService<Data.TektonContext>();
            Product productEntity = _mapper.Map<Product>(product);
            productEntity.Status = true;
            productEntity.CreationDate = DateTime.Now;
            productEntity.CreationUser = ipAdress;
            productEntity.LastModificationDate = productEntity.CreationDate;
            productEntity.LastModificationUser = ipAdress;

            var r = await db.Products.AddAsync(productEntity);

            if (db.SaveChanges() == 1)
                return r.Entity.ProductId;
            else
                return -1;
        }

        public async Task<bool> Update(ProductRequestUpdateDTO product, string ipAdress)
        {
            using var db = _serviceProvider.GetService<Data.TektonContext>();
            Product productCopy = await db.Products.FindAsync(product.ProductId);

            if (productCopy == null)
                return false;
                
            productCopy.Name = product.Name;
            productCopy.Status = product.Status;
            productCopy.Stock = product.Stock;
            productCopy.Description = product.Description;
            productCopy.Price = product.Price;
            productCopy.LastModificationDate = DateTime.Now;
            productCopy.LastModificationUser = ipAdress;

            if (db.SaveChanges() == 1)
                return true;
            else
                return false;
        }
    }
}
