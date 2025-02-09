using CroBooks.Domain.Tests;

namespace CroBooks.Infrastructure.Repositories
{
    public class TestRepository : Repository<Test, int>, ITestRepository
    {
        public TestRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
