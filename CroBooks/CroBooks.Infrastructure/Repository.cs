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

public class Repository<T, TKey, TTennantKey, TTennantKeyNullable> : IRepository<T, TKey> where T : class
{
    private readonly IHttpContextAccessor? _contextAccessor;
    private readonly DbFactory _dbFactory;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private DbSet<T>? _dbSet;
    private TTennantKey _tennantId = default!;

    public Repository(DbFactory dbFactory, AuthenticationStateProvider authenticationStateProvider)
    {
        _dbFactory = dbFactory;
        _authenticationStateProvider = authenticationStateProvider;
        LoadUserFromAuthStateProvider();
    }

    public Repository(DbFactory dbFactory, IHttpContextAccessor contextAccessor)
    {
        _dbFactory = dbFactory;
        _contextAccessor = contextAccessor;
        LoadUser();
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
        if (typeof(IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            return await DbSet.Where(x =>
                    !((IDeleteEntity)x).IsDeleted && ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                    ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId)
                )
                .CountAsync();

        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            return await DbSet.Where(x =>
                    ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                    ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId))
                .CountAsync();

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            return await DbSet.Where(x => !((IDeleteEntity)x).IsDeleted).CountAsync();

        return await DbSet.CountAsync();
    }

    public async Task<bool> EntityExistsAsync(Func<T, bool> expression)
    {
        if (typeof(IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
        {
            var r = DbSet.Where(x =>
                    !((IDeleteEntity)x).IsDeleted && ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                    ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId))
                .AsEnumerable();
            return r.Any(expression);
        }

        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
        {
            var r = DbSet.Where(x =>
                ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId)).AsEnumerable();
            return r.Any(expression);
        }

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
        {
            var r = DbSet.Where(x => !((IDeleteEntity)x).IsDeleted).AsEnumerable();
            return r.Any(expression);
        }

        return await Task.FromResult(DbSet.Any(expression));
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        if (typeof(IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            return await DbSet.AsNoTracking().Where(x =>
                    !((IDeleteEntity)x).IsDeleted && ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                    ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId))
                .ToListAsync();

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            return await DbSet.AsNoTracking().Where(x => !((IDeleteEntity)x).IsDeleted).ToListAsync();

        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            return await DbSet.AsNoTracking()
                .Where(x => ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                            ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId)).ToListAsync();

        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllDeletedAsync()
    {
        if (typeof(IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            return await DbSet.AsNoTracking().Where(x =>
                    ((IDeleteEntity)x).IsDeleted && ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                    ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId))
                .ToListAsync();

        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            return await DbSet.AsNoTracking().Where(x => ((IDeleteEntity)x).IsDeleted).ToListAsync();

        return new List<T>();
    }

    public async Task<IEnumerable<T>> GetAllIncludingDeletedAsync()
    {
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            return await DbSet.AsNoTracking()
                .Where(x => ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                            ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId)).ToListAsync();
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
        if (typeof(IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>)entity).CreatedBy = _tennantId;
        if (typeof(IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable>)entity).CreatedBy = _tennantId;
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T))) ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            ((ITenantEntity<TKey, TTennantKey>)entity).TenantId = _tennantId;
        var newEntity = await DbSet.AddAsync(entity);
        return newEntity.Entity;
    }

    public async Task AddRangeAsync(List<T> list)
    {
        if (typeof(IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            foreach (var entity in list)
                ((IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>)entity).CreatedBy = _tennantId;
        if (typeof(IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            foreach (var entity in list)
                ((IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable>)entity).CreatedBy = _tennantId;
        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            foreach (var entity in list)
                ((IAuditEntity)entity).CreatedDate = DateTime.UtcNow;
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            foreach (var entity in list)
                ((ITenantEntity<TKey, TTennantKey>)entity).TenantId = _tennantId;
        await DbSet.AddRangeAsync(list);
    }

    public async Task DeleteAsync(TKey id)
    {
        var e = await FindAsync(id);

        if (e == null) return;

        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            if (!(((ITenantEntity<TKey, TTennantKey>)e).TenantId != null &&
                  ((ITenantEntity<TKey, TTennantKey>)e).TenantId!.Equals(_tennantId)))
                return;

        if (typeof(IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>)e).DeletedBy =
                (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                    .ConvertFromInvariantString(_tennantId!.ToString()!);

        if (typeof(IDeleteEntityWithUser<TKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IDeleteEntityWithUser<TKey, TTennantKeyNullable>)e).DeletedBy =
                (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                    .ConvertFromInvariantString(_tennantId!.ToString()!);

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
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            if (!(((ITenantEntity<TKey, TTennantKey>)e).TenantId != null &&
                  ((ITenantEntity<TKey, TTennantKey>)e).TenantId!.Equals(_tennantId)))
                return;

        if (typeof(IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>)e).DeletedBy =
                (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                    .ConvertFromInvariantString(_tennantId!.ToString()!);

        if (typeof(IDeleteEntityWithUser<TKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IDeleteEntityWithUser<TKey, TTennantKeyNullable>)e).DeletedBy =
                (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                    .ConvertFromInvariantString(_tennantId!.ToString()!);

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
            if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
                if (!(((ITenantEntity<TKey, TTennantKey>)entity).TenantId != null &&
                      ((ITenantEntity<TKey, TTennantKey>)entity).TenantId!.Equals(_tennantId)))
                    return;

            if (typeof(IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
                ((IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>)entity).DeletedBy =
                    (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                        .ConvertFromInvariantString(_tennantId!.ToString()!);

            if (typeof(IDeleteEntityWithUser<TKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
                ((IDeleteEntityWithUser<TKey, TTennantKeyNullable>)entity).DeletedBy =
                    (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                        .ConvertFromInvariantString(_tennantId!.ToString()!);

            if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            {
                ((IDeleteEntity)entity).IsDeleted = true;
                ((IDeleteEntity)entity).DeletedDate = DateTime.UtcNow;
            }
        }

        if (typeof(IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)) ||
            typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
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

        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            if (!(((ITenantEntity<TKey, TTennantKey>)e).TenantId != null &&
                  ((ITenantEntity<TKey, TTennantKey>)e).TenantId!.Equals(_tennantId)))
                return;

        if (typeof(IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>)e).DeletedBy = default;

        if (typeof(IDeleteEntityWithUser<TKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IDeleteEntityWithUser<TKey, TTennantKeyNullable>)e).DeletedBy = default;

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
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            r = r.Where(x =>
                ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId));
        if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T))) r = r.Where(x => !((IDeleteEntity)x).IsDeleted);

        return await r.ToListAsync();
    }

    public async Task<IList<T>> ListIncludingDeletedAsync(Expression<Func<T, bool>> expression)
    {
        var r = DbSet.Where(expression);
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            r = r.Where(x =>
                ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId));

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
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            r = r.Where(x =>
                ((ITenantEntity<TKey, TTennantKey>)x).TenantId != null &&
                ((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId));

        return await r.FirstOrDefaultAsync(expression);
    }

    public async Task UpdateAsync(T entity)
    {
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            if (!(((ITenantEntity<TKey, TTennantKey>)entity).TenantId != null &&
                  ((ITenantEntity<TKey, TTennantKey>)entity).TenantId!.Equals(_tennantId)))
                return;

        if (typeof(IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>)entity).UpdatedBy =
                (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                    .ConvertFromInvariantString(_tennantId!.ToString()!);

        if (typeof(IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            ((IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable>)entity).UpdatedBy =
                (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                    .ConvertFromInvariantString(_tennantId!.ToString()!);

        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T))) ((IAuditEntity)entity).UpdatedDate = DateTime.UtcNow;
        DbSet.Update(entity);
    }

    public async Task UpdateRangeAsync(List<T> list)
    {
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            if (!list.Any(x => !((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId)))
                return;

        if (typeof(IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            foreach (var e in list)
                ((IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>)e).UpdatedBy =
                    (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                        .ConvertFromInvariantString(_tennantId!.ToString()!);

        if (typeof(IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable>).IsAssignableFrom(typeof(T)))
            foreach (var e in list)
                ((IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable>)e).UpdatedBy =
                    (TTennantKeyNullable?)TypeDescriptor.GetConverter(typeof(TTennantKeyNullable))
                        .ConvertFromInvariantString(_tennantId!.ToString()!);

        if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
            foreach (var e in list)
                ((IAuditEntity)e).UpdatedDate = DateTime.UtcNow;

        DbSet.UpdateRange(list);
    }

    public async Task UpdateRangeWithoutDateAsync(List<T> list)
    {
        if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)))
            if (!list.Any(x => !((ITenantEntity<TKey, TTennantKey>)x).TenantId!.Equals(_tennantId)))
                return;
        DbSet.UpdateRange(list);
    }

    public IQueryable<T> Queriable()
    {
        return DbSet;
    }

    private void LoadUser()
    {
        var t = typeof(TTennantKey);
        TTennantKey? possibleTenantId;
        if (t == typeof(Guid))
        {
            if (typeof(ITenantEntity<TKey, TTennantKey>).IsAssignableFrom(typeof(T)) &&
                _contextAccessor?.HttpContext?.User != null)
            {
                var identifier = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (identifier == null) throw new NullReferenceException("User Identifier couldn't be found.");
                possibleTenantId = (TTennantKey?)TypeDescriptor.GetConverter(typeof(TTennantKey))
                    .ConvertFromInvariantString(identifier);
                if (possibleTenantId == null) throw new NullReferenceException("Tenant ID is null.");
                _tennantId = possibleTenantId;
                return;
            }

            possibleTenantId = (TTennantKey?)TypeDescriptor.GetConverter(typeof(TTennantKey))
                .ConvertFromInvariantString(Guid.Empty.ToString());
            if (possibleTenantId == null) throw new NullReferenceException("Tenant ID is null.");
            _tennantId = possibleTenantId;
        }
        else if (t == typeof(string))
        {
            var UserName = _contextAccessor?.HttpContext.User?.Identity?.Name;
            if (UserName == null) throw new NullReferenceException("Username couldn't be found.");
            possibleTenantId = (TTennantKey?)TypeDescriptor.GetConverter(typeof(TTennantKey))
                .ConvertFromInvariantString(UserName);
            if (possibleTenantId == null) throw new NullReferenceException("Tenant ID is null.");
            _tennantId = possibleTenantId;
        }
    }

    private async Task LoadUserFromAuthStateProvider()
    {
        TTennantKey? tennantKey;
        var username = string.Empty;
        var authstate = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authstate.User;
        if (user.Claims.Any())
        {
            username = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        }

        var t = typeof(TTennantKey);
        if (t == typeof(string))
        {
            tennantKey = (TTennantKey?)TypeDescriptor.GetConverter(typeof(TTennantKey))
                .ConvertFromInvariantString(username);
            if (tennantKey == null) throw new NullReferenceException("Tenant ID is null.");
            _tennantId = tennantKey;
        }

    }
}