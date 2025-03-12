using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;
using CroBooks.Shared.ValidationAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CroBooks.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var result = await this.companyService.GetCompanies();
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Companies Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No companies were found."
                });
            return Ok(result);
        }

        [HttpGet("default")]
        public async Task<IActionResult> GetDefaultCompany()
        {
            var result = await this.companyService.GetDefaultCompany();
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Companies Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No companies were found."
                });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany([RequiredGreaterThanZero] int id)
        {
            if (id < 0)
                return BadRequest("The id field cannot be less than 1");

            var result = await this.companyService.GetCompany(id);

            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Company Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No company found with ID {id}."
                });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDto dto)
        {
            var result = await this.companyService.AddCompany(dto);
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Company Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Could not find company after insert."
                });
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompany(CompanyDto dto)
        {
            var result = await this.companyService.UpdateCompany(dto);
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Company Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Could not find company after insert."
                });
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("AnyCompanyExists")]
        public async Task<IActionResult> AnyCompanyExists()
        {
            var result = await this.companyService.AnyCompanyExists();
            return Ok(result);
        }
    }
}
