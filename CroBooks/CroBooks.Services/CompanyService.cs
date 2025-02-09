using CroBooks.Domain.Companies;
using CroBooks.Domain.Interfaces;
using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;

namespace CroBooks.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IUnitOfWork unitOfWork;

        public CompanyService(ICompanyRepository companyRepository
            , IUnitOfWork unitOfWork)
        {
            this.companyRepository = companyRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CompanyDto?> GetCompany(int id)
        {
            var company = await companyRepository.FindAsync(id);
            if (company == null)
            {
                return null;
            }
            return company.ToDto();
        }

        public async Task<List<CompanyDto>> GetCompanies()
        {
            var companies = await companyRepository.GetAllAsync();
            return companies.Select(x => x.ToDto()).ToList();
        }

        public async Task<CompanyDto> AddCompany(CompanyDto dto)
        {
            var company = new Company(dto);
            var result = await companyRepository.AddAsync(company);
            await unitOfWork.SaveChangesAsync();

            return result.ToDto();
        }
    }
}
