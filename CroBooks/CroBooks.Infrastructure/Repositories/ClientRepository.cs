using CroBooks.Domain.Clients;

namespace CroBooks.Infrastructure.Repositories
{
    public class ClientRepository(ApplicationDbContext context) : Repository<Client, int>(context), IClientRepository;
}
