using CroBooks.Shared.Dto;

namespace CroBooks.Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientDto> AddClient(ClientDto dto);
        Task DeleteClient(int id);
        Task<ClientDto?> GetClient(int id);
        Task<List<ClientDto>> GetClients();
        Task<ClientDto?> UpdateClient(ClientDto dto);
    }
}