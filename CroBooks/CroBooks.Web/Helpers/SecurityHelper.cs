using Blazored.LocalStorage;

namespace CroBooks.Web.Helpers
{
    public class SecurityHelper
    {
        private readonly ILocalStorageService localStorage;

        public SecurityHelper(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        public async Task<string> GetToken()
        {
            return await localStorage.GetItemAsync<string>("jwt");
        }

        public async Task<bool> IsAuthenticated()
        {
            var token = await GetToken();
            return !string.IsNullOrEmpty(token);
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("jwt");
        }

        public async Task SetToken(string token)
        {
            await localStorage.SetItemAsync("jwt", token);
        }
    }
}
