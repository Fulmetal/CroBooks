using CroBooks.Domain.Companies;
using CroBooks.Domain.Interfaces;
using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;

namespace CroBooks.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<CompanyDto?> GetCompany(int id)
        {
            var company = await unitOfWork.Companies.FindAsync(id);
            if (company == null)
            {
                return null;
            }
            return company.ToDto();
        }

        public async Task<List<CompanyDto>> GetCompanies()
        {
            var companies = await unitOfWork.Companies.GetAllAsync();
            return companies.Select(x => x.ToDto()).ToList();
        }

        public async Task<CompanyDto?> GetDefaultCompany()
        {
            var company = await unitOfWork.Companies.SingleAsync(x => x.IsDefault == true);
            if (company == null)
            {
                return null;
            }
            return company.ToDto();
        }

        public async Task<CompanyDto> AddCompany(CompanyDto dto)
        {
            var company = new Company(dto);
            var result = await unitOfWork.Companies.AddAsync(company);
            await unitOfWork.CommitAsync();

            return result.ToDto();
        }

        public async Task<CompanyDto?> UpdateCompany(CompanyDto dto)
        {
            var company = await unitOfWork.Companies.FindAsync(dto.Id);
            if (company == null)
                return null;
            company.UpdateFromDto(company, dto);
            await unitOfWork.Companies.UpdateAsync(company);
            return company.ToDto();
        }

        public async Task<bool> AnyCompanyExists()
        {
            var exists = await unitOfWork.Companies.EntityCountAsync();
            return exists > 0;
        }
    }
}
