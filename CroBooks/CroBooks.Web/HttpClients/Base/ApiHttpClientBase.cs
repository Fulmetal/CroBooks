using Newtonsoft.Json;

namespace CroBooks.Web.HttpClients.Base
{
    public abstract class ApiHttpClientBase : IApiHttpClientBase
    {
        protected ApiHttpClientBase(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public async Task<TR> GetAsync<TR>(string endpoint) where TR : new()
        {
            var response = await this.HttpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<TR>(response);
        }

        public async Task<TR> GetAsync<T, TR>(T dto, string endpoint) where TR : new()
        {
            var response = await this.HttpClient.PostAsJsonAsync(endpoint, dto);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<TR>(response);
        }

        public async Task<TR> PostAsJsonAsync<T, TR>(T dto, string endpoint) where TR : new()
        {
            var response = await this.HttpClient.PostAsJsonAsync(endpoint, dto);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<TR>(response);
        }

        public async Task<TR> PostAsync<TR>(string endpoint) where TR : new()
        {
            var response = await this.HttpClient.PostAsync(endpoint, null);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<TR>(response);
        }

        public async Task<TR> PutAsJsonAsync<T, TR>(T dto, string endpoint) where TR : new()
        {
            var response = await this.HttpClient.PutAsJsonAsync(endpoint, dto);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<TR>(response);
        }

        public async Task PutAsync(string endpoint)
        {
            var response = await this.HttpClient.PutAsync(endpoint, null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<TR> PutAsync<TR>(string endpoint) where TR : new()
        {
            var response = await this.HttpClient.PutAsync(endpoint, null);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<TR>(response);

        }

        public async Task DeleteAsJsonAsync<T>(T dto, string endpoint)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Content = JsonContent.Create(dto),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(this.HttpClient.BaseAddress + endpoint)
            };

            var response = await this.HttpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        private async Task<Tr> DeserializeResponse<Tr>(HttpResponseMessage response) where Tr : new()
        {
            var responseData = await response.Content.ReadAsStringAsync();

            var responseDto = JsonConvert.DeserializeObject<Tr>(responseData);

            if (responseDto == null) { return new Tr(); }

            return responseDto;
        }
    }
}
