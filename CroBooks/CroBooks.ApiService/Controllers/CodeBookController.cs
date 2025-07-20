using CroBooks.Domain.CodeBooks;
using CroBooks.Services;
using CroBooks.Services.Interfaces;
using CroBooks.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CroBooks.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class CodeBookController(IServiceScopeFactory scopeFactory) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCodeBook(CodeBooksEnum codeBookType)
    {
        using var scope = scopeFactory.CreateScope();
        switch (codeBookType)
        {
            case CodeBooksEnum.AddressType:
            {
                var service = scope.ServiceProvider.GetRequiredService<ICodeBookService<AddressType>>();
                return Ok(await service.GetCodeBook());
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(codeBookType), codeBookType, null);
        }
    }

}