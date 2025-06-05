using CroBooks.Shared.Dto;
using CroBooks.Web.HttpClients.Base;

namespace CroBooks.Web.HttpClients
{
    public class ClientHttpClient : ApiHttpClientBase
    {
        public const string controllerBase = "/api/client";

        public ClientHttpClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ClientDto> GetClient(int id)
        {
            return await GetAsync<ClientDto>($"{controllerBase}/{id}");
        }

        public async Task<List<ClientDto>> GetClients()
        {
            return await GetAsync<List<ClientDto>>($"{controllerBase}/");
        }

        public async Task<ClientDto> AddClient(ClientDto dto)
        {
            return await PostAsJsonAsync<ClientDto, ClientDto>(dto, $"{controllerBase}/");
        }

        public async Task<ClientDto> UpdateClient(ClientDto dto)
        {
            return await PutAsJsonAsync<ClientDto, ClientDto>(dto, $"{controllerBase}/");
        }
    }
}
