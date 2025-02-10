using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CroBooks.Web.HttpClients.Base
{
    public abstract class ApiHttpClientBase : IApiHttpClientBase
    {
        protected ApiHttpClientBase(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public async Task<Tr> GetAsync<Tr>(string endpoint) where Tr : new()
        {
            var response = await this.HttpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<Tr>(response);
        }

        public async Task<Tr> GetAsync<T, Tr>(T dto, string endpoint) where Tr : new()
        {
            var response = await this.HttpClient.PostAsJsonAsync(endpoint, dto);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<Tr>(response);
        }

        public async Task<Tr> PostAsJsonAsync<T, Tr>(T dto, string endpoint) where Tr : new()
        {
            var response = await this.HttpClient.PostAsJsonAsync(endpoint, dto);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<Tr>(response);
        }

        public async Task<Tr> PostAsync<Tr>(string endpoint) where Tr : new()
        {
            var response = await this.HttpClient.PostAsync(endpoint, null);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<Tr>(response);
        }

        public async Task<Tr> PutAsJsonAsync<T, Tr>(T dto, string endpoint) where Tr : new()
        {
            var response = await this.HttpClient.PutAsJsonAsync(endpoint, dto);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<Tr>(response);
        }

        public async Task PutAsync(string endpoint)
        {
            var response = await this.HttpClient.PutAsync(endpoint, null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Tr> PutAsync<Tr>(string endpoint) where Tr : new()
        {
            var response = await this.HttpClient.PutAsync(endpoint, null);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<Tr>(response);

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
