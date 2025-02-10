using CroBooks.Shared.Dto;
using CroBooks.Shared.Dto.Request;
using CroBooks.Shared.Dto.Response;

namespace CroBooks.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AddUser(CreateUserRequestDto dto);
        Task<bool> AdminCheck();
        Task<UserDto?> GetUser(int id);
        Task<List<UserDto>> GetUsers();
        Task<LoginResponseDto> Login(LoginRequestDto dto);
    }
}