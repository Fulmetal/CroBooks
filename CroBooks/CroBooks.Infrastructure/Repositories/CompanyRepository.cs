using CroBooks.Domain.Clients;
using CroBooks.Domain.Companies;
using CroBooks.Domain.Contacts;
using Microsoft.AspNetCore.Components.Authorization;

namespace CroBooks.Infrastructure.Repositories
{
    public class CompanyRepository : Repository<Company, int>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
