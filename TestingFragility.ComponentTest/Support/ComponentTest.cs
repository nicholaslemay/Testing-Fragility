using System;
using System.Net.Http;
using BFF.Database.Migrations;
using BFF.Support.Database;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Server;
using Xunit;

namespace BFF.Component.Tests.Support;

public class CompontentTestFixture : IDisposable
{
    public CompontentTestFixture()
    {
        _application = new BffComponentTestApplication();
        Client = _application.CreateClient();
        Db = _application.Services.CreateScope().ServiceProvider.GetRequiredService<BffDb>();
        DatabaseCleaner = new DatabaseCleaner(Db);
        BffDatabaseMigration = new BffDatabaseMigration(Db);
        FakeCommunicationService = WireMockServer.Start(777);
    }

    public void Dispose()
    {
        FakeCommunicationService.Stop();
        _application.Dispose();
    }

    public readonly HttpClient Client;
    private readonly BffComponentTestApplication _application;
    public readonly DatabaseCleaner DatabaseCleaner;
    public readonly BffDb Db;
    public readonly BffDatabaseMigration BffDatabaseMigration;
    public readonly WireMockServer FakeCommunicationService;
}

[CollectionDefinition("ComponentTest")]
public class ComponentTestCollection : ICollectionFixture<CompontentTestFixture> { }

[Collection("ComponentTest")]
public abstract class ComponentTest
{
    protected readonly HttpClient Client;
    protected readonly BffDb Db;
    protected readonly WireMockServer FakeCommunicationService;

    protected ComponentTest(CompontentTestFixture fixture)
    {
        Client = fixture.Client;
        Db = fixture.Db;
        FakeCommunicationService = fixture.FakeCommunicationService;
        
        FakeCommunicationService.ResetLogEntries();
        fixture.BffDatabaseMigration.MigrateAsync();
        fixture.DatabaseCleaner.CleanDb();
    }
}