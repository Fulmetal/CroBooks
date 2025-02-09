using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto.Request;
using CroBooks.Shared.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CroBooks.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await this.userService.GetUsers();
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Users Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No users were found."
                });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([RequiredGreaterThanZero] int id)
        {
            if (id < 0)
                return BadRequest("The id field cannot be less than 1");
            var result = await this.userService.GetUser(id);
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "User Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No user found with ID {id}."
                });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(CreateUserRequestDto dto)
        {
            var result = await this.userService.AddUser(dto);
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "User Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No user found with after insert."
                });
            return Ok(result);
        }
    }
}
