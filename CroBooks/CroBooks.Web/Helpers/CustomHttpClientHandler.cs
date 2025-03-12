using Blazored.LocalStorage;
using MudBlazor;

namespace CroBooks.Web.Helpers
{
    public class CustomHttpClientHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _storage;
        private readonly ISnackbar _snackbar;

        public CustomHttpClientHandler(ILocalStorageService storage, ISnackbar snackbar)
        {
            _storage = storage;
            _snackbar = snackbar;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = await _storage.GetItemAsStringAsync("token");

            if (!string.IsNullOrEmpty(token) && token != "\"\"")
                request.Headers.Add("Authorization", $"Bearer {token.Replace("\"", "")}");

            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                await HandleTimeout();
                throw new TimeoutException();
            }
        }

        private async Task HandleTimeout()
        {
            _snackbar.Add("Request has timed out.", Severity.Error);
        }
    }
}
