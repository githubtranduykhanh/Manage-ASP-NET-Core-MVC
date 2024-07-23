using AutoMapper;
using ECommerceMVC.Application.Dtos.NewCategories;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Caterory;
using ECommerceMVC.UI.Areas.Admin.ViewModels.NewCategories;

namespace ECommerceMVC.UI.Areas.Admin.AutoMapperProfile
{
    public class AdminMapper : Profile
    {
        public AdminMapper()
        {
            CreateMap<NewCategoriesModel, NewCategoryVM>()
            .ForMember(dest => dest.Image, opt => opt.Ignore()).ReverseMap();
        }
    }
}
