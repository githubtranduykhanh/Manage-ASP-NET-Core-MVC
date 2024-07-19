using AutoMapper;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Caterory;


namespace ECommerceMVC.UI.Areas.Admin.AutoMapperProfile.Caterory
{
    public class CateroryCreateProfile : Profile
    {
        public CateroryCreateProfile()
        {
            CreateMap<CreateCateroryVM, DbCategory>().ReverseMap();
        }
    }
}
