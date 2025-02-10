
namespace CroBooks.Web.HttpClients.Base
{
    public interface IApiHttpClientBase
    {
        HttpClient HttpClient { get; }

        Task DeleteAsJsonAsync<T>(T dto, string endpoint);
        Task<Tr> GetAsync<T, Tr>(T dto, string endpoint) where Tr : new();
        Task<Tr> GetAsync<Tr>(string endpoint) where Tr : new();
        Task<Tr> PostAsJsonAsync<T, Tr>(T dto, string endpoint) where Tr : new();
        Task<Tr> PostAsync<Tr>(string endpoint) where Tr : new();
        Task<Tr> PutAsJsonAsync<T, Tr>(T dto, string endpoint) where Tr : new();
        Task<Tr> PutAsync<Tr>(string endpoint) where Tr : new();
        Task PutAsync(string endpoint);
    }
}