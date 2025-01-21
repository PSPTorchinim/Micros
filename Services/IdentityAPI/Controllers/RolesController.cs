using IdentityAPI.DTO.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityAPI.Services;
using Shared.Services.App;

namespace IdentityAPI.Controllers
{
    public class RolesController : BaseController<RolesController>
    {

        private readonly IRolesService _rolesService;

        public RolesController(ILogger<RolesController> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
            _rolesService = serviceProvider.GetRequiredService<IRolesService>();
        }

        [HttpGet]
        [Authorize(Roles = "GetRoles, Full")]
        public async Task<IActionResult> GetRoles(){
            return await Handle(async () => await _rolesService.GetRoles());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "GetRole, Full")]
        public async Task<IActionResult> GetRole(Guid id){
            return await Handle(async () => await _rolesService.GetRole(id));
        }

        [HttpPost]
        [Authorize(Roles = "AddRole, Full")]
        public async Task<IActionResult> PostRole(AddRoleRequest request)
        {
            return await Handle(async () => await _rolesService.AddRole(request));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "EditRole, Full")]
        public async Task<IActionResult> PutRole(Guid id,[FromBody] AddRoleRequest request)
        {
            return await Handle(async () => await _rolesService.EditRole(id, request));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "DeleteRole, Full")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            return await Handle(async () => await _rolesService.DeleteRole(id));
        }
    }
}