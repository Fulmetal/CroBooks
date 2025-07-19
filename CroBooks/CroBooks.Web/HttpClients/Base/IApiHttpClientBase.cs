
namespace CroBooks.Web.HttpClients.Base
{
    public interface IApiHttpClientBase
    {
        HttpClient HttpClient { get; }

        Task DeleteAsJsonAsync<T>(T dto, string endpoint);
        Task<TR> GetAsync<T, TR>(T dto, string endpoint) where TR : new();
        Task<TR> GetAsync<TR>(string endpoint) where TR : new();
        Task<TR> PostAsJsonAsync<T, TR>(T dto, string endpoint) where TR : new();
        Task<TR> PostAsync<TR>(string endpoint) where TR : new();
        Task<TR> PutAsJsonAsync<T, TR>(T dto, string endpoint) where TR : new();
        Task<TR> PutAsync<TR>(string endpoint) where TR : new();
        Task PutAsync(string endpoint);
    }
}