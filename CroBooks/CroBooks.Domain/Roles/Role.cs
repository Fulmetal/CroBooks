using CroBooks.Domain.Base;
using CroBooks.Domain.Users;

namespace CroBooks.Domain.Roles
{
    public class Role : AuditEntity<int>
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
