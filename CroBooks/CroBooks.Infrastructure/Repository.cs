using CroBooks.Domain.Base;
using CroBooks.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace CroBooks.Infrastructure;

public class Repository<T, TKey> : IRepository<T, TKey> where T : class
{
    private readonly DbFactory _dbFactory;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private DbSet<T>? _dbSet;

    public Repository(DbFactory dbFactory, AuthenticationStateProvider authenticationStateProvider)
    {
        _dbFactory = dbFactory;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public Repository(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    protected DbSet<T> DbSet => _dbSet ?? (_dbSet = _dbFactory.DbContext.Set<T>());

    public async Task<T> AttachAsync(T entity)
    {
        return DbSet.Attach(entity).Entity;
    }

    public async Task<long> EntityCountAsync()
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            return await DbSet.Where(x => !((IDeleteEntity)x).IsDeleted).CountAsync();

        return await DbSet.CountAsync();
    }

    public async Task<bool> EntityExistsAsync(Func<T, bool> expression)
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
        {
            var r = DbSet.Where(x => !((IDeleteEntity)x).IsDeleted).AsEnumerable();
            return r.Any(expression);
        }

        return await Task.FromResult(DbSet.Any(expression));
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            return await DbSet.AsNoTracking().Where(x => !((IDeleteEntity)x).IsDeleted).ToListAsync();

        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllDeletedAsync()
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            return await DbSet.AsNoTracking().Where(x => ((IDeleteEntity)x).IsDeleted).ToListAsync();

        return new List<T>();
    }

    public async Task<IEnumerable<T>> GetAllIncludingDeletedAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> SudoGetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> FindAsync(TKey id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T))) ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;

        var newEntity = await DbSet.AddAsync(entity);
        return newEntity.Entity;
    }

    public async Task AddRangeAsync(List<T> list)
    {
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            foreach (var entity in list)
                ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;
        await DbSet.AddRangeAsync(list);
    }

    public async Task DeleteAsync(TKey id)
    {
        var e = await FindAsync(id);

        if (e == null) return;

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
        {
            ((IDeleteEntity)e).IsDeleted = true;
            ((IDeleteEntity)e).DeletedDate = DateTime.UtcNow;

            DbSet.Update(e);
            return;
        }

        DbSet.Remove(e);
    }

    public async Task DeleteAsync(T e)
    {
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
        {
            ((IDeleteEntity)e).IsDeleted = true;
            ((IDeleteEntity)e).DeletedDate = DateTime.UtcNow;

            DbSet.Update(e);
            return;
        }

        DbSet.Remove(e);
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
            DbSet.UpdateRange(entities);
        else
            DbSet.RemoveRange(entities);
    }

    public async Task SudoDeleteRangeAsync(List<TKey> list)
    {
        var entities = await SudoListAsync(x => list.Contains(((IEntityBase<TKey>)x).Id));
        DbSet.RemoveRange(entities);
    }

    public async Task RestoreAsync(T entity)
    {
        var e = await FindAsync(((IEntityBase<TKey>)entity).Id);

        if (e == null) return;

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
        {
            ((IDeleteEntity)e).IsDeleted = false;
            ((IDeleteEntity)e).DeletedDate = null;
            DbSet.Update(e);
        }
    }

    public async Task<IList<T>> ListAsync(Expression<Func<T, bool>> expression)
    {
        var r = DbSet.AsNoTracking().Where(expression);

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T))) r = r.Where(x => !((IDeleteEntity)x).IsDeleted);

        return await r.ToListAsync();
    }

    public async Task<IList<T>> ListIncludingDeletedAsync(Expression<Func<T, bool>> expression)
    {
        var r = DbSet.Where(expression);

        return await r.ToListAsync();
    }

    public async Task<IList<T>> SudoListAsync(Expression<Func<T, bool>> expression)
    {
        var r = DbSet.Where(expression);

        return await r.ToListAsync();
    }

    public async Task<T?> SingleAsync(Expression<Func<T, bool>> expression)
    {
        var r = DbSet.AsQueryable();

        return await r.FirstOrDefaultAsync(expression);
    }

    public async Task UpdateAsync(T entity)
    {
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T))) ((IAuditEntity)entity).UpdatedDate = DateTime.UtcNow;
        DbSet.Update(entity);
    }

    public async Task UpdateRangeAsync(List<T> list)
    {
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            foreach (var e in list)
                ((IAuditEntity)e).UpdatedDate = DateTime.UtcNow;

        DbSet.UpdateRange(list);
    }

    public async Task UpdateRangeWithoutDateAsync(List<T> list)
    {
        DbSet.UpdateRange(list);
    }

    public IQueryable<T> Queriable()
    {
        return DbSet;
    }
}