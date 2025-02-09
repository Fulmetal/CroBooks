using CroBooks.Domain.Interfaces;

namespace CroBooks.Domain.Users
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<User?> GetUserByEmailOrUsername(string usernmaeOrEmail);
    }
}
