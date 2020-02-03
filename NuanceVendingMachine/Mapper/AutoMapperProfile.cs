using AutoMapper;
using NuanceVendingMachine.Dto;
using NuanceVendingMachine.Models;

namespace NuanceVendingMachine.Mapper
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Name, opt =>
                 {
                     opt.MapFrom(src => src.Title);
                 })
                 .ForMember(dest => dest.AvailableQuantity, opt =>
                 {
                     opt.MapFrom(src => src.Stock);
                 })
                 .ForMember(dest => dest.Price, opt =>
                 {
                     opt.MapFrom(src => 
                    
                     src.ProductType.Price
                     );
                 })
                 .ForMember(dest => dest.ProductType, opt =>
                 {
                     opt.MapFrom(src => src.ProductType.TypeName);
                 });

            CreateMap<ProductDto, Product>()
               .ForMember(dest => dest.Title, opt =>
               {
                   opt.MapFrom(src => src.Name);
               })
                .ForMember(dest => dest.Stock, opt =>
                {
                    opt.MapFrom(src => src.AvailableQuantity);
                });

                CreateMap<ProductType, ProductTypeDto>()
              .ForMember(dest => dest.Name, opt =>
              {
                  opt.MapFrom(src => src.TypeName);
              })
               .ForMember(dest => dest.Price, opt =>
               {
                   opt.MapFrom(src => src.Price);
               })
               .ForMember(dest => dest.Id, opt =>
               {
                   opt.MapFrom(src => src.ProductTypeId);
               }).ReverseMap();

        }
    }
}
