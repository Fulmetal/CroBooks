using CroBooks.Shared.Dto;
using System.Text.Json;

namespace CroBooks.Web.HttpClients
{
    public class CompanyHttpClient(HttpClient httpClient)
    {
        public async Task<CompanyDto> GetCompany(int id)
        {
            var response = await httpClient.GetAsync($"/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CompanyDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null)
                throw new Exception("Company not found");
            return result;
        }

        public async Task<List<CompanyDto>> GetCompanies()
        {
            var response = await httpClient.GetAsync("/");
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
            var response = await httpClient.PostAsJsonAsync($"/", json);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CompanyDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result == null)
                throw new Exception("Company not found");
            return result;
        }
    }
}
