using CroBooks.Domain.Clients;
using CroBooks.Domain.Companies;
using CroBooks.Domain.Contacts;
using CroBooks.Domain.Roles;
using CroBooks.Domain.Users;

namespace CroBooks.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository Companies { get; }
        IUserRepository Users { get; }
        IRolesRepository Roles { get; }
        IClientRepository Clients { get; }
        IContactRepository Contacts { get; }
        Task<int> CommitAsync();
    }
}
