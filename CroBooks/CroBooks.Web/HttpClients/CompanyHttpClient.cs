using CroBooks.Shared.Dto;
using CroBooks.Web.HttpClients.Base;

namespace CroBooks.Web.HttpClients
{
    public class CompanyHttpClient : ApiHttpClientBase
    {
        public const string controllerBase = "/api/company";

        public CompanyHttpClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<CompanyDto> GetCompany(int id)
        {
            return await GetAsync<CompanyDto>($"{controllerBase}/{id}");
        }

        public async Task<CompanyDto> GetDefaultCompany()
        {
            return await GetAsync<CompanyDto>($"{controllerBase}/default");
        }

        public async Task<List<CompanyDto>> GetCompanies()
        {
            return await GetAsync<List<CompanyDto>>($"{controllerBase}/");
        }

        public async Task<CompanyDto> AddCompany(CompanyDto dto)
        {
            return await PostAsJsonAsync<CompanyDto, CompanyDto>(dto, $"{controllerBase}/");
        }

        public async Task<CompanyDto> UpdateCompany(CompanyDto dto)
        {
            return await PutAsJsonAsync<CompanyDto, CompanyDto>(dto, $"{controllerBase}/");
        }

        public async Task<bool> CheckAnyCompanyExists()
        {
            return await GetAsync<bool>($"{controllerBase}/anycompanyexists");
        }
    }
}
