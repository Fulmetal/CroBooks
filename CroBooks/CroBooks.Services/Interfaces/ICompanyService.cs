using CroBooks.Shared.Dto;

namespace CroBooks.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDto> AddCompany(CompanyDto dto);
        Task<List<CompanyDto>> GetCompanies();
        Task<CompanyDto?> GetCompany(int id);
    }
}