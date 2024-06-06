using AutoMapper;
using ECommerceMVC.ViewModels;

namespace ECommerceMVC.AutoMapperProfile.User
{
    public class UserInformationcClient : Profile
    {
        public UserInformationcClient()
        {
            CreateMap<UserVM, UserCurrentVM>().ReverseMap();
        }
    }
}
