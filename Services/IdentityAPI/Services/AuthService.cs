using AutoMapper;
using Shared.Data.Exceptions;
using IdentityAPI.DTO.User;
using IdentityAPI.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityAPI.Repositories;
using Shared.Services.RabbitMQ;
using Shared.Services.App;

namespace IdentityAPI.Services
{
    public interface IAuthService : IService
    {
        LoginResponseDTO GenerateToken(User user);
        Task<LoginResponseDTO> RefreshTokenAsync(string token, string UserId);
        bool ValidateToken(string authToken, bool isInvited = false);
    }

    public class AuthService : BaseService<IAuthService>, IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _usersRepository;

        public AuthService(IConfiguration configuration, ILogger<IAuthService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, RabbitMQProducerService rabbitMQProducerService, IServiceProvider serviceProvider) : base(logger, mapper, httpContextAccessor, rabbitMQProducerService, serviceProvider)
        {
            _configuration = configuration;
            _usersRepository = serviceProvider.GetRequiredService<IUsersRepository>();
        }

        private List<Claim> GenerateClaims(User user)
        {
            return ExceptionHandler.Handle(() =>
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email)
                };
                user.Roles.SelectMany(r => r.Permissions).ToList().ForEach(p =>
                {
                    claims.Add(new Claim(ClaimTypes.Role, p.ToString()));
                });
                return claims;
            }, _logger);
        }

        private string GenerateRefresh()
        {
            return ExceptionHandler.Handle(() =>
            {
                var random = new byte[32];
                using (var generator = RandomNumberGenerator.Create())
                {
                    generator.GetBytes(random);
                    return Convert.ToBase64String(random);
                }
            }, _logger);
        }

        public LoginResponseDTO GenerateToken(User user)
        {
            return ExceptionHandler.Handle(() =>
            {
                var key = _configuration.GetSection("TokenConfiguration").GetValue<string>("Key");
                var issuer = _configuration.GetSection("TokenConfiguration").GetValue<string>("Issuer");
                var audience = _configuration.GetSection("TokenConfiguration").GetValue<string>("Audience");
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var time = _configuration.GetSection("TokenConfiguration").GetValue<long>("TokenExpireTime");
                var expires = DateTime.Now.AddSeconds(time);
                var claims = GenerateClaims(user);
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    expires: expires,
                    claims: claims,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                var tokenResponse = new LoginResponseDTO();
                tokenResponse.Token = new JwtSecurityTokenHandler().WriteToken(token);
                tokenResponse.RefreshToken = GenerateRefresh();
                tokenResponse.UserId = user.Id;
                return tokenResponse;
            }, _logger);
        }

        public async Task<LoginResponseDTO> RefreshTokenAsync(string token, string UserId)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                if (UserId == null)
                    throw new AppException(ExceptionCodes.CorruptedToken);

                var user = (await _usersRepository.Get(x => x.Id.ToString().Equals(UserId))).FirstOrDefault();
                if (user == null || user.RefreshToken == null || !user.RefreshToken.Equals(token))
                    throw new AppException(ExceptionCodes.CorruptedToken);

                return GenerateToken(user);
            }, _logger);
        }

        public bool ValidateToken(string authToken, bool isInvited = false)
        {
            return ExceptionHandler.Handle(() =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _configuration.GetSection("TokenConfiguration").GetValue<string>("Key");
                var encodedKey = Encoding.ASCII.GetBytes(key);
                var time = _configuration.GetSection("TokenConfiguration").GetValue<long>("TokenExpiration");
                tokenHandler.ValidateToken(authToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(encodedKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromSeconds(time)
                }, out SecurityToken validatedToken);
                return true;
            }, _logger);
        }
    }
}
