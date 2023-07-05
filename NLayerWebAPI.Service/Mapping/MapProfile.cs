using AutoMapper;
using NLayerWebAPI.Core.DTOs;
using NLayerWebAPI.Core.Entities;

namespace NLayerWebAPI.Service.Mapping
{
	public class MapProfile : Profile
	{
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            //ProductUpdateDto'da Reverse mape gerek yok entitye çevireceğimiz için.
            CreateMap<Product, ProductsWithCategoryDto>();


        }
    }
}
