using AutoMapper;
using Shared.Data.Exceptions;
using Shared.Helpers;
using Shared.Services.RabbitMQ;
using Shared.Services.App;
using Shared.Data.Models;
using IdentityAPI.Data.DTO.User;
using IdentityAPI.DTO.User;
using IdentityAPI.Repositories;
using IdentityAPI.Data.Specifications;
using IdentityAPI.Helpers;
using IdentityAPI.Entities;

namespace IdentityAPI.Services
{
    public interface IUsersService : IService
    {
        Task<LoginResponseDTO> Login(LoginUserRequestDTO loginUser);
        Task<bool> Register(RegisterUserRequestDTO registerUser);
        Task<LoginResponseDTO> RefreshToken(RefreshTokenRequestDTO refreshToken);
        Task<bool> BlockUser(BlockUserDTO request);
        Task<bool> ChangePassword(ChangePasswordRequestDTO request);
        Task<bool> ForgotPassword(ForgotPasswordRequestDTO request);
        Task<bool> ActivateAccount(ActivateAccountRequestDTO request);
    }

    public class UsersService : BaseService<IUsersService>, IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IAuthService _authService;

        public UsersService(ILogger<IUsersService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, RabbitMQProducerService rabbitMQProducerService, IServiceProvider serviceProvider) : base(logger, mapper, httpContextAccessor, rabbitMQProducerService, serviceProvider)
        {
            _usersRepository = serviceProvider.GetRequiredService<IUsersRepository>();
            _authService = serviceProvider.GetRequiredService<IAuthService>();
        }

        public async Task<LoginResponseDTO> Login(LoginUserRequestDTO loginUser)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var spec = new UserWithRolesAndPermissions(u => u.Email == loginUser.Email);
                var usersByEmail = await _usersRepository.Get(spec);
                usersByEmail = usersByEmail.ToList();
                if (usersByEmail.Count() != 1)
                    throw new AppException(ExceptionCodes.LoginUsernameNotFound);

                var matchingUser =
                    usersByEmail.Where(u => u.Passwords.GetLatest().Equals(loginUser.Password)).FirstOrDefault();
                if (matchingUser == null)
                    throw new AppException(ExceptionCodes.LoginWrongPassword);

                if (matchingUser.Blocks.Any(x => !x.Deactivated && (x.To > DateTime.Now || x.Pernament)))
                    throw new AppException(ExceptionCodes.LoginUserBlocked);

                var result = _authService.GenerateToken(matchingUser);

                if (result == null)
                    throw new AppException(ExceptionCodes.CorruptedToken);


                matchingUser.RefreshToken = result.RefreshToken;
                await _usersRepository.Update(matchingUser);

                result.User = _mapper.Map<GetUserDTO>(matchingUser);
                
                var mailMessage = new RabbitMQResponse<MailMessage>();
                await _rabbitMQProducerService.SendMessage(mailMessage, "SendMail");

                return result;
            }, _logger);
        }

        public async Task<bool> Register(RegisterUserRequestDTO registerUser)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var matchingEmail = await _usersRepository.Count(u => u.Email.Equals(registerUser.Email));
                if (matchingEmail > 0)
                    throw new AppException(ExceptionCodes.RegisterEmailFound);

                var newUser = new User()
                {
                    Passwords = new List<Password>() {
                        new Password {
                            CreatedDate = DateTime.Now,
                            Value = registerUser.Password
                        }
                    },
                    Email = registerUser.Email,
                    ActivationCode = StringHelper.GenerateRandomPassword(5)
                };
                try
                {
                    await _usersRepository.Add(newUser);
                }
                catch (Exception ex)
                {
                    throw new AppException(ExceptionCodes.DatabaseError);
                }

                ///send mail with activation code

                return true;
            }, _logger);
        }

        public async Task<LoginResponseDTO> RefreshToken(RefreshTokenRequestDTO refreshToken)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var newToken = await _authService.RefreshTokenAsync(refreshToken.RefreshToken, refreshToken.UserId.ToString());
                if (newToken == null)
                    throw new AppException(ExceptionCodes.CorruptedToken);

                return newToken;
            }, _logger);
        }

        public async Task<bool> BlockUser(BlockUserDTO request)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var user = (await _usersRepository.Get(u => u.Id.Equals(request.UserId))).FirstOrDefault();
                if (user == null)
                    throw new AppException(ExceptionCodes.LoginUsernameNotFound);

                user.Blocks.Append(_mapper.Map<Block>(request));

                return await _usersRepository.Update(user);
            }, _logger);
        }

        public async Task<bool> ChangePassword(ChangePasswordRequestDTO request)
        {
            return await ExceptionHandler.Handle(async () =>
            {

                var id = GetClaim("Id");
                if (id == null)
                    throw new AppException(ExceptionCodes.CorruptedToken);

                var user = (await _usersRepository.Get(u => u.Id.Equals(Guid.Parse(id)))).FirstOrDefault();
                if (user == null)
                    throw new AppException(ExceptionCodes.LoginUsernameNotFound);

                var password = user.Passwords.OrderByDescending(x => x.CreatedDate).First();
                if (!password.Value.ToLower().Equals(request.OldPassword.ToLower()))
                    throw new AppException(ExceptionCodes.LoginWrongPassword);

                var usedPassword = user.Passwords.FirstOrDefault(x => x.Value.ToLower().Equals(request.NewPassword.ToLower()));
                if (usedPassword != null)
                {
                    if (usedPassword.CreatedDate >= DateTime.Now.AddMonths(-6))
                    {
                        throw new AppException(ExceptionCodes.PasswordAlreadyUsed);
                    }
                    else
                    {
                        usedPassword.CreatedDate = DateTime.Now;
                    }
                }
                else
                {
                    var newPassword = new Password() { CreatedDate = DateTime.Now, Value = request.NewPassword };
                    user.Passwords.Append(newPassword);
                }
                return await _usersRepository.Update(user);
            }, _logger);
        }

        public async Task<bool> ForgotPassword(ForgotPasswordRequestDTO request)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var foundByEmail = (await _usersRepository.Get(x => x.Email.ToLower().Equals(request.Email.ToLower()))).FirstOrDefault();
                if(foundByEmail == null)
                    throw new AppException(ExceptionCodes.LoginUsernameNotFound);

                foundByEmail.Passwords.Add(new Password
                {
                    CreatedDate = DateTime.Now,
                    Value = StringHelper.GenerateRandomPassword(10),
                    UserId = foundByEmail.Id
                });

                ///send mail about password change

                return await _usersRepository.Update(foundByEmail);
            }, _logger);
        }

        public async Task<bool> ActivateAccount(ActivateAccountRequestDTO request)
        {
            return await ExceptionHandler.Handle(async () =>
            {
                var user = (await _usersRepository.Get(x => x.Email.Compare(request.Email))).FirstOrDefault();
                if (user == null)
                    throw new AppException(ExceptionCodes.UserNotFound);
                if (user.ActivationCode.ToLower().Equals(request.ActivationCode.ToLower()))
                    throw new AppException(ExceptionCodes.WrongActivationCode);

                user.Activated = true;
                return await _usersRepository.Update(user);
            }, _logger);
        }
    }
}
