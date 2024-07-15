using AutoMapper;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using ECommerceMVC.ViewModels.Caterory;

namespace ECommerceMVC.AutoMapperProfile.Caterory
{
    public class CateroryCreateProfile : Profile
    {
        public CateroryCreateProfile()
        {
            CreateMap<CreateCategoryVM, DbCategory>().ReverseMap();
        }
    }
}
