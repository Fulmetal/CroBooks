using CroBooks.Domain.Companies;
using CroBooks.Domain.Roles;
using CroBooks.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CroBooks.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}
