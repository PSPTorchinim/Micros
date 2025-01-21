using AutoMapper;
using IdentityAPI.DTO.Permission;
using IdentityAPI.Entities;

namespace IdentityAPI.Mappers
{
    public class PermissionsMapper : Profile
    {
        public PermissionsMapper() {
            CreateMap<Permission, GetPermissionsDTO>();
            CreateMap<Permission, GetPermissionDTO>();
            CreateMap<AddPermissionDTO, Permission>();
        }
    }
}
