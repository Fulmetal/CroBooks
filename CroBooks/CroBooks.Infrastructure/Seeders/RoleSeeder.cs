using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Roles;

namespace CroBooks.Infrastructure.Seeders
{
    public class RoleSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public RoleSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SeedData()
        {

            AddRole(CreateRole("Admin"));
            AddRole(CreateRole("User"));

            _context.SaveChanges();
        }

        private Role CreateRole(string name)
        {
            var role = new Role
            {
                Name = name,
                CreatedDate = DateTime.UtcNow
            };

            return role;
        }

        private void AddRole(Role role)
        {
            var existingRole = _context.Roles.FirstOrDefault(p => p.Name == role.Name);
            if (existingRole == null) _context.Roles.Add(role);
        }
    }
}
