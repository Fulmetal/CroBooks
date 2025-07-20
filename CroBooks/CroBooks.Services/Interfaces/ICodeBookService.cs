using CroBooks.Shared.Dto;

namespace CroBooks.Services.Interfaces;

public interface ICodeBookService<T>  where T : class
{
    Task<List<CodeBookDto>> GetCodeBook();
    Task<CodeBookDto?> AddCodeBook(CodeBookDto codeBookDto);
    Task<CodeBookDto?> UpdateCodeBook(CodeBookDto? codeBookDto);
    Task DeleteCodeBook(int id);
}