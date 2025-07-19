using CroBooks.Shared.Dto.Request;
using CroBooks.Shared.Dto.Response;
using CroBooks.Web.HttpClients.Base;

namespace CroBooks.Web.HttpClients
{
    public class AuthHttpClient(HttpClient httpClient) : ApiHttpClientBase(httpClient)
    {
        private const string ControllerBase = "/api/auth";

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var response = await PostAsJsonAsync<LoginRequestDto, LoginResponseDto>(dto, $"{ControllerBase}/login");
            return response;
        }

        public async Task<LoginResponseDto> Logout(LoginRequestDto dto)
        {
            return await PostAsJsonAsync<LoginRequestDto, LoginResponseDto>(dto, $"{ControllerBase}/logout");
        }

    }
}
