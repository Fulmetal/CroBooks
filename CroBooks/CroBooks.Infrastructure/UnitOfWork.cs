using CroBooks.Domain.Companies;
using CroBooks.Domain.Interfaces;
using CroBooks.Infrastructure.Repositories;

namespace CroBooks.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private CompanyRepository? _companyRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICompanyRepository Companies => _companyRepository ??= new CompanyRepository(_context);

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
