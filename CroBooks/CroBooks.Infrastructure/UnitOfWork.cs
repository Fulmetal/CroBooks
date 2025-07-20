using CroBooks.Domain.Clients;
using CroBooks.Domain.CodeBooks;
using CroBooks.Domain.Companies;
using CroBooks.Domain.Contacts;
using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Roles;
using CroBooks.Domain.Users;
using CroBooks.Infrastructure.Repositories;

namespace CroBooks.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private CompanyRepository _companyRepository = null!;
        private UserRepository _userRepository = null!;
        private RolesRepository _roleRepository = null!;
        private ClientRepository _clientRepository = null!;
        private ContactRepository _contactRepository = null!;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICompanyRepository Companies => _companyRepository ?? new CompanyRepository(_context);
        public IUserRepository Users => _userRepository ?? new UserRepository(_context);
        public IRolesRepository Roles => _roleRepository ?? new RolesRepository(_context);
        public IClientRepository Clients => _clientRepository ?? new ClientRepository(_context);
        public IContactRepository Contacts => _contactRepository ?? new ContactRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        
        private CodeBookRepository<T>? _codeBookRepository = null!;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public ICodeBookRepository<T> CodeBook => _codeBookRepository ?? new CodeBookRepository<T>(_context);

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
