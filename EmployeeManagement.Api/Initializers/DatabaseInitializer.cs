using EmployeeManagement.Api.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.Api.Initializers;

public class DatabaseInitializer
{
    private readonly DatabaseOptions _databaseOptions;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        IOptions<DatabaseOptions> databaseOptions,
        ILogger<DatabaseInitializer> logger)
    {
        _databaseOptions = databaseOptions.Value;
        _logger = logger;
    }

    public void CreateAndSeedDatabase()
    {
        try
        {
            CreateDatabaseIfNotExists();
            CreateEmployeeTableIfNotExists();
            SeedDataIfNotExists();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating or seeding the database.");

            throw;
        }
    }

    private void CreateDatabaseIfNotExists()
    {
        try
        {
            using var connection = new SqlConnection(_databaseOptions.ServerConnectionString);
            connection.Open();
            var commandText = @"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EmployeesDatabase')
                BEGIN
                    CREATE DATABASE EmployeesDatabase;
                END;
            ";

            using var command = new SqlCommand(commandText, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the database.");
        }
    }

    private void CreateEmployeeTableIfNotExists()
    {
        try
        {
            using var connection = new SqlConnection(_databaseOptions.DatabaseConnectionString);
            connection.Open();

            var commandText = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employees')
                BEGIN
                    CREATE TABLE Employees (
                        Id INT PRIMARY KEY,
                        Name NVARCHAR(100),
                        ManagerId INT,
                        Enable BIT
                    );
                END;
            ";

            using var command = new SqlCommand(commandText, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the Employees table.");
        }
    }

    private void SeedDataIfNotExists()
    {
        try
        {
            using var connection = new SqlConnection(_databaseOptions.DatabaseConnectionString);
            connection.Open();

            var commandText = @"
                IF NOT EXISTS (SELECT * FROM Employees)
                BEGIN
                    INSERT INTO Employees (Id, Name, ManagerId, Enable)
                    VALUES (1, 'Employee 1', NULL, 1),
                           (2, 'Employee 2', 1, 1),
                           (3, 'Employee 3', 1, 1),
                           (4, 'Employee 4', 2, 1),
                           (5, 'Employee 5', 4, 1),
                           (6, 'Employee 6', 5, 1);
                END;
            ";

            using var command = new SqlCommand(commandText, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding data into the Employees table.");
        }
    }
}
