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

    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var result = await this.clientService.GetClients();
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

            var result = await this.clientService.GetClient(id);

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
            var result = await this.clientService.UpdateClient(dto);
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Client Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Could not find client after insert."
                });
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient(ClientDto dto)
        {
            var result = await this.clientService.UpdateClient(dto);
            if (result == null)
                return NotFound(new ProblemDetails
                {
                    Title = "Client Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Could not find client after update."
                });
            return Ok(result);
        }
    }
}
