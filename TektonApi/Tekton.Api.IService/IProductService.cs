using Tekton.Api.ViewModel;
using Tekton.Api.ViewModel.DTO;

namespace Tekton.Api.IService
{
    public interface IProductService
    {
        Task<RespuestaViewModel<ProductResponseDTO>> GetById(long productId, string ipAdress, string idLog);
        Task<RespuestaViewModel<long>> Insert(ProductRequestInsertDTO product, string ipAdress, string idLog);
        Task<RespuestaViewModel<bool>> Update(ProductRequestUpdateDTO product, string ipAdress, string idLog);
    }
}