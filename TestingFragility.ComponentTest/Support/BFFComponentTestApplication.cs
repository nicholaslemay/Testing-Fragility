using System.IO;
using System.Linq;
using BFF.Support.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static BFF.Component.Tests.Support.Database.BffComponentTestDbHelper;

namespace BFF.Component.Tests.Support;

public class BffComponentTestApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile($"{Directory.GetCurrentDirectory()}/../../../Support/appsettings.componenttests.json");
        });
        
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<BffDb>));
            services.Remove(descriptor);
            services.AddSqlite<BffDb>($"Data Source={DatabaseFolderLocation}bff.db;Cache=Shared");
        });
        return base.CreateHost(builder);
    }
}