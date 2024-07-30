using AutoMapper;
using ECommerceMVC.Application.Dtos.NewCategories;
using ECommerceMVC.Application.Dtos.Order;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Caterory;
using ECommerceMVC.UI.Areas.Admin.ViewModels.NewCategories;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Order;

namespace ECommerceMVC.UI.Areas.Admin.AutoMapperProfile
{
    public class AdminMapper : Profile
    {
        public AdminMapper()
        {
            CreateMap<NewCategoriesModel, NewCategoryVM>()
            .ForMember(dest => dest.Image, opt => opt.Ignore()).ReverseMap();

            CreateMap<OrderModel, OrderVM>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.IdUserNavigation)).ReverseMap();
        }
    }
}
