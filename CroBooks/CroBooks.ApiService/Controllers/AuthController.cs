using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CroBooks.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await this.userService.Login(dto);
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Login failed",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Username or password invalid"
                });
            return Ok(result);
        }
    }
}
