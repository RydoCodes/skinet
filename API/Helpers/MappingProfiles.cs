using API.Dtos;
using API.Dtos.BasketDTOs;
using API.Dtos.IdentityDTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDto>()
                        .ForMember(d=> d.ProductBrand,o=>o.MapFrom(s=> s.ProductBrand.Name)) //MapFrom<TSourceMember>(Expression<Func<TSource, TSourceMember>> mapExpression)
                        .ForMember(d=> d.ProductType,o=>o.MapFrom(o=> o.ProductType.Name)) // o,s : IMemberConfigurationExpression<TSource, TDestination, TMember> where Tmember is string
                        .ForMember(d=> d.PictureUrl,o=> o.MapFrom<ProductUrlResolver>());

            // GetAddress of User - .Net Core Identity
            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();


        }
    }
}