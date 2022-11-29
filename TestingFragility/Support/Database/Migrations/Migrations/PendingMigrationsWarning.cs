using Microsoft.EntityFrameworkCore;

namespace BFF.Support.Database.Migrations;

public static class PendingMigrationsWarning
{
    public static  WebApplication ValidateNoPendingMigrations(this WebApplication app)
    {
        using var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<BffDb>();
        if (db.Database.IsRelational() && db.Database.GetPendingMigrations().Any())
        {
            app.Logger.LogWarning("*************************************************************");
            app.Logger.LogWarning("There are pending migration. Run BFF.Database.Migrations to migrate DB");
            app.Logger.LogWarning("*************************************************************");

            if (!app.Environment.IsEnvironment("DatabaseMigration"))
            {
                throw new Exception("There are pending migration. Run BFF.Database.Migrations to migrate DB");
            }

        }
        return app;
    }
}