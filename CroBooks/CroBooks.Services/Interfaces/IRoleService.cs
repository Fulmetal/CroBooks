using CroBooks.Domain.Roles;

namespace CroBooks.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetRoles();
    }
}