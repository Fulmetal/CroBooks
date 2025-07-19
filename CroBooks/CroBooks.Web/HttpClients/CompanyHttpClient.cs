using CroBooks.Shared.Dto;
using CroBooks.Web.HttpClients.Base;

namespace CroBooks.Web.HttpClients
{
    public class CompanyHttpClient(HttpClient httpClient) : ApiHttpClientBase(httpClient)
    {
        private const string ControllerBase = "/api/company";

        public async Task<CompanyDto> GetCompany(int id)
        {
            return await GetAsync<CompanyDto>($"{ControllerBase}/{id}");
        }

        public async Task<CompanyDto> GetDefaultCompany()
        {
            return await GetAsync<CompanyDto>($"{ControllerBase}/default");
        }

        public async Task<List<CompanyDto>> GetCompanies()
        {
            return await GetAsync<List<CompanyDto>>($"{ControllerBase}/");
        }

        public async Task<CompanyDto> AddCompany(CompanyDto dto)
        {
            return await PostAsJsonAsync<CompanyDto, CompanyDto>(dto, $"{ControllerBase}/");
        }

        public async Task<CompanyDto> UpdateCompany(CompanyDto dto)
        {
            return await PutAsJsonAsync<CompanyDto, CompanyDto>(dto, $"{ControllerBase}/");
        }

        public async Task<bool> CheckAnyCompanyExists()
        {
            return await GetAsync<bool>($"{ControllerBase}/anycompanyexists");
        }
    }
}
