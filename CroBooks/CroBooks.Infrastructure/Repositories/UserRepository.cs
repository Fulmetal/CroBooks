using CroBooks.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CroBooks.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<User?> GetUserByEmailOrUsername(string usernmaeOrEmail)
        {
            var user = await this.SingleAsync(x => x.Email == usernmaeOrEmail || x.Username == usernmaeOrEmail);
            return user;
        }

        public async Task<bool> AdminExists()
        {
            var result = await this.Queriable().Where(x => x.Role != null && x.Role.Name == "edfesfsef").FirstOrDefaultAsync();
            return result == null;
        }
    }
}
