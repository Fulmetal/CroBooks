using CroBooks.Domain.Clients;
using CroBooks.Domain.Interfaces;
using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;

namespace CroBooks.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientDto?> GetClient(int id)
        {
            var client = await _unitOfWork.Clients.FindAsync(id);
            return client?.ToDto();
        }

        public async Task<List<ClientDto>> GetClients()
        {
            var client = await _unitOfWork.Clients.GetAllAsync();
            return client.Select(x => x.ToDto()).ToList();
        }

        public async Task<ClientDto> AddClient(ClientDto dto)   
        {
            var client = new Client(dto);
            var result = await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.CommitAsync();

            return result.ToDto();
        }

        public async Task<ClientDto?> UpdateClient(ClientDto dto)
        {
            var client = await _unitOfWork.Clients.FindAsync(dto.Id);
            if (client == null)
                return null;
            client.UpdateFromDto(client, dto);
            await _unitOfWork.Clients.UpdateAsync(client);
            await _unitOfWork.CommitAsync();
            return client.ToDto();
        }

        public async Task DeleteClient(int id)
        {
            var client = await _unitOfWork.Clients.FindAsync(id);
            if (client == null)
                return;

            await _unitOfWork.Clients.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }
    }
}
