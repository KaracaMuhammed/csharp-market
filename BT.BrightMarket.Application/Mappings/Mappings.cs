using AutoMapper;
using BT.BrightMarket.Domain.DTOs;
using BT.BrightMarket.Domain.Models.Products;

namespace BT.BrightMarket.Application.Mappings
{
    public class Mappings : Profile
    {
        public Mappings() 
        {

            CreateMap<ProductDTO.PostProductObject, Product>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        }
    }
}
