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

        public async Task<CompanyDto> AddCompany(CompanyDto dto)
        {
            var company = new Company(dto);
            var result = await companyRepository.AddAsync(company);
            await unitOfWork.SaveChangesAsync();

            return result.ToDto();
        }
    }
}
