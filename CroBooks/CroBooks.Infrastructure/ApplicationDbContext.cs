using Microsoft.EntityFrameworkCore;
using CroBooks.Domain.Tests;

namespace CroBooks.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Test> Tests { get; set; } = null!;
    }
}
