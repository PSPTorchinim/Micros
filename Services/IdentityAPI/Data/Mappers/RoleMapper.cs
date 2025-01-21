using AutoMapper;
using IdentityAPI.DTO.Role;
using IdentityAPI.Entities;

namespace Shared.Mappers
{
    public class RoleMapper : Profile
    {
        public RoleMapper() {
            CreateMap<Role, GetRoleDTO>();
        }
    }
}
