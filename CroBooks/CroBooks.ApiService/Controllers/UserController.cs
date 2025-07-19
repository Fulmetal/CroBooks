using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto.Request;
using CroBooks.Shared.ValidationAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CroBooks.ApiService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await userService.GetUsers();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([RequiredGreaterThanZero] int id)
        {
            if (id < 0)
                return BadRequest("The id field cannot be less than 1");
            var result = await userService.GetUser(id);
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "User Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No user found with ID {id}."
                });
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddUser(CreateUserRequestDto dto)
        {
            var result = await userService.AddUser(dto);
            if (result == null)
                return BadRequest(new ProblemDetails
                {
                    Title = "Cannot add this user.",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"The username or email might already exist."
                });
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("setupadmin")]
        public async Task<IActionResult> SetupAdmin(CreateUserRequestDto dto)
        {
            var adminExists = await userService.AdminCheck();
            if (adminExists)
                return BadRequest("You can no longer use this endpoint");

            var result = await userService.AddUser(dto);
            if (result == null)
                return BadRequest(new ProblemDetails
                {
                    Title = "Cannot add this user.",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"The username or email might already exist."
                });
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("admincheck")]
        public async Task<IActionResult> AdminCheck()
        {
            var result = await userService.AdminCheck();
            return Ok(result);
        }
    }
}
