using AutoMapper;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using ECommerceMVC.ViewModels.Group;

namespace ECommerceMVC.AutoMapperProfile.Group
{
    public class GroupCreateProfile : Profile
    {
        public GroupCreateProfile()
        {
            CreateMap<CreateGroupVM, DbGroup>().ReverseMap();
        }
    }
}
