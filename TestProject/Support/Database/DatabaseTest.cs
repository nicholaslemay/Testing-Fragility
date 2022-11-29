using System;
using System.Linq;
using System.Threading.Tasks;
using BFF.Database.Migrations;
using BFF.Support.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace BFF.Tests.Support.Database;

public class DatabaseCleaner{
    private readonly BffDb _db;

    public DatabaseCleaner(BffDb db) => _db = db;

    public void CleanDb() => CleanDbAsync().Wait();

    private async Task CleanDbAsync() => await _db.Database.ExecuteSqlRawAsync("DELETE FROM Users;");
}
public class BFFDatabaseTestApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<BffDb>));
            services.Remove(descriptor);
            services.AddSqlite<BffDb>($"Data Source={BffTestDbHelper.DatabaseFolderLocation}bff.db;Cache=Shared");
        });
        builder.UseEnvironment("DatabaseMigration");
        return base.CreateHost(builder);
    }
}
public class DatabaseTestFixture : IDisposable
{
    public DatabaseTestFixture()
    {
        _application = new BFFDatabaseTestApplication();
        Db = GetNewDBConnection();
        DatabaseCleaner = new DatabaseCleaner(Db);
        DatabaseMigration = new BffDatabaseMigration(Db);
    }

    public BffDb GetNewDBConnection() => _application.Services.CreateScope().ServiceProvider.GetRequiredService<BffDb>();

    public void Dispose()
    {
        _application.Dispose();
    }

    private readonly BFFDatabaseTestApplication _application;
    public readonly DatabaseCleaner DatabaseCleaner;
    public readonly BffDb Db;
    public readonly BffDatabaseMigration DatabaseMigration;
}

[CollectionDefinition("DatabaseTest")]
public class DatabaseTestCollection : ICollectionFixture<DatabaseTestFixture> { }

[Collection("DatabaseTest")]
public abstract class DatabaseTest
{
    protected readonly BffDb Db;

    protected DatabaseTest(DatabaseTestFixture fixture)
    {
        Db = fixture.Db;
        fixture.DatabaseMigration.MigrateAsync().Wait();
        fixture.DatabaseCleaner.CleanDb();
    }
}