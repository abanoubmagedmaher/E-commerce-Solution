using AutoMapper;
using Core.Entities;
using E_commerce.Api.Dtos;

namespace E_commerce.Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>();
        }
    }
}
