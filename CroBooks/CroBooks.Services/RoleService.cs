using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Roles;
using CroBooks.Services.Interfaces;

namespace CroBooks.Services
{
    public class RoleService(IUnitOfWork unitOfWork) : IRoleService
    {
        public async Task<List<Role>> GetRoles()
        {
            var result = await unitOfWork.Roles.GetAllAsync();
            return result.ToList();
        }
    }
}
