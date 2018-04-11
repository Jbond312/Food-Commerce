using AutoMapper;
using Foods.Service.Repository.Users.Entities;
using Users.Service.Business.Entities;

namespace Users.Service.Business.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}