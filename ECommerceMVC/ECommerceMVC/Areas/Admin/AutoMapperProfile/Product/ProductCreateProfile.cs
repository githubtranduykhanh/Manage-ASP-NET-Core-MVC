using AutoMapper;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Product;


namespace ECommerceMVC.UI.Areas.Admin.AutoMapperProfile.Product
{
    public class ProductCreateProfile : Profile
    {
        public ProductCreateProfile()
        {
            CreateMap<ProductCreateVM, DbProduct>().ReverseMap();
        }
    }
}
