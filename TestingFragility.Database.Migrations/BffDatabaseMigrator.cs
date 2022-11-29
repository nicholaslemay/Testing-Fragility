using BFF.Support.Database;
using Microsoft.EntityFrameworkCore;

namespace BFF.Database.Migrations;

public class BffDatabaseMigration
{
    private readonly BffDb _db;

    public BffDatabaseMigration(BffDb db) => _db = db;

    public async Task MigrateAsync()
    {
        Console.WriteLine("Migrating database...");
        await _db.Database.MigrateAsync();
        Console.WriteLine("Migrated database");
    }
}