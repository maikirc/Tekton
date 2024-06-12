using Tekton.Api.ViewModel;

namespace Tekton.Api.IService
{
    public interface IStatusService
    {
        public List<StatusViewModel> GetStatus();
    }
}