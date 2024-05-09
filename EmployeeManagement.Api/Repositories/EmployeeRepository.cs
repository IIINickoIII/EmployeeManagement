using EmployeeManagement.Api.Entities;
using EmployeeManagement.Api.Repositories;
using Microsoft.Data.SqlClient;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _databaseConnectionString;

    public EmployeeRepository(IConfiguration configuration)
    {
        _databaseConnectionString = configuration.GetConnectionString("EmployeesDatabase");
    }

    public async Task<EmployeeDto> GetEmployeeById(
        int employeeId,
        CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_databaseConnectionString);

        await connection.OpenAsync();

        var employees = await GetEmployeesRecursive(connection, employeeId, cancellationToken);

        return employees;
    }

    public async Task<bool> UpdateEmployeeEnabledStatus(
        int employeeId,
        bool isEnabled,
        CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_databaseConnectionString);

        await connection.OpenAsync(cancellationToken);

        using var command = connection.CreateCommand();

        command.CommandText = @"
            UPDATE Employees
            SET Enable = @NewStatus
            WHERE Id = @EmployeeId
        ";
        command.Parameters.AddWithValue("@NewStatus", isEnabled);
        command.Parameters.AddWithValue("@EmployeeId", employeeId);

        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private async Task<EmployeeDto> GetEmployeesRecursive(
        SqlConnection connection,
        int employeeId,
        CancellationToken cancellationToken = default)
    {
        var employee = await GetEmployeeByIdAsync(connection, employeeId, cancellationToken);

        if (employee == null)
            return null;

        employee.Employees = await GetSubordinatesRecursive(connection, employeeId, cancellationToken);

        return employee;
    }

    private async Task<List<EmployeeDto>> GetSubordinatesRecursive(
        SqlConnection connection,
        int managerId,
        CancellationToken cancellationToken = default)
    {
        var subordinates = new List<EmployeeDto>();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Name, ManagerId, Enable
            FROM Employees
            WHERE ManagerId = @ManagerId
        ";
        command.Parameters.AddWithValue("@ManagerId", managerId);

        using (var reader = await command.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                var subordinateId = (int)reader["Id"];
                var subordinate = await GetEmployeesRecursive(connection, subordinateId, cancellationToken);
                subordinates.Add(subordinate);
            }
        }

        return subordinates;
    }

    private async Task<EmployeeDto> GetEmployeeByIdAsync(
        SqlConnection connection,
        int employeeId,
        CancellationToken cancellationToken = default)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Name, ManagerId, Enable
            FROM Employees
            WHERE Id = @EmployeeId
        ";
        command.Parameters.AddWithValue("@EmployeeId", employeeId);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return new EmployeeDto
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                ManagerId = reader["ManagerId"] == DBNull.Value
                    ? null
                    : (int?)reader["ManagerId"],
                Enable = (bool)reader["Enable"]
            };
        }
        else
        {
            return null;
        }
    }
}
