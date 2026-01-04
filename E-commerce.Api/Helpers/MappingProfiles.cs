using AutoMapper;
using Core.Entities;
using E_commerce.Api.Dtos;

namespace E_commerce.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name));
        }
    }
}
