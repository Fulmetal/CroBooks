using CroBooks.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CroBooks.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbFactory _dbFactory;

    public UnitOfWork(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public void ClearTracker()
    {
        _dbFactory.DbContext.ChangeTracker.Clear();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbFactory.DbContext.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbFactory.DbContext.Database.BeginTransactionAsync();
    }
}