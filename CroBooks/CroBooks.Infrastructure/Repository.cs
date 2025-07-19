using CroBooks.Domain.Base;
using CroBooks.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CroBooks.Infrastructure;

public class Repository<T, TKey> : IRepository<T, TKey> where T : class
{
    private readonly DbSet<T> _dbSet;

    protected Repository(ApplicationDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<T> AttachAsync(T entity)
    {
        _dbSet.Attach(entity);
        return await Task.FromResult(entity);
    }

    public async Task<long> EntityCountAsync()
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            return await _dbSet.Where(x => !((IDeleteEntity)x).IsDeleted).CountAsync();

        return await _dbSet.CountAsync();
    }

    public async Task<bool> EntityExistsAsync(Func<T, bool> expression)
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
        {
            var r = _dbSet.Where(x => !((IDeleteEntity)x).IsDeleted).AsEnumerable();
            return r.Any(expression);
        }

        return await Task.FromResult(_dbSet.Any(expression));
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            return await _dbSet.AsNoTracking().Where(x => !((IDeleteEntity)x).IsDeleted).ToListAsync();

        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllDeletedAsync()
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            return await _dbSet.AsNoTracking().Where(x => ((IDeleteEntity)x).IsDeleted).ToListAsync();

        return new List<T>();
    }

    public async Task<IEnumerable<T>> GetAllIncludingDeletedAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> SudoGetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> FindAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T))) ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;

        var newEntity = await _dbSet.AddAsync(entity);
        return newEntity.Entity;
    }

    public async Task AddRangeAsync(List<T> list)
    {
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            foreach (var entity in list)
                ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;
        await _dbSet.AddRangeAsync(list);
    }

    public async Task DeleteAsync(TKey id)
    {
        var e = await FindAsync(id);

        if (e == null) return;

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
        {
            ((IDeleteEntity)e).IsDeleted = true;
            ((IDeleteEntity)e).DeletedDate = DateTime.UtcNow;

            _dbSet.Update(e);
            return;
        }

        _dbSet.Remove(e);
    }

    public async Task DeleteAsync(T e)
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
        {
            ((IDeleteEntity)e).IsDeleted = true;
            ((IDeleteEntity)e).DeletedDate = DateTime.UtcNow;

            _dbSet.Update(e);
            await Task.CompletedTask;
            return;
        }

        _dbSet.Remove(e);
        await Task.CompletedTask;
    }


    public async Task DeleteRangeAsync(List<TKey> list)
    {
        var entities = await ListAsync(x => list.Contains(((IEntityBase<TKey>)x).Id));

        foreach (var entity in entities)
        {
            if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            {
                ((IDeleteEntity)entity).IsDeleted = true;
                ((IDeleteEntity)entity).DeletedDate = DateTime.UtcNow;
            }
        }

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            _dbSet.UpdateRange(entities);
        else
            _dbSet.RemoveRange(entities);
    }

    public async Task SudoDeleteRangeAsync(List<TKey> list)
    {
        var entities = await SudoListAsync(x => list.Contains(((IEntityBase<TKey>)x).Id));
        _dbSet.RemoveRange(entities);
    }

    public async Task RestoreAsync(T entity)
    {
        var e = await FindAsync(((IEntityBase<TKey>)entity).Id);

        if (e == null) return;

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
        {
            ((IDeleteEntity)e).IsDeleted = false;
            ((IDeleteEntity)e).DeletedDate = null;
            _dbSet.Update(e);
        }
    }

    public async Task<IList<T>> ListAsync(Expression<Func<T, bool>> expression)
    {
        var r = _dbSet.AsNoTracking().Where(expression);

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T))) r = r.Where(x => !((IDeleteEntity)x).IsDeleted);

        return await r.ToListAsync();
    }

    public async Task<IList<T>> ListIncludingDeletedAsync(Expression<Func<T, bool>> expression)
    {
        var r = _dbSet.Where(expression);

        return await r.ToListAsync();
    }

    public async Task<IList<T>> SudoListAsync(Expression<Func<T, bool>> expression)
    {
        var r = _dbSet.Where(expression);
        return await r.ToListAsync();
    }

    public async Task<T?> SingleAsync(Expression<Func<T, bool>> expression)
    {
        var r = _dbSet.AsQueryable();

        return await r.FirstOrDefaultAsync(expression);
    }

    public async Task UpdateAsync(T entity)
    {
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T))) ((IAuditEntity)entity).UpdatedDate = DateTime.UtcNow;
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateRangeAsync(List<T> list)
    {
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            foreach (var e in list)
                ((IAuditEntity)e).UpdatedDate = DateTime.UtcNow;

        _dbSet.UpdateRange(list);
        await Task.CompletedTask;
    }

    public async Task UpdateRangeWithoutDateAsync(List<T> list)
    {
        _dbSet.UpdateRange(list);
        await Task.CompletedTask;
    }

    public IQueryable<T> Queriable()
    {
        return _dbSet;
    }
}