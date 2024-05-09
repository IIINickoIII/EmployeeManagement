using Microsoft.Extensions.Options;

namespace EmployeeManagement.Api.Options;

public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private const string ConfigurationSectionName = nameof(DatabaseOptions);
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(DatabaseOptions options)
    {
        var databaseConnectionString = _configuration.GetConnectionString("EmployeesDatabase");
        options.DatabaseConnectionString = databaseConnectionString;

        _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
