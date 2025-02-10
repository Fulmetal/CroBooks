using CroBooks.Domain.Companies;
using CroBooks.Domain.Roles;
using CroBooks.Domain.Users;

namespace CroBooks.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository Companies { get; }
        IUserRepository Users { get; }
        IRolesRepository Roles { get; }
        Task<int> CommitAsync();
    }
}
