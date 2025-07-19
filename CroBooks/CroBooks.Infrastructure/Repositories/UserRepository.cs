using CroBooks.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CroBooks.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : Repository<User, int>(context), IUserRepository
    {
        public async Task<User?> GetUserByEmailOrUsername(string usernmaeOrEmail)
        {
            var user = await SingleAsync(x => x.Email == usernmaeOrEmail || x.Username == usernmaeOrEmail);
            return user;
        }

        public async Task<bool> AdminExists()
        {
            var result = await Queriable().Where(x => x.Role != null && x.Role.Name == "Admin").FirstOrDefaultAsync();
            return result != null;
        }
    }
}
