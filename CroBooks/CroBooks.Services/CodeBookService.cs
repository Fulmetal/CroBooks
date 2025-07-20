using CroBooks.Domain.CodeBooks;
using CroBooks.Domain.Interfaces;
using CroBooks.Services.Interfaces;
using CroBooks.Shared.Dto;

namespace CroBooks.Services;

public class CodeBookService<T> : ICodeBookService<T> where T : class, new()
{
    private readonly IUnitOfWork<T> _unitOfWork;

    public CodeBookService(ICodeBookRepository<T> codeBookRepository,
        IUnitOfWork<T> unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ValidateClass();
    }

    public async Task<List<CodeBookDto>> GetCodeBook()
    {
        var codeBook = await _unitOfWork.CodeBook.GetAllAsync();
        return ConvertToCodeBookDtoList(codeBook as List<ICodeBook>);
    }

    public async Task<CodeBookDto?> AddCodeBook(CodeBookDto codeBookDto)
    {
        var cb = ConvertToCodeBook(codeBookDto);
        if (cb == null) return null;
        
        var result = await _unitOfWork.CodeBook.AddAsync(cb);
        await _unitOfWork.CommitAsync();

        return ConvertToCodeBookDto(result);
    }

    public async Task<CodeBookDto?> UpdateCodeBook(CodeBookDto? codeBookDto)
    {
        if (codeBookDto == null) return null;
        if (codeBookDto.IsSystemGenerated) return null;
        
        var cb = await _unitOfWork.CodeBook.FindAsync(codeBookDto.Id);
        if (cb is not ICodeBook c) return null;
        
        c.Name = codeBookDto.Name;
        
        await _unitOfWork.CodeBook.UpdateAsync(cb);
        await _unitOfWork.CommitAsync();

        return ConvertToCodeBookDto(cb);
    }

    public async Task DeleteCodeBook(int id)
    {
        var cb = await _unitOfWork.CodeBook.FindAsync(id);
        if (cb is not ICodeBook c) return;
        if (c.IsSystemGenerated) return;
        
        await _unitOfWork.CodeBook.DeleteAsync(cb);
        await _unitOfWork.CommitAsync();

        return;
    }

    private static void ValidateClass()
    {
        if (!typeof(ICodeBook).IsAssignableFrom(typeof(T)))
        {
            throw new ArgumentException("CodeBookService<T> can only be called with a class that implements ICodeBook.");
        }
    }

    private static T? ConvertToCodeBook(CodeBookDto codeBookDto)
    {
        if (new T() is not ICodeBook cb) return null;
        cb.Name = codeBookDto.Name;
        return (T)cb;
    }

    private static CodeBookDto? ConvertToCodeBookDto(T codeBook)
    {
        if (codeBook is not ICodeBook cb) return null;
        var dto = new CodeBookDto
        {
            Id = cb.Id,
            Name = cb.Name,
            IsSystemGenerated = cb.IsSystemGenerated
        };
        return dto;
    }

    private List<CodeBookDto> ConvertToCodeBookDtoList(List<ICodeBook>? codeBook)
    {
        var codeBookDtos = new List<CodeBookDto>();
        
        if (codeBook == null) 
            return codeBookDtos;
        
        foreach (var codeBookItem in codeBook)
        {
            codeBookDtos.Add(new CodeBookDto()
                {
                    Id = codeBookItem.Id,
                    Name = codeBookItem.Name,
                    IsSystemGenerated = codeBookItem.IsSystemGenerated
                }
            );
        }

        return codeBookDtos;
    }
}