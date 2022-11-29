using BFF.Communications;
using BFF.Support.Database;
using BFF.Support.Database.Migrations;
using BFF.Users;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<Program>(lifetime: ServiceLifetime.Transient);

builder.Services.AddSqlite<BffDb>($"Data Source={DBHelper.DatabaseFolderLocation}bff.db;Cache=Shared");

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddHttpClient<ICommunicationServiceClient, CommunicationServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["CommunicationServiceBaseUrl"]);
});

var app = builder.Build();

app.ValidateNoPendingMigrations()
    .MapUserEndpoints()
    .Run();

namespace BFF
{
    public partial class Program { }
}