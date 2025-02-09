using CroBooks.Shared.Dto;
using CroBooks.Shared.Dto.Request;
using CroBooks.Shared.Dto.Response;
using System.Text.Json;

namespace CroBooks.Web.HttpClients
{
    public class UserHttpClient(HttpClient httpClient)
    {
        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var response = await httpClient.PostAsJsonAsync($"/login", json);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<LoginResponseDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result == null)
                throw new Exception("Login failed");
            return result;
        }

        public async Task<UserDto> GetUser(int id)
        {
            var response = await httpClient.GetAsync($"/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null)
                throw new Exception("User not found");
            return result;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var response = await httpClient.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<UserDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null)
                throw new Exception("Users not found");
            return result;
        }

        public async Task<UserDto> AddUser(CreateUserRequestDto dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var response = await httpClient.PostAsJsonAsync($"/", json);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result == null)
                throw new Exception("User not found");
            return result;
        }
    }
}
