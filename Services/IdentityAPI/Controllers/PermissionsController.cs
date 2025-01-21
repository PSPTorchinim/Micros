using IdentityAPI.DTO.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityAPI.Services;
using Shared.Services.App;

namespace IdentityAPI.Controllers
{
    public class PermissionsController : BaseController<PermissionsController>
    {
        private IPermissionsService _permissionsService {get;set;}

        public PermissionsController(ILogger<PermissionsController> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
            _permissionsService = serviceProvider.GetRequiredService<IPermissionsService>();
        }
        
        [HttpGet]
        [Authorize(Roles = "GetPermissions, Full")]
        public async Task<IActionResult> Get(){
            return await Handle(async () => await _permissionsService.GetPermissions());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "GetPermissions, Full")]
        public async Task<IActionResult> Get(Guid id){
            return await Handle(async () => await _permissionsService.GetPermission(id));
        }

        [HttpPost()]
        [Authorize(Roles = "AddPermission, Full")]
        public async Task<IActionResult> Post(AddPermissionDTO request)
        {
            return await Handle(async () => await _permissionsService.AddPermission(request));
        }
    }
}