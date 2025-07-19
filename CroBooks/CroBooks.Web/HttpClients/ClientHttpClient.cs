using CroBooks.Shared.Dto;
using CroBooks.Web.HttpClients.Base;

namespace CroBooks.Web.HttpClients
{
    public class ClientHttpClient(HttpClient httpClient) : ApiHttpClientBase(httpClient)
    {
        private const string ControllerBase = "/api/client";

        public async Task<ClientDto> GetClient(int id)
        {
            return await GetAsync<ClientDto>($"{ControllerBase}/{id}");
        }

        public async Task<List<ClientDto>> GetClients()
        {
            return await GetAsync<List<ClientDto>>($"{ControllerBase}/");
        }

        public async Task<ClientDto> AddClient(ClientDto dto)
        {
            return await PostAsJsonAsync<ClientDto, ClientDto>(dto, $"{ControllerBase}/");
        }

        public async Task<ClientDto> UpdateClient(ClientDto dto)
        {
            return await PutAsJsonAsync<ClientDto, ClientDto>(dto, $"{ControllerBase}/");
        }

        public async Task DeleteClient(ClientDto dto)
        {
            await DeleteAsJsonAsync<ClientDto>(dto, $"{ControllerBase}/");
        }
    }
}
