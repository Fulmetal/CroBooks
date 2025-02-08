using System.Linq.Expressions;

namespace CroBooks.Domain.Interfaces;

public interface IRepository<T, TKey> where T : class
{
    Task<T> AttachAsync(T entity);
    Task<long> EntityCountAsync();
    Task<bool> EntityExistsAsync(Func<T, bool> expression);
    Task<T?> FindAsync(TKey id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllDeletedAsync();
    Task<IEnumerable<T>> GetAllIncludingDeletedAsync();
    Task<IEnumerable<T>> SudoGetAllAsync();
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(List<T> list);
    Task DeleteAsync(TKey id);
    Task DeleteAsync(T e);
    Task DeleteRangeAsync(List<TKey> list);
    Task SudoDeleteRangeAsync(List<TKey> list);
    Task RestoreAsync(T entity);
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(List<T> list);
    Task UpdateRangeWithoutDateAsync(List<T> list);
    Task<IList<T>> ListAsync(Expression<Func<T, bool>> expression);
    Task<IList<T>> ListIncludingDeletedAsync(Expression<Func<T, bool>> expression);
    Task<IList<T>> SudoListAsync(Expression<Func<T, bool>> expression);
    Task<T?> SingleAsync(Expression<Func<T, bool>> expression);
    IQueryable<T> Queriable();
}