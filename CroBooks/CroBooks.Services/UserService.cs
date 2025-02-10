using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Users;
using CroBooks.Services.Helpers;
using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;
using CroBooks.Shared.Dto.Request;
using CroBooks.Shared.Dto.Response;

namespace CroBooks.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var response = new LoginResponseDto()
            {
                Message = "User or passowrd is invalid;",
            };

            var user = await unitOfWork.Users.GetUserByEmailOrUsername(dto.UsernameOrEmail);
            if (user == null)
            {
                return response;
            }

            var passHash = SecurityHelper.CreatePasswordHash(dto.Password);
            var passwordValid = SecurityHelper.ValidatePassword(dto.Password, user.Password);
            if (passwordValid)
            {
                response.Token = SecurityHelper.CreateToken(user.Id.ToString(), user.Username, user.FirstName, user.LastName, string.Empty, "todo: generate proper key and store it");
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

        public async Task<UserDto> AddUser(CreateUserRequestDto dto)
        {
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
            var exists = await unitOfWork.Users.EntityExistsAsync(x => x.Role.Name == "Admin");
            return exists;
        }
    }
}
