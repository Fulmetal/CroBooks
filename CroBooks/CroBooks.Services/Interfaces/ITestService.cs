using CroBooks.Shared.Dto;

namespace CroBooks.Services.Interfaces
{
    public interface ITestService
    {
        Task<List<TestDto>> GetTest();
        Task<TestDto> InsertTest();
    }
}