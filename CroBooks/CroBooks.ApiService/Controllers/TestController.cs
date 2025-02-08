using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CroBooks.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService testService;

        public TestController(ITestService testService)
        {
            this.testService = testService;
        }

        [HttpGet]
        public async Task<List<TestDto>> Get()
        {
            var result = await this.testService.GetTest();
            return result;
        }

        [HttpPost]
        public async Task<TestDto> Post()
        {
            var result = await this.testService.InsertTest();
            return result;
        }
    }
}
