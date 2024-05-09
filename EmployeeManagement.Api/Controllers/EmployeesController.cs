using EmployeeManagement.Api.Entities;
using EmployeeManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly IEmployeesService _employeesService;

    public EmployeesController(
        ILogger<EmployeesController> logger,
        IEmployeesService employeesService)
    {
        _logger = logger;
        _employeesService = employeesService;
    }

    /// <summary>
    /// Get an employee with subordinates by employee id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = "GetEmployeeById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(
        [Required, FromRoute] int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var employee = await _employeesService
                .GetEmployeeById(id, cancellationToken);

            if (employee == null)
            {
                return NotFound($"Employee with Id = {id} wasn't found.");
            }

            return Ok(employee);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message, exception);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update employee status by employee id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isEnabled"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id:int}/status", Name = "UpdateEmployeeStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEmployeeStatus(
        [Required, FromRoute] int id,
        [Required, FromBody] bool isEnabled,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var updated = await _employeesService.UpdateEmployeeEnabledStatus(
                id,
                isEnabled,
                cancellationToken);

            if (updated)
            {
                return NoContent();
            }
            else
            {
                return NotFound($"Employee with Id = {id} wasn't updated.");
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message, exception);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
