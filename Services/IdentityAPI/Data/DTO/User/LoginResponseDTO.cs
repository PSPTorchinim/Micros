using IdentityAPI.DTO.Permission;
using IdentityAPI.DTO.Role;

namespace IdentityAPI.DTO.User
{
    public class LoginResponseDTO
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public GetUserDTO User { get; set; }
    }
}
