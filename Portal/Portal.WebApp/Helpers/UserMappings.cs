using AutoMapper;
using Portal.Domain.DTOs;
using Portal.Domain.Models;
using Portal.WebApp.Models.UserViewModels;

namespace Portal.WebApp.Helpers
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<UserLoginViewModel, UserLoginDTO>();
            CreateMap<UserRegisterViewModel, User>().AfterMap((src, dest) =>
            {
                dest.UserName = src.Email;
                dest.AccessLevel = 0;
            });
            CreateMap<User, UserProfileViewModel>();
        }
    }
}
