using CroBooks.Shared.Dto;

namespace CroBooks.Services.Interfaces
{
    public interface ITestService
    {
        Task<TestDto> GetTest();
        Task<TestDto> InsertTest();
    }
}