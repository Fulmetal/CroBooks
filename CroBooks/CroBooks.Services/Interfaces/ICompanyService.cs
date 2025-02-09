using CroBooks.Shared.Dto;

namespace CroBooks.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDto> AddCompany(CompanyDto dto);
    }
}