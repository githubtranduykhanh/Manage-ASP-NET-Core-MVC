using AutoMapper;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;

namespace ECommerceMVC.AutoMapperProfile
{
    public class UserVMDbUserProfile : Profile
    {
        public UserVMDbUserProfile()
        {
            // Từ DbUser sang UserVM
            CreateMap<DbUser, UserVM>().ReverseMap();
               
        }
    }
}
