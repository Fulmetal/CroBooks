using CroBooks.Domain.Clients;

namespace CroBooks.Infrastructure.Repositories
{
    public class ClientRepository : Repository<Client, int>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
