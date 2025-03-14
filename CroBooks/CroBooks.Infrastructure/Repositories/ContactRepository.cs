using CroBooks.Domain.Contacts;

namespace CroBooks.Infrastructure.Repositories
{
    public class ContactRepository : Repository<Contact, int>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
