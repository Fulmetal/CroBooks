using CroBooks.Domain.Companies;
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
    }
}
