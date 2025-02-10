using CroBooks.Domain.Companies;
using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Roles;
using CroBooks.Domain.Users;
using CroBooks.Infrastructure.Repositories;

namespace CroBooks.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private CompanyRepository _companyRepository = default!;
        private UserRepository userRepository = default!;
        private RolesRepository roleRepository = default!;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICompanyRepository Companies => _companyRepository ??= new CompanyRepository(_context);
        public IUserRepository Users => userRepository ??= new UserRepository(_context);
        public IRolesRepository Roles => roleRepository ??= new RolesRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
