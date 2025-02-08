using CroBooks.Domain.Interfaces;
using CroBooks.Domain.Tests;
using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;

namespace CroBooks.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository testRepository;
        private readonly IUnitOfWork unitOfWork;

        public TestService(ITestRepository testRepository,
            IUnitOfWork unitOfWork)
        {
            this.testRepository = testRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<TestDto> GetTest()
        {
            var test = await testRepository.GetAllAsync();
            
            return new TestDto() { Message = "Hello world!" };
        }

        public async Task<TestDto> InsertTest()
        {
            var testResult = await testRepository.AddAsync(new Test() { });
            await unitOfWork.SaveChangesAsync();
            return new TestDto() { Message = $"New task saved with id: {testResult.Id}" };
        }
    }
}
