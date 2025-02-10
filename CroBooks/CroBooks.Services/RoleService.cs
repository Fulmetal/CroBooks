using Microsoft.Extensions.Logging;
using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Roles;
using CroBooks.Services.Interfaces;

namespace OVKS.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<Role>> GetRoles()
        {
            var result = await this.unitOfWork.Roles.GetAllAsync();
            return result.ToList();
        }
    }
}
