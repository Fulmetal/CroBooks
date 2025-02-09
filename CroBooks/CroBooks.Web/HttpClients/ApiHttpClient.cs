using CroBooks.Shared.Dto;
using System.Text.Json;

namespace CroBooks.Web.HttpClients
{
    public class ApiHttpClient(HttpClient httpClient)
    {
        private static readonly string controllerPath = "company";

        public async Task<CompanyDto> GetCompany(int id)
        {
            var response = await httpClient.GetAsync($"{controllerPath}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CompanyDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null)
                throw new Exception("Company not found");
            return result;
        }

        public async Task<List<CompanyDto>> GetCompanies()
        {
            var response = await httpClient.GetAsync($"{controllerPath}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<CompanyDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null)
                throw new Exception("Companies not found");
            return result;
        }

        public async Task<CompanyDto> AddCompany(CompanyDto dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var response = await httpClient.PostAsJsonAsync($"{controllerPath}", json);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CompanyDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result == null)
                throw new Exception("Company not found");
            return result;
        }
    }
}
