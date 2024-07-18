using AutoMapper;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels.Group;
using ECommerceMVC.ViewModels.Product;

namespace ECommerceMVC.AutoMapperProfile.Product
{
    public class ProductCreateProfile : Profile
    {
        public ProductCreateProfile() {
            CreateMap<ProductCreateVM, DbProduct>().ReverseMap();
        }
    }
}
