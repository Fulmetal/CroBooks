using CroBooks.Domain.Clients;
using CroBooks.Domain.Interfaces;
using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;

namespace CroBooks.Services
{
    public class ClientService(IUnitOfWork unitOfWork) : IClientService
    {
        public async Task<ClientDto?> GetClient(int id)
        {
            var client = await unitOfWork.Clients.FindAsync(id);
            return client?.ToDto();
        }

        public async Task<List<ClientDto>> GetClients()
        {
            var client = await unitOfWork.Clients.GetAllAsync();
            return client.Select(x => x.ToDto()).ToList();
        }

        public async Task<ClientDto> AddClient(ClientDto dto)   
        {
            var client = new Client(dto);
            var result = await unitOfWork.Clients.AddAsync(client);
            await unitOfWork.CommitAsync();

            return result.ToDto();
        }

        public async Task<ClientDto?> UpdateClient(ClientDto dto)
        {
            var client = await unitOfWork.Clients.FindAsync(dto.Id);
            if (client == null)
                return null;
            Client.UpdateFromDto(client, dto);
            await unitOfWork.Clients.UpdateAsync(client);
            await unitOfWork.CommitAsync();
            return client.ToDto();
        }

        public async Task DeleteClient(int id)
        {
            var client = await unitOfWork.Clients.FindAsync(id);
            if (client == null)
                return;

            await unitOfWork.Clients.DeleteAsync(id);
            await unitOfWork.CommitAsync();
        }
    }
}
