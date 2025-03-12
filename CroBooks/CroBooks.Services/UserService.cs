using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Users;
using CroBooks.Services.Helpers;
using CroBooks.Services.Interfaces;
using CroBooks.Services.Models.Options;
using CroBooks.Shared.Dto;
using CroBooks.Shared.Dto.Request;
using CroBooks.Shared.Dto.Response;
using Microsoft.Extensions.Options;

namespace CroBooks.Services
{
    public class UserService : IUserService
    {
        private readonly AppSecuritySettingsOptions securityOptions;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IOptions<AppSecuritySettingsOptions> securityOptions
            , IUnitOfWork unitOfWork)
        {
            this.securityOptions = securityOptions.Value;
            this.unitOfWork = unitOfWork;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            if (securityOptions == null)
                throw new ApplicationException("Configuration error");

            var response = new LoginResponseDto()
            {
                Message = "User or passowrd is invalid;",
            };

            var user = await unitOfWork.Users.GetUserByEmailOrUsername(dto.UsernameOrEmail);
            if (user == null)
                return response;

            var role = await unitOfWork.Roles.FindAsync(user.RoleId);
            if (role == null)
                return response;

            var passHash = SecurityHelper.CreatePasswordHash(dto.Password);
            var passwordValid = SecurityHelper.ValidatePassword(dto.Password, user.Password);
            if (passwordValid)
            {
                response.Token = SecurityHelper.CreateToken(user.Id.ToString(), user.Username, user.FirstName, user.LastName, role.Name, securityOptions);
                response.Message = "Login successful";
            }

            return response;
        }

        public async Task<UserDto?> GetUser(int id)
        {
            var user = await unitOfWork.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return user.ToDto();
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users = await unitOfWork.Users.GetAllAsync();
            return users.Select(x => x.ToDto()).ToList();
        }

        public async Task<UserDto?> AddUser(CreateUserRequestDto dto)
        {
            var usernameExists = await unitOfWork.Users.EntityExistsAsync(x => x.Username == dto.Username);
            var emailExists = await unitOfWork.Users.EntityExistsAsync(x => x.Email == dto.Email);

            if (usernameExists || emailExists)
                return null;

            var user = new User()
            {
                CreatedDate = DateTime.UtcNow,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                Password = SecurityHelper.CreatePasswordHash(dto.Password),
                RoleId = dto.RoleId
            };
            var result = await unitOfWork.Users.AddAsync(user);
            await unitOfWork.CommitAsync();
            return result.ToDto();
        }

        public async Task<bool> AdminCheck()
        {
            var exists = await unitOfWork.Users.AdminExists();
            return exists;
        }
    }
}
