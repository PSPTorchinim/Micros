using AutoMapper;
using Shared.Data.Exceptions;
using Shared.Services.RabbitMQ;
using Shared.Services.App;
using IdentityAPI.Entities;
using IdentityAPI.DTO.Role;
using IdentityAPI.Repositories;
using IdentityAPI.Specifications;

namespace IdentityAPI.Services
{
    public interface IRolesService : IService
    {
        Task<List<Role>> GetRoles();
        Task<GetRoleDTO> GetRole(Guid id);
        Task<bool> AddRole(AddRoleRequest request);
        Task<bool> EditRole(Guid id, AddRoleRequest request);
        Task<bool> DeleteRole(Guid id);
    }

    public class RolesService : BaseService<IRolesService>, IRolesService
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IPermissionsRepository _permissionsRepository;

        public RolesService(ILogger<IRolesService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, RabbitMQProducerService rabbitMQProducerService, IServiceProvider serviceProvider) : base(logger, mapper, httpContextAccessor, rabbitMQProducerService, serviceProvider)
        {
            _rolesRepository = serviceProvider.GetRequiredService<IRolesRepository>();
            _permissionsRepository = serviceProvider.GetRequiredService<IPermissionsRepository>();
        }

        public async Task<List<Role>> GetRoles()
        {
            return await ExceptionHandler.Handle(async () =>
            {
                return (await _rolesRepository.Get()).ToList();
            }, _logger);
        }

        public async Task<GetRoleDTO> GetRole(Guid id)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var spec = new RolePermissionsSpec(x => x.Id.Equals(id));
                var req = await _rolesRepository.Get(spec);
                return _mapper.Map<GetRoleDTO>(req);
            }, _logger);
        }

        public async Task<bool> AddRole(AddRoleRequest request)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var foundByName = await _rolesRepository.Exists(role => role.Name == request.Name);
                if (foundByName)
                    throw new AppException(ExceptionCodes.AddRoleExists);

                var toAdd = new Role
                {
                    Name = request.Name,
                    Description = request.Description,
                    Permissions = await _permissionsRepository.Get(p => request.Permissions.Contains(p.Id))
                };

                return await _rolesRepository.Add(toAdd);
            }, _logger);
        }

        public async Task<bool> EditRole(Guid id, AddRoleRequest request)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var foundByName = (await _rolesRepository.Get(role => role.Id == id)).FirstOrDefault();
                if (foundByName == null)
                    throw new AppException(ExceptionCodes.RoleNotExists);

                foundByName.Name = request.Name;
                foundByName.Description = request.Description;
                foundByName.Permissions = await _permissionsRepository.Get(p => request.Permissions.Contains(p.Id));

                return await _rolesRepository.Update(foundByName);
            }, _logger);
        }

        public async Task<bool> DeleteRole(Guid id)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var foundByName = (await _rolesRepository.Get(role => role.Id == id)).FirstOrDefault();
                if (foundByName == null)
                    throw new AppException(ExceptionCodes.RoleNotExists);

                if (foundByName.Users.Any())
                    throw new AppException(ExceptionCodes.RoleHasUsers);

                return await _rolesRepository.Delete(foundByName);
            }, _logger);
        }
    }
}
