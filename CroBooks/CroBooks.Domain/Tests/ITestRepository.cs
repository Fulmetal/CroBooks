using CroBooks.Domain.Interfaces;

namespace CroBooks.Domain.Tests
{
    public interface ITestRepository : IRepository<Test, int>
    {
    }
}