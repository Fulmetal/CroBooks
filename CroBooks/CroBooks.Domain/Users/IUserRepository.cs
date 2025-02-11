using CroBooks.Domain.Interfaces;

namespace CroBooks.Domain.Users
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<bool> AdminExists();
        Task<User?> GetUserByEmailOrUsername(string usernmaeOrEmail);
    }
}
