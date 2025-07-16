using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CroBooks.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AuthController(IUserService userService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await userService.Login(dto);
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
