using CroBooks.Domain.Roles;

namespace CroBooks.Infrastructure.Repositories
{
    public class RolesRepository : Repository<Role, int>, IRolesRepository
    {
        public RolesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
