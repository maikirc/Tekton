using Microsoft.Extensions.Caching.Memory;
using Tekton.Api.IService;
using Tekton.Api.ViewModel;

namespace Tekton.Api.Service
{
    public class StatusService : IStatusService
    {
        private readonly IMemoryCache _memoryCache;
        public string cacheKey = "status";

        public StatusService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<StatusViewModel> GetStatus()
        {
            List<StatusViewModel> status;

            if (!_memoryCache.TryGetValue(cacheKey, out status))
            {
                status = new List<StatusViewModel>
                {
                    new StatusViewModel { StatusId = 0, Name = "Inactive" },
                    new StatusViewModel { StatusId = 1, Name = "Active" }
                };

                _memoryCache.Set(cacheKey, status,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return status;
        }
    }
}