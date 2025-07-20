using CroBooks.Domain.CodeBooks;

namespace CroBooks.Infrastructure.Repositories;

public class CodeBookRepository<T>(ApplicationDbContext context) : Repository<T, int>(context), ICodeBookRepository<T> where T : class
{
    
}