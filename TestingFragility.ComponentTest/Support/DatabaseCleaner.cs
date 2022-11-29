using System.Threading.Tasks;
using BFF.Support.Database;
using Microsoft.EntityFrameworkCore;

namespace BFF.Component.Tests.Support;

public class DatabaseCleaner{
    private readonly BffDb _db;

    public DatabaseCleaner(BffDb db) => _db = db;

    public void CleanDb() => CleanDbAsync().Wait();

    private async Task CleanDbAsync() => await _db.Database.ExecuteSqlRawAsync("DELETE FROM Users;");
}