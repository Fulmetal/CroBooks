using CroBooks.Domain.Clients;
using CroBooks.Domain.CodeBooks;
using CroBooks.Domain.Companies;
using CroBooks.Domain.Contacts;
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
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<AddressType> AddressTypes { get; set; } = null!;
    }
}
