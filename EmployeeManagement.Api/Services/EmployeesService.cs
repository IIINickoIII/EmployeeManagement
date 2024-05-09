using EmployeeManagement.Api.Entities;
using EmployeeManagement.Api.Repositories;

namespace EmployeeManagement.Api.Services;

public class EmployeesService : IEmployeesService
{
    private readonly ILogger<EmployeesService> _logger;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesService(
        ILogger<EmployeesService> logger,
        IEmployeeRepository employeeRepository)
    {
        _logger = logger;
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDto> GetEmployeeById(
        int employeeId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _employeeRepository.GetEmployeeById(
                employeeId,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting Employee with Id = {employeeId}.");

            return new EmployeeDto();
        }
    }

    public async Task<bool> UpdateEmployeeEnabledStatus(
        int employeeId,
        bool isEnabled,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _employeeRepository.UpdateEmployeeEnabledStatus(
                employeeId,
                isEnabled,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting Employee with Id = {employeeId}.");

            return false;
        }
    }
}
