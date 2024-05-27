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
            CreateMap<DbUser, UserVM>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.IdRoleNavigation.Name));

            // Từ UserVM sang DbUser
            CreateMap<UserVM, DbUser>()
                .ForMember(dest => dest.IdRoleNavigation, opt => opt.Ignore()); // Không ánh xạ ngược lại IdRoleNavigation
        }
    }
}
