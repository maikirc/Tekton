using Tekton.Api.IRepository;
using Tekton.Api.ViewModel.DTO;

namespace Tekton.Api.Logic
{
    public class ProductLogic
    {
        private readonly IProductRepository _productRepository;

        public ProductLogic(IProductRepository productRepository)
         => _productRepository = productRepository ?? throw new ArgumentNullException("productRepository");

        public async Task<ProductResponseDTO> GetById(long productId)
        => await _productRepository.GetById(productId);

        public async Task<long> Insert(ProductRequestInsertDTO product, string ipAdress)
        => await _productRepository.Insert(product, ipAdress);

        public async Task<bool> Update(ProductRequestUpdateDTO product, string ipAdress)
        => await _productRepository.Update(product, ipAdress);

    }
}