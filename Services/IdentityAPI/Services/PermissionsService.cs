using AutoMapper;
using Shared.Services.RabbitMQ;
using Shared.Services.App;
using Shared.Data.Exceptions;
using IdentityAPI.DTO.Permission;
using IdentityAPI.Repositories;
using IdentityAPI.Entities;

namespace IdentityAPI.Services
{
    public interface IPermissionsService : IService
    {
        Task<List<GetPermissionsDTO>> GetPermissions();
        Task<GetPermissionDTO> GetPermission(Guid id);
        Task<bool> AddPermission(AddPermissionDTO request);
        Task<bool> EditPermission(Guid id, EditPermissionDTO request);
        Task<bool> DeletePermission(Guid id);
    }

    public class PermissionsService : BaseService<IPermissionsService>, IPermissionsService
    {
        public PermissionsService(ILogger<IPermissionsService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, RabbitMQProducerService rabbitMQProducerService, IServiceProvider serviceProvider) : base(logger, mapper, httpContextAccessor, rabbitMQProducerService, serviceProvider)
        {
            _permissionsRepository = serviceProvider.GetRequiredService<IPermissionsRepository>();
        }

        private IPermissionsRepository _permissionsRepository { get; set; }
        

        public async Task<List<GetPermissionsDTO>> GetPermissions()
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var result = await _permissionsRepository.Get();
                return _mapper.Map<List<GetPermissionsDTO>>(result);
            }, _logger);
        }

        public async Task<bool> AddPermission(AddPermissionDTO request)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                if (await _permissionsRepository.Exists(x => x.Name.Equals(request.Name)))
                    return false;
                var req = _mapper.Map<Permission>(request);
                return await _permissionsRepository.Add(req);
            }, _logger);
        }

        public async Task<GetPermissionDTO> GetPermission(Guid id)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var req = await _permissionsRepository.Get(p => p.Id.Equals(id));
                return _mapper.Map<GetPermissionDTO>(req.FirstOrDefault());
            }, _logger);
        }

        public async Task<bool> EditPermission(Guid id, EditPermissionDTO request)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var permission = (await _permissionsRepository.Get(p => p.Id.Equals(id))).FirstOrDefault();
                permission.Name = request.Name;
                permission.Description = request.Description;
                return await _permissionsRepository.Update(permission);
            }, _logger);
        }

        public async Task<bool> DeletePermission(Guid id)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var permission = (await _permissionsRepository.Get(p => p.Id.Equals(id))).FirstOrDefault();
                if (permission != null)
                    return await _permissionsRepository.Delete(permission);
                else
                    return false;
            }, _logger);
        }
    }
}