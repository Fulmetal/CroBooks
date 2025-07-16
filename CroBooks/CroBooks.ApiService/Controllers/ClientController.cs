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

    public class ClientController(IClientService clientService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var result = await clientService.GetClients();
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Clients Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No clients were found."
                });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient([RequiredGreaterThanZero] int id)
        {
            if (id < 0)
                return BadRequest("The id field cannot be less than 1");

            var result = await clientService.GetClient(id);

            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Client Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"No client found with ID {id}."
                });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(ClientDto dto)
        {
            var result = await clientService.AddClient(dto);
            if (result == null)
                return BadRequest(new ProblemDetails
                {
                    Title = "Cannot add this client.",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"Cannot add this client."
                });
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient(ClientDto dto)
        {
            var result = await clientService.UpdateClient(dto);
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Client Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Could not find client after update."
                });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient([RequiredGreaterThanZero] int id)
        {
            if (id < 0)
                return BadRequest("The id field cannot be less than 1");

            await clientService.DeleteClient(id);

            return Ok();
        }
    }
}
