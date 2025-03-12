using CroBooks.Shared.Dto.Request;
using CroBooks.Shared.Dto.Response;
using CroBooks.Web.HttpClients.Base;
using Blazored.LocalStorage;

namespace CroBooks.Web.HttpClients
{
    public class AuthHttpClient : ApiHttpClientBase
    {
        public const string controllerBase = "/api/auth";
        private readonly HttpClient httpClient;

        public AuthHttpClient(HttpClient httpClient) : base(httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var response = await PostAsJsonAsync<LoginRequestDto, LoginResponseDto>(dto, $"{controllerBase}/login");
            return response;
        }

        public async Task<LoginResponseDto> Logout(LoginRequestDto dto)
        {
            return await PostAsJsonAsync<LoginRequestDto, LoginResponseDto>(dto, $"{controllerBase}/logout");
        }

    }
}
