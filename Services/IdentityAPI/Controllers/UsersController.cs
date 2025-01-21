using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityAPI.DTO.User;
using Shared.Services.App;
using IdentityAPI.Data.DTO.User;
using IdentityAPI.Services;

namespace IdentityAPI.Controllers
{
    public class UsersController : BaseController<UsersController>
    {
        private readonly IUsersService _usersService;

        public UsersController(ILogger<UsersController> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
            _usersService = serviceProvider.GetRequiredService<IUsersService>();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserRequestDTO loginUser)
        {
            return await Handle(async () => await _usersService.Login(loginUser));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserRequestDTO register)
        {
            return await Handle(async () => await _usersService.Register(register));
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO refreshToken)
        {
            return await Handle(async () => await _usersService.RefreshToken(refreshToken));
        }

        [Authorize(Roles = "Block, Full")]
        [HttpPut("Block")]
        public async Task<IActionResult> BlockUser(BlockUserDTO request)
        {
            return await Handle(async () => await _usersService.BlockUser(request));
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDTO request)
        {
            return await Handle(async () => await _usersService.ChangePassword(request));
        }

        [AllowAnonymous]
        [HttpPut("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO request)
        {
            return await Handle(async () => await _usersService.ForgotPassword(request));
        }

        [HttpPut("ActivateAccount")]
        public async Task<IActionResult> ActivateAccount(ActivateAccountRequestDTO request)
        {
            return await Handle(async () => await _usersService.ActivateAccount(request));
        }
    }
}
