using Tekton.Api.ViewModel;

namespace Tekton.Api.IService
{
    public interface IDiscountService
    {
        Task<decimal> GetDiscount(long productId);
    }
}