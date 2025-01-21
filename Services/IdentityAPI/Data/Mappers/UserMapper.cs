using AutoMapper;
using IdentityAPI.DTO.User;
using IdentityAPI.Entities;

namespace Shared.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() {
            CreateMap<User, GetUserDTO>();
        }
    }
}
