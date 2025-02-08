using Microsoft.EntityFrameworkCore.Storage;

namespace CroBooks.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();
    void ClearTracker();
}