using CroBooks.Shared.Dto;
using CroBooks.Shared.Dto.Request;
using CroBooks.Web.HttpClients.Base;

namespace CroBooks.Web.HttpClients
{
    public class UserHttpClient(HttpClient httpClient) : ApiHttpClientBase(httpClient)
    {
        private const string ControllerBase = "/api/user";

        public async Task<UserDto> GetUser(int id)
        {
            return await GetAsync<UserDto>($"{ControllerBase}/{id}");
        }

        public async Task<List<UserDto>> GetUsers()
        {
            return await GetAsync<List<UserDto>>($"{ControllerBase}/");
        }

        public async Task<UserDto> AddUser(CreateUserRequestDto dto)
        {
            return await PostAsJsonAsync<CreateUserRequestDto, UserDto>(dto, $"{ControllerBase}/");
        }

        public async Task<UserDto> SetupAdminUser(CreateUserRequestDto dto)
        {
            return await PostAsJsonAsync<CreateUserRequestDto, UserDto>(dto, $"{ControllerBase}/setupadmin");
        }

        public async Task<bool> CheckAdminExists()
        {
            return await GetAsync<bool>($"{ControllerBase}/admincheck");
        }
    }
}
