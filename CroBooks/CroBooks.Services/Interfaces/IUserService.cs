using CroBooks.Shared.Dto;

namespace CroBooks.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUser(int id);
    }
}