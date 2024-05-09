using EmployeeManagement.Api.Entities;

namespace EmployeeManagement.Api.Repositories;

public interface IEmployeeRepository
{
    Task<EmployeeDto> GetEmployeeById(
        int employeeId,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateEmployeeEnabledStatus(
        int employeeId,
        bool isEnabled,
        CancellationToken cancellationToken = default);
}
