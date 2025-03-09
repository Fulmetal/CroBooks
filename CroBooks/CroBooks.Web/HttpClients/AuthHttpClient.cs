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
        private readonly ILocalStorageService localStorage;

        public AuthHttpClient(HttpClient httpClient
            , ILocalStorageService localStorage) : base(httpClient)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var response = await PostAsJsonAsync<LoginRequestDto, LoginResponseDto>(dto, $"{controllerBase}/login");
            if (response.Token != null)
            {
                await localStorage.SetItemAsync("jwt", response.Token);
            }
            return response;
        }

        public async Task<LoginResponseDto> Logout(LoginRequestDto dto)
        {
            await localStorage.RemoveItemAsync("jwt");
            return await PostAsJsonAsync<LoginRequestDto, LoginResponseDto>(dto, $"{controllerBase}/logout");
        }

    }
}
