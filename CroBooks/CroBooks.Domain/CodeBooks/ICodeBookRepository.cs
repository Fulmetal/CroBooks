using CroBooks.Domain.Interfaces;

namespace CroBooks.Domain.CodeBooks;

public interface ICodeBookRepository<T> : IRepository<T, int> where T : class
{
    
}