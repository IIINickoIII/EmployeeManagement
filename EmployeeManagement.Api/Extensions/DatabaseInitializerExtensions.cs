using EmployeeManagement.Api.Initializers;
using EmployeeManagement.Api.Options;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.Api.Extensions;

public static class DatabaseInitializerExtensions
{
    public static void UseDatabaseInitializer(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            var databaseOptions = services.GetRequiredService<IOptions<DatabaseOptions>>();
            var logger = services.GetRequiredService<ILogger<DatabaseInitializer>>();
            var databaseInitializer = new DatabaseInitializer(databaseOptions, logger);
            databaseInitializer.CreateAndSeedDatabase();
        }
    }
}
