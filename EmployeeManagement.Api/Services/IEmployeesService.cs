using EmployeeManagement.Api.Entities;

namespace EmployeeManagement.Api.Services;

public interface IEmployeesService
{
    Task<EmployeeDto> GetEmployeeById(
        int employeeId,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateEmployeeEnabledStatus(
        int employeeId,
        bool isEnabled,
        CancellationToken cancellationToken = default);
}
