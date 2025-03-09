using CroBooks.Shared.Dto;
using CroBooks.Shared.Dto.Request;
using CroBooks.Shared.Dto.Response;
using CroBooks.Web.HttpClients.Base;
using System.Text.Json;

namespace CroBooks.Web.HttpClients
{
    public class UserHttpClient : ApiHttpClientBase
    {
        public const string controllerBase = "/api/user";

        public UserHttpClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<UserDto> GetUser(int id)
        {
            return await GetAsync<UserDto>($"{controllerBase}/{id}");
        }

        public async Task<List<UserDto>> GetUsers()
        {
            return await GetAsync<List<UserDto>>($"{controllerBase}/"); ;
        }

        public async Task<UserDto> AddUser(CreateUserRequestDto dto)
        {
            return await PostAsJsonAsync<CreateUserRequestDto, UserDto>(dto, $"{controllerBase}/");
        }

        public async Task<UserDto> SetupAdminUser(CreateUserRequestDto dto)
        {
            return await PostAsJsonAsync<CreateUserRequestDto, UserDto>(dto, $"{controllerBase}/setupadmin");
        }

        public async Task<bool> CheckAdminExists()
        {
            return await GetAsync<bool>($"{controllerBase}/admincheck");
        }
    }
}
