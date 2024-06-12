using AutoMapper;
using Tekton.Api.Entities;
using Tekton.Api.ViewModel.DTO;

namespace Tekton.Api.Repository
{
    public class AutomapperProductProfile : Profile
    {
        public AutomapperProductProfile()
        {
            CreateMap<Product, ProductResponseDTO>();
            CreateMap<ProductRequestInsertDTO, Product>();
        }
    }
}
