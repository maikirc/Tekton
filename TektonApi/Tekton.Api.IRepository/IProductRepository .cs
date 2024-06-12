using Tekton.Api.ViewModel.DTO;

namespace Tekton.Api.IRepository
{
    public interface IProductRepository
    {
        Task<ProductResponseDTO> GetById(long productId);
        Task<long> Insert(ProductRequestInsertDTO product, string ipAdress);
        Task<bool> Update(ProductRequestUpdateDTO product, string ipAdress);
    }
}