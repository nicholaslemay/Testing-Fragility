using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BFF.Support.Database;

public class BffDb : DbContext
{
    public BffDb(DbContextOptions<BffDb> options) : base(options) { }
    
    public DbSet<UserRecord> Users => Set<UserRecord>(); 
}

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
}
public class UnitOfWork : IUnitOfWork
{
    private readonly BffDb _database;
    private IDbContextTransaction _currentTransaction;

    public UnitOfWork(BffDb Database)
    {
        _database = Database;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await _database.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null) throw new InvalidOperationException($"No current transaction");

        try
        {
            await _database.SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}



public class UserRecord 
{
    public int Id { get; set; }
    public string Email { get; init; } = null!;
     public string Name { get; init; } = null!;
     //public string Status { get; set; } = null!;
     public string Gender { get; init; } = null!;
}