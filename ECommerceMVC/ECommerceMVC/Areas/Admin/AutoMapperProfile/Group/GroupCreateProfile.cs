using AutoMapper;
using ECommerceMVC.Domain.Entities;
using ECommerceMVC.UI.Areas.Admin.ViewModels.Group;


namespace ECommerceMVC.UI.Areas.Admin.AutoMapperProfile.Group
{
    public class GroupCreateProfile : Profile
    {
        public GroupCreateProfile()
        {
            CreateMap<CreateGroupVM, DbGroup>().ReverseMap();
        }
    }
}
