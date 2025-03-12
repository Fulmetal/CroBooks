using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace CroBooks.Web.Helpers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;

        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        public CustomAuthStateProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var savedToken = await localStorageService.GetItemAsync<string>("token");

                if (string.IsNullOrEmpty(savedToken))
                {
                    await UpdateAuthenticationState(savedToken);
                    return await Task.FromResult(new AuthenticationState(anonymous));
                }

                if (string.IsNullOrEmpty(Constants.Token))
                {
                    Constants.Token = await localStorageService.GetItemAsync<string>("token");
                }

                if (string.IsNullOrEmpty(Constants.Token))
                {
                    await UpdateAuthenticationState(Constants.Token);
                    return await Task.FromResult(new AuthenticationState(anonymous));
                }

                var claims = ParseClaimsFromJwt(Constants.Token);
                // Checks the exp field of the token
                var expiry = claims.Where(claim => claim.Type.Equals("exp")).FirstOrDefault();
                if (expiry == null)
                {
                    //UpdateAuthenticationState(Constants.Token);
                    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
                    return await Task.FromResult(new AuthenticationState(anonymous));
                }

                // The exp field is in Unix time
                var datetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry.Value));
                if (datetime.UtcDateTime <= DateTime.UtcNow)
                {
                    //UpdateAuthenticationState(Constants.Token);
                    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
                    return await Task.FromResult(new AuthenticationState(anonymous));
                }

                var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));

                return await Task.FromResult(authState);

            }
            catch (Exception)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        public async Task UpdateAuthenticationState(string token)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (!string.IsNullOrEmpty(token))
            {
                Constants.Token = token;
                await localStorageService.SetItemAsync("token", token);
                var claims = ParseClaimsFromJwt(token);
                claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, "jwt"));
            }
            else
            {
                Constants.Token = null!;
                await localStorageService.RemoveItemAsync("token");
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<int> GetUserId()
        {
            var authState = await GetAuthenticationStateAsync();

            if (authState == null) return 0;

            var user = authState.User;

            if (user == null) return 0;

            var id = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return Convert.ToInt32(id);
        }

        public async Task<string> GetUsername()
        {
            var authState = await GetAuthenticationStateAsync();

            if (authState == null) return string.Empty;

            var user = authState.User;

            if (user == null) return string.Empty;

            var username = user.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
            return username;
        }

        public async Task<string> GetUserRole()
        {
            var authState = await GetAuthenticationStateAsync();

            if (authState == null) return string.Empty;

            var user = authState.User;

            if (user == null) return string.Empty;

            var role = user.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;

            return role;
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs == null) return new List<Claim>();

            return keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString()));
        }

        public static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            base64 = base64.Replace('_', '/').Replace('-', '+');

            return Convert.FromBase64String(base64);
        }
    }

    public static class Constants
    {
        public static string Token { get; set; } = string.Empty;
    }
}
